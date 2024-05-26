using ModelLayer.CollaborationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICollaborationBL
    {
        public Task<bool> AddCollaboration(int userId, CreateCollaborationModel model);
        public Task<IEnumerable<CollaborationResponseModel>> GetAllCollaborations(int userId);
        public Task RemoveCollaboration(int collaborationId);
    }
}
