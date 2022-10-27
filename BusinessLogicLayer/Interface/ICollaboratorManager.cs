using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface ICollaboratorManager
    {
        Task<string> AddCollaborator(CollaboratorModel collaborate);
        Task<string> DeleteCollaborator(int noteId, string collabEmail);
        IEnumerable<CollaboratorModel> GetAllCollaborators(int noteId);
    }
}
