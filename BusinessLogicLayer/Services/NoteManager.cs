using BusinessLogicLayer.Interface;
using CommonLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class NoteManager : INoteManager
    {
        private readonly INoteRepository repository;
        public IConfiguration Configuration { get; }
        public NoteManager(IConfiguration configuration, INoteRepository repository)
        {
            this.repository = repository;
            this.Configuration = configuration;
        }
        public async Task<NoteModel> CreateNote(NoteModel note)
        {
            try
            {
                return await this.repository.CreateNote(note);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<NoteModel> GetNote(int userId)
        {
            try
            {
                return this.repository.GetNote(userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<NoteModel> EditNote(NoteModel note)
        {
            try
            {
                return await this.repository.EditNote(note);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<NoteModel> ChangeColour(NoteModel note)
        {
            try
            {
                return await this.repository.ChangeColour(note);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> NoteArchive(int noteId)
        {
            try
            {
                return await this.repository.NoteArchive(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> Pinning(int noteId)
        {
            try
            {
                return await this.repository.Pinning(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<NoteModel> GetArchiveNote(int userId)
        {
            try
            {
                return this.repository.GetArchiveNote(userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<NoteModel> GetTrashNote(int userId)
        {
            try
            {
                return this.repository.GetTrashNote(userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> DeleteNote(int noteId)
        {
            try
            {
                return await this.repository.DeleteNote(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> RetriveFromTrash(int noteId)
        {
            try
            {
                return await this.repository.RetriveFromTrash(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> DeleteFromTrash(int notesId)
        {
            try
            {
                return await this.repository.DeleteFromTrash(notesId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> AddReminder(int notesId, string remind)
        {
            try
            {
                return await this.repository.AddReminder(notesId,remind);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> DeleteReminder(int noteId)
        {
            try
            {
                return await this.repository.DeleteReminder(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<NoteModel> GetReminder(int noteId)
        {
            try
            {
                return this.repository.GetReminder(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> AddingImage(int noteId)//Add this (, IFormFile form) when cloudinary work 
        {
            try
            {
                return await this.repository.AddingImage(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> RemovingImage(int noteId)
        {
            try
            {
                return await this.repository.RemovingImage(noteId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
