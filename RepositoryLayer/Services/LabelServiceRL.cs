using Dapper;
using ModelLayer.LabelModel;
using RepositoryLayer.Interfaces;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Contexts;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Services
{
    public class LabelServiceRL : ILabelRL
    {
        private readonly DapperContext _context;

        public LabelServiceRL(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateLabel(LabelCreateModel model, int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("LabelName", model.LabelName, DbType.String);
            parameters.Add("UserId", userId, DbType.Int64);

            int labelId;
            using (var connection = _context.CreateConnection())
            {
                labelId = await connection.ExecuteScalarAsync<int>("spCreateLabel", parameters, commandType: CommandType.StoredProcedure);
            }
            return labelId;
        }

        public async Task<Label> GetLabel(int userId, int labelId)
        {
            Label label;
            using (var connection = _context.CreateConnection())
            {
                label = await connection.QueryFirstAsync<Label>("spGetLabelByLabelId", new { labelId, userId }, commandType: CommandType.StoredProcedure);
            }

            if (label != null)
            {
                return label;
            }
            throw new Exception("Label not found!");
        }

        public async Task<IEnumerable<Label>> GetAllLabels(int userId)
        {
            IEnumerable<Label> label;
            using (var connection = _context.CreateConnection())
            {
                label = await connection.QueryAsync<Label>("spGetAllLabels",new { userId }, commandType: CommandType.StoredProcedure);
            }

            if (label != null)
            {
                return label.ToList();
            }
            throw new Exception("Labels not found!");
        }

        public async Task<bool> DeleteLabel(int labelId)
        {
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("spDeleteLabelById", labelId);
            }
            return true;
        }
    }
}
