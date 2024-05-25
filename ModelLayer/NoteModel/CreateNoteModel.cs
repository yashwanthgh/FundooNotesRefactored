using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.NoteModel
{
    public class CreateNoteModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; } = "Black";
        public bool IsArchived { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime? TimeRemaining { get; set; } = null;
    }
}
