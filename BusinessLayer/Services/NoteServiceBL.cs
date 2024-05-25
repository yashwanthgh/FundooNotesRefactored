using BusinessLayer.Interfaces;
using ModelLayer.NoteModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteServiceBL : INoteBL
    {
        public readonly INoteRL _note;

        public NoteServiceBL(INoteRL note)
        {
            _note = note;
        }

        public async Task<CreateNoteResponseModel> CreateNote(CreateNoteModel model, int labelId)
        {
            return await _note.CreateNote(model, labelId);
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllArchivedNotes()
        {
            return await _note.GetAllArchivedNotes();
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllNotesInLabel(int labelId)
        {
            return await _note.GetAllNotesInLabel(labelId);
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllTrashNotes()
        {
            return await _note.GetAllTrashNotes();
        }

        public Task MoveToArchive(int noteId)
        {
            return (_note.MoveToArchive(noteId));
        }

        public Task MoveToTrash(int noteId)
        {
            return (_note.MoveToTrash(noteId));
        }

        public Task RemoveFromArchive(int noteId)
        {
            return _note.RemoveFromArchive(noteId);
        }

        public Task RetriveFromTrash(int noteId)
        {
            return _note.RetriveFromTrash(noteId);
        }

        public async Task<CreateNoteResponseModel> UpdateNote(UpdateNoteModel model, int noteId)
        {
            return await _note.UpdateNote(model, noteId);
        }
    }
}
