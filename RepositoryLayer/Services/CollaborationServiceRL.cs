using Dapper;
using ModelLayer.CollaborationModel;
using ModelLayer.EmailModel;
using RepositoryLayer.Contexts;
using RepositoryLayer.Interfaces;
using System.Data;
using System.Text.RegularExpressions;
using RepositoryLayer.GlobalExceptions;
using System.Net.Mail;
using System.Net;
using System.Reflection;

namespace RepositoryLayer.Services
{

    public class CollaborationServiceRL : ICollaborationRL
    {
        private readonly DapperContext _context;
        private readonly IEmail _email;

        public CollaborationServiceRL(DapperContext context, IEmail email)
        {
            _context = context;
            _email = email;
        }

        public async Task<bool> AddCollaboration(int userId, CreateCollaborationModel model)
        {
            if(model == null) { throw  new ArgumentNullException("model"); }
            if(model.CollaborationEmail == null || !IsEmailValid(model.CollaborationEmail)) { throw new Exception("Email should not be null"); }
            
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int64);
            parameters.Add("NoteId", model.NoteId, DbType.Int64);
            parameters.Add("CollaborationEmail", model.CollaborationEmail, DbType.String);

            using(var connection  = _context.CreateConnection())
            {
                var getEmailUsingUserId = await connection.QueryFirstOrDefaultAsync<string>("spGetEmailByUserId", new { UserId = userId}, commandType: CommandType.StoredProcedure);  
                var ifEmailExists = await connection.QueryFirstOrDefaultAsync<bool>("spCheckEmailExists", new { Email = getEmailUsingUserId }, commandType: CommandType.StoredProcedure);
                if(!ifEmailExists)
                {
                    throw new InvalidEmailException("Email don't exist, Register first!");
                }
                bool ifCollaborationExists = await connection.QueryFirstOrDefaultAsync<bool>("spCheckAlreadyCollaborated", new { Email = model.CollaborationEmail, model.NoteId }, commandType: CommandType.StoredProcedure);
                if(ifCollaborationExists)
                {
                    throw new Exception("Collaboration already exists for the notes");
                }
                await connection.ExecuteAsync("spAddCollaboration", parameters, commandType: CommandType.StoredProcedure);
                var userInfo = connection.QueryFirstOrDefault("spGetUserInfoByEmail", new { Email = getEmailUsingUserId }, commandType: CommandType.StoredProcedure);
                if (userInfo != null)
                {
                    return await _email.SendEmail(model.CollaborationEmail, "Collaboration", $"{userInfo.FirstName}, {getEmailUsingUserId} added you as a Collaborator!");
                }
                return false;
            }
        }

        public async Task<IEnumerable<CollaborationResponseModel>> GetAllCollaborations(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<CollaborationResponseModel>("spGetAllCollaborations",new { userId }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task RemoveCollaboration(int collaborationId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("CollaborationId", collaborationId, DbType.Int64);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("spDeleteCollaborationById", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        private static bool IsEmailValid(string? email)
        {
            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (email == null) return false;
            return Regex.IsMatch(email, pattern);
        }
    }
}
