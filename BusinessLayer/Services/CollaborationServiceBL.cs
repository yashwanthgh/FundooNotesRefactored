using BusinessLayer.Interfaces;
using ModelLayer.CollaborationModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CollaborationServiceBL : ICollaborationBL
    {
        private readonly ICollaborationRL _collaboration;

        public CollaborationServiceBL(ICollaborationRL collaboration)
        {
            _collaboration = collaboration;
        }

        public Task AddCollaboration(int userId, CreateCollaborationModel model)
        {
            return _collaboration.AddCollaboration(userId, model);
        }

        public async Task<IEnumerable<CollaborationResponseModel>> GetAllCollaborations()
        {
            return await _collaboration.GetAllCollaborations();
        }

        public Task RemoveCollaboration(int collaborationId)
        {
            return _collaboration.RemoveCollaboration(collaborationId);

        }
    }
}
