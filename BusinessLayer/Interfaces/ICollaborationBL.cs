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
        public Task AddCollaboration(int userId, CreateCollaborationModel model);
        public Task<IEnumerable<CollaborationResponseModel>> GetAllCollaborations();
        public Task RemoveCollaboration(int collaborationId);
    }
}
