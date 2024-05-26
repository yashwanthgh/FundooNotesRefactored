using ModelLayer.LabelModel;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBL
    {
        public Task<int> CreateLabel(LabelCreateModel model, int userId);
        public Task<Label> GetLabel(int userId, int labelId);
        public Task<IEnumerable<Label>> GetAllLabels(int userId);
        public Task<bool> DeleteLabel(int labelId);
    }
}
