using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.NoteModel
{
    public class CreateNoteResponseModel
    {
        public int NoteId { get; set; }
        public int LabelId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
