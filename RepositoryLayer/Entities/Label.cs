using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class Label
    {
        public int LabelId { get; set; }
        public string? LabelName { get; set; }
        public int UserId { get; set; }
    }
}
