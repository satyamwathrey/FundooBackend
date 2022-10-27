using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteRepository : INoteRepository
    {
        private readonly UserContext userContext;

        public NoteRepository(IConfiguration configuration, UserContext userContext)
        {
            this.Configuration = configuration;
            this.userContext = userContext;
        }
        public IConfiguration Configuration { get; }
        public async Task<NoteModel> CreateNote(NoteModel note)
        {
            try
            {
                if (note.Title != null || note.Describe != null)
                {
                    this.userContext.Notes.Add(note);
                    await this.userContext.SaveChangesAsync();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (ArgumentException e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<NoteModel> GetNote(int userId)
        {
            try
            {
                //var myNotes = this.userContext.Notes.Where(x => x.UserId == userId && x.Trash == false && x.Archieve == false).ToList();
                var myNotes = (from note in this.userContext.Notes
                               where note.UserId == userId && note.Archieve == false
                               && note.Trash == false
                               select note).ToList();

                if (myNotes.Count != 0)
                {
                    return myNotes;
                }
                else
                    return null;
            }
            catch (ArgumentException e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<NoteModel> EditNote(NoteModel note)
        {
            try
            {
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == note.NoteId).FirstOrDefault();
                if (presentNoteId != null)
                {
                    if (note != null)
                    {
                        presentNoteId.Title = note.Title;
                        presentNoteId.Describe = note.Describe;
                        this.userContext.Notes.Update(presentNoteId);
                        await this.userContext.SaveChangesAsync();
                        return note;
                    }
                    return null;
                }
                else
                {
                    return null;
                }
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
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == note.NoteId).FirstOrDefault();
                if (presentNoteId != null)
                {
                    if (note != null)
                    {
                        presentNoteId.Colour = note.Colour;
                        this.userContext.Notes.Update(presentNoteId);
                        await this.userContext.SaveChangesAsync();
                        return note;
                    }
                    return null;
                }
                else
                {
                    return null;
                }
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
                string res;
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == noteId).FirstOrDefault();
                if (presentNoteId != null)
                {
                    if (presentNoteId.Archieve == false)
                    {
                        presentNoteId.Archieve = true;
                        if (presentNoteId.Pinned == true)
                        {
                            presentNoteId.Pinned = false;
                            res = "Archived";
                        }
                        else
                            res = "Archived";
                    }
                    else
                    {
                        presentNoteId.Archieve = false;
                        res = "Unarchived";
                    }
                    this.userContext.Notes.Update(presentNoteId);
                    await this.userContext.SaveChangesAsync();
                }
                else
                {
                    res = "cannot Archive";
                }
                return res;
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
                string res;
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == noteId).FirstOrDefault();
                if (presentNoteId != null)
                {
                    if (presentNoteId.Pinned == false)
                    {
                        presentNoteId.Pinned = true;
                        if (presentNoteId.Archieve == true)
                        {
                            presentNoteId.Archieve = false;
                            res = "Note unarchived and pinned";
                        }
                        else
                        {
                            res = "Note pinned";
                        }
                    }
                    else
                    {
                        presentNoteId.Pinned = false;
                        res = "Note unpinned";
                    }
                    this.userContext.Notes.Update(presentNoteId);
                    await this.userContext.SaveChangesAsync();
                }
                else
                {
                    res = "Note does not exist";
                }
                return res;
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
                var myNotes = this.userContext.Notes.Where(a => a.UserId == userId &&
                        a.Trash == false && a.Archieve == true).ToList();

                if (myNotes != null)
                {
                    return myNotes;
                }
                else
                    return null;
            }
            catch (ArgumentException e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<NoteModel> GetTrashNote(int userId) //dispay trash notes
        {
            try
            {
                var myNotes = this.userContext.Notes.Where(a => a.UserId == userId &&
                        a.Trash == true).ToList();

                if (myNotes != null)
                {
                    return myNotes;
                }
                else
                    return null;
            }
            catch (ArgumentException e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> DeleteNote(int noteId) //put in trash
        {
            try
            {
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == noteId).SingleOrDefault();
                if (presentNoteId != null)
                {
                    presentNoteId.Trash = true;
                    if (presentNoteId.Pinned == true)
                    {
                        presentNoteId.Pinned = false;
                        this.userContext.Notes.Update(presentNoteId);
                        await this.userContext.SaveChangesAsync();
                        return "Note unpinned and Trashed";
                    }
                    else
                    {
                        this.userContext.Notes.Update(presentNoteId);
                        await this.userContext.SaveChangesAsync();
                        return "Note Trashed";
                    }
                }
                else
                {
                    return "Note does not exist";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<string> RetriveFromTrash(int notesId)
        {
            try
            {
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == notesId).FirstOrDefault();
                if (presentNoteId != null)
                {
                    presentNoteId.Trash = false; //Note Received from trash
                    this.userContext.Notes.Update(presentNoteId);
                    await this.userContext.SaveChangesAsync();
                    return "Note Restored";
                }
                else
                {
                    return "This note does not exist";
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> DeleteFromTrash(int notesId) //permanent delete
        {
            try
            {
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == notesId).FirstOrDefault();
                if (presentNoteId != null)
                {
                    this.userContext.Notes.Remove(presentNoteId);
                    await this.userContext.SaveChangesAsync();
                    return "Note Deleted Permanently";
                }
                else
                {
                    return "This note does not exist";
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> AddReminder(int notesId, string remind)
        {
            try
            {
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == notesId).SingleOrDefault();
                if (presentNoteId != null)
                {
                    presentNoteId.Reminder = remind;
                    this.userContext.Notes.Update(presentNoteId);
                    await this.userContext.SaveChangesAsync();
                    return "Remind me";
                }
                else
                {
                    return "This note does not exist";
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> DeleteReminder(int noteId)
        {
            try
            {
                var presentNoteId = this.userContext.Notes.Where(a => a.NoteId == noteId).SingleOrDefault();
                if (presentNoteId != null)
                {
                    presentNoteId.Reminder = null;
                    this.userContext.Notes.Update(presentNoteId);
                    await this.userContext.SaveChangesAsync();
                    return "Reminder Deleted";
                }
                return "This note does not exist";
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<NoteModel> GetReminder(int noteId)
        {
            try
            {
                var myNotes = this.userContext.Notes.Where(a => a.NoteId == noteId &&
                        a.Reminder != null).ToList();

                if (myNotes.Count != 0)
                {
                    return myNotes;
                }
                else
                    return null;
            }
            catch (ArgumentException e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<string> AddingImage(int noteId) //Add this (, IFormFile form) when cloudinary work 
        {
            try
            {
                var isNoteId = this.userContext.Notes.Where(x => x.NoteId == noteId).SingleOrDefault();
                if (isNoteId != null)
                {
                    //Account account = new Account(Configuration["Cloudinary:CloudName"], Configuration["Cloudinary:ApiKey"], Configuration["Cloudinary:ApiSecret"]);
                    //Cloudinary cloudObj = new Cloudinary(account);
                    //var cloudObj = new Cloudinary(new Account("dhb6pi07n", "942895279784874", "a6TTCpMkCtrUPyafjc7gAhCw4eE"));

                    //var uploadImage = new ImageUploadParams()
                    //{
                    //    File = new FileDescription(form.FileName, form.OpenReadStream()),
                    //};
                    //var uploadRes = cloudObj.Upload(uploadImage);
                    //var uploadPath = uploadRes.Url;

                    string uploadPath = "https://res.cloudinary.com/dhb6pi07n/image/upload/v1655223023/tiger_fbvsq0.jpg";

                    isNoteId.Image = uploadPath; //.ToString()
                    this.userContext.Notes.Update(isNoteId);
                    await this.userContext.SaveChangesAsync();
                    return "Image added Successfully";
                }
                else
                {
                    return "Note does not exist";
                }
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
                var presentNote = this.userContext.Notes.Where(a => a.NoteId == noteId).SingleOrDefault();
                if (presentNote != null)
                {
                    presentNote.Image = null;
                    this.userContext.Notes.Update(presentNote);
                    await this.userContext.SaveChangesAsync();
                    return "Removed Image Successfully";
                }
                return "This note does not exist";
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
