using ModelLayer.NoteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRL
    {
        public Task<CreateNoteResponseModel> CreateNote(CreateNoteModel model, int labelId);
        public Task<IEnumerable<NoteResponseModel>> GetAllNotesInLabel(int labelId);
        public Task<IEnumerable<NoteResponseModel>> GetAllTrashNotes();
        public Task<IEnumerable<NoteResponseModel>> GetAllArchivedNotes();
        public Task<CreateNoteResponseModel> UpdateNote(UpdateNoteModel model, int noteId);
        public Task MoveToTrash(int noteId);
        public Task RetriveFromTrash(int noteId);
        public Task MoveToArchive(int noteId);
        public Task RemoveFromArchive(int noteId);
    }
}
