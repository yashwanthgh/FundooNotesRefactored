using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.CollaborationModel
{
    public class CollaborationResponseModel
    {
        public int CollaborationID { get; set; }
        public string? CollaborationEmail { get; set; }
        public int NoteId { get; set; }
    }
}
