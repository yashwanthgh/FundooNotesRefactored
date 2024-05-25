using Dapper;
using System.Data;
using ModelLayer.NoteModel;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Contexts;

namespace RepositoryLayer.Services
{
    public class NoteServiceRL : INoteRL
    {
        private readonly DapperContext _context;

        public NoteServiceRL(DapperContext context)
        {
            _context = context;
        }

        public async Task<CreateNoteResponseModel> CreateNote(CreateNoteModel model, int labelId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Title", model.Title, DbType.String);
            parameters.Add("Description", model.Description, DbType.String);
            parameters.Add("Colour", model.Color, DbType.String);
            parameters.Add("IsArchived", model.IsArchived, DbType.Boolean);
            parameters.Add("IsDeleted", model.IsDeleted, DbType.Boolean);
            parameters.Add("LabelId", labelId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                int noteId = await connection.ExecuteScalarAsync<int>("spCreateNote", parameters, commandType: CommandType.StoredProcedure);

                var insertedNote = await connection.QueryFirstOrDefaultAsync<CreateNoteResponseModel>("spGetNotesByNoteId", new { NoteID = noteId }, commandType: CommandType.StoredProcedure);

                if (insertedNote == null)
                {
                    throw new Exception("Failed to retrieve the inserted note.");
                }
                return insertedNote;
            }
        }

        public async Task MoveToTrash(int noteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NoteId", noteId, DbType.Int64);
            parameters.Add("IsDeleted", true);
            parameters.Add("TimeRemaining", DateTime.UtcNow.AddDays(30), DbType.DateTime);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("spMoveNoteToTrash", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task RetriveFromTrash(int noteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NoteId", noteId, DbType.Int64);
            parameters.Add("IsDeleted", true);
            parameters.Add("TimeRemaining", null);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("spRetriveNoteFromTrash", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task MoveToArchive(int noteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NoteId", noteId, DbType.Int64);
            parameters.Add("IsArchived", true);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("spMoveorRemoveNoteArchive", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task RemoveFromArchive(int noteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NoteId", noteId, DbType.Int64);
            parameters.Add("IsArchived", false);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync("spMoveorRemoveNoteArchive", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllNotesInLabel(int labelId)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<NoteResponseModel>("spGetAllNotesByLabelId", labelId, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllTrashNotes()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<NoteResponseModel>("spGetAllTrashNotes", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllArchivedNotes()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<NoteResponseModel>("spGetAllArchivedNotes", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<CreateNoteResponseModel> UpdateNote(UpdateNoteModel model, int noteId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("NoteID", noteId, DbType.Int32);
            parameters.Add("Title", model.Title, DbType.String);
            parameters.Add("Description", model.Description, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var updatedNote = await connection.QueryFirstOrDefaultAsync<CreateNoteResponseModel>("spUpdateNote", parameters, commandType: CommandType.StoredProcedure );

                if (updatedNote == null)
                {
                    throw new Exception("Failed to retrieve the updated note.");
                }
                return updatedNote;
            }

        }
    }
}
