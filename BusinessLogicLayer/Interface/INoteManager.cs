using CommonLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface INoteManager
    {
        Task<NoteModel> CreateNote(NoteModel note);
        List<NoteModel> GetNote(int userId);
        Task<NoteModel> EditNote(NoteModel note);
        Task<NoteModel> ChangeColour(NoteModel note);
        Task<string> NoteArchive(int noteId);
        Task<string> Pinning(int noteId);
        List<NoteModel> GetArchiveNote(int userId);
        List<NoteModel> GetTrashNote(int userId);
        Task<string> DeleteNote(int noteId);
        Task<string> RetriveFromTrash(int noteId);
        Task<string> DeleteFromTrash(int notesId);
        Task<string> AddReminder(int notesId, string remind);
        Task<string> DeleteReminder(int noteId);
        IEnumerable<NoteModel> GetReminder(int noteId);
        Task<string> AddingImage(int noteId); //Add this (, IFormFile form) when cloudinary work 
        Task<string> RemovingImage(int noteId);
    }
}
