using BusinessLogicLayer.Interface;
using CommonLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class CollaboratorManager : ICollaboratorManager
    {
        private readonly ICollaboratorRepository repository;
        public IConfiguration Configuration { get; }


        public CollaboratorManager(IConfiguration configuration, ICollaboratorRepository repository)
        {
            this.repository = repository;
            this.Configuration = configuration;
        }

        public async Task<string> AddCollaborator(CollaboratorModel collaborate)
        {
            try
            {
                return await this.repository.AddCollaborator(collaborate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> DeleteCollaborator(int noteId, string collabEmail)
        {
            try
            {
                return await this.repository.DeleteCollaborator(noteId, collabEmail);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IEnumerable<CollaboratorModel> GetAllCollaborators(int noteId)
        {
            try
            {
                return this.repository.GetAllCollaborators(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
