using ModelLayer.LabelModel;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRL
    {
        public Task<int> CreateLabel(LabelCreateModel model, int userId);
        public Task<Label> GetLabel(int userId, int labelId);
        public Task<IEnumerable<Label>> GetAllLabels(int userId);
        public Task<bool> DeleteLabel(int labelId);
    }
}
