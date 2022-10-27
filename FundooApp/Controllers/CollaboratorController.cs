using BusinessLogicLayer.Interface;
using CommonLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FundooApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CollaborateController : Controller
    {

        private readonly ICollaboratorManager manager;
        public CollaborateController(ICollaboratorManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("addCollaborator")]
        public async Task<IActionResult> AddCollaborator([FromBody] CollaboratorModel collaborate)
        {
            try
            {
                var result = await this.manager.AddCollaborator(collaborate);
                if (result == "Collaborator Added")
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "Cannot Add Collaborator" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpDelete]
        [Route("deleteCollaborator")]
        public async Task<IActionResult> DeleteCollaborator(int noteId, string collabEmail)
        {
            try
            {
                var result = await this.manager.DeleteCollaborator(noteId, collabEmail);
                if (result == "Collaborator Removed")
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "Cannot remove Collaborator" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet]
        [Route("getCollaborator")]
        public IActionResult GetAllCollaborators(int noteId)
        {
            try
            {
                var result = this.manager.GetAllCollaborators(noteId);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "Cannot get Collaborator" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
