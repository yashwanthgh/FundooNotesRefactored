using BusinessLayer.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.NoteModel;
using RepositoryLayer.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteServiceBL : INoteBL
    {
        private readonly INoteRL _note;
        private readonly IDistributedCache _cache;

        public NoteServiceBL(INoteRL note, IDistributedCache cache)
        {
            _note = note;
            _cache = cache;
        }

        public async Task<CreateNoteResponseModel> CreateNote(CreateNoteModel model, int labelId)
        {
            var newNote = await _note.CreateNote(model, labelId);

            var labelNotesCacheKey = $"NotesInLabel-{labelId}";
            await _cache.RemoveAsync(labelNotesCacheKey);

            return newNote;
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllArchivedNotes()
        {
            var cacheKey = "ArchivedNotes";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var archivedNotes = JsonSerializer.Deserialize<IEnumerable<NoteResponseModel>>(cachedData);
                if (archivedNotes != null)
                    return archivedNotes;
            }

            var notes = await _note.GetAllArchivedNotes();

            if (notes != null)
            {
                var serializedNote = JsonSerializer.Serialize(notes);
                await _cache.SetStringAsync(cacheKey, serializedNote);
                return notes;
            }
            else
            {
                throw new Exception("Unable to fetch data from Archives");
            }
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllNotesInLabel(int labelId)
        {
            var cacheKey = $"NotesInLabel-{labelId}";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var notesInLabel = JsonSerializer.Deserialize<IEnumerable<NoteResponseModel>>(cachedData);
                if (notesInLabel != null)
                    return notesInLabel;
            }

            var notes = await _note.GetAllNotesInLabel(labelId);

            if (notes != null)
            {
                var serializedNote = JsonSerializer.Serialize(notes);
                await _cache.SetStringAsync(cacheKey, serializedNote);
                return notes;
            }
            else
            {
                throw new Exception("Unable to fetch Notes from Label");
            }
        }

        public async Task<IEnumerable<NoteResponseModel>> GetAllTrashNotes()
        {
            var cacheKey = "TrashNotes";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var trashNotes = JsonSerializer.Deserialize<IEnumerable<NoteResponseModel>>(cachedData);
                if (trashNotes != null)
                    return trashNotes;
            }

            var notes = await _note.GetAllTrashNotes();

            if (notes != null)
            {
                var serializedNote = JsonSerializer.Serialize(notes);
                await _cache.SetStringAsync(cacheKey, serializedNote);
                return notes;
            }
            else
            {
                throw new Exception("Unable to fetch data from Trash");
            }
        }

        public async Task MoveToArchive(int noteId)
        {
            await _note.MoveToArchive(noteId);
            var note = await _note.GetNoteById(noteId);
            var labelId = note.LabelId;

            await _cache.RemoveAsync($"NotesInLabel-{labelId}");

            await _cache.RemoveAsync("ArchivedNotes");
        }

        public async Task MoveToTrash(int noteId)
        {
            await _note.MoveToTrash(noteId);

            var note = await _note.GetNoteById(noteId);
            var labelId = note.LabelId;

            await _cache.RemoveAsync($"NotesInLabel-{labelId}");

            await _cache.RemoveAsync("TrashNotes");
        }

        public async Task RemoveFromArchive(int noteId)
        {
            await _note.RemoveFromArchive(noteId);

            var note = await _note.GetNoteById(noteId);
            var labelId = note.LabelId;

            await _cache.RemoveAsync($"NotesInLabel-{labelId}");

            await _cache.RemoveAsync("ArchivedNotes");
        }

        public async Task RetrieveFromTrash(int noteId)
        {
            await _note.RetrieveFromTrash(noteId);

            var note = await _note.GetNoteById(noteId);
            var labelId = note.LabelId;

            await _cache.RemoveAsync($"NotesInLabel-{labelId}");

            await _cache.RemoveAsync("TrashNotes");
        }

        public async Task<CreateNoteResponseModel> UpdateNote(UpdateNoteModel model, int noteId)
        {
            var updatedNote = await _note.UpdateNote(model, noteId);

            var labelId = updatedNote.LabelId;

            await _cache.RemoveAsync($"NotesInLabel-{labelId}");

            return updatedNote;
        }
    }
}

