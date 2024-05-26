using BusinessLayer.Interfaces;
using ModelLayer.LabelModel;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class LabelServiceBL : ILabelBL
    {
        private readonly ILabelRL _label;

        public LabelServiceBL(ILabelRL label)
        {
            _label = label;
        }

        public async Task<int> CreateLabel(LabelCreateModel model, int userId)
        {
            return await _label.CreateLabel(model, userId);
        }

        public async Task<bool> DeleteLabel(int labelId)
        {
            return await _label.DeleteLabel(labelId);
        }

        public async Task<IEnumerable<Label>> GetAllLabels(int userId)
        {
            return await _label.GetAllLabels(userId);
        }

        public async Task<Label> GetLabel(int userId, int labelId)
        {
            return await _label.GetLabel(userId, labelId);
        }
    }
}
