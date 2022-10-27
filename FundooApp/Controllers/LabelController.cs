using BusinessLogicLayer.Interface;
using CommonLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundooApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LabelController : Controller
    {
        private readonly ILabelManager manager;
        public LabelController(ILabelManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("addLabelWithUserId")]
        public async Task<IActionResult> AddLabelWithUserId([FromBody] LabelModel labelModel)
        {
            try
            {
                var result = await this.manager.AddLabelWithUserId(labelModel);
                if (result == "Label Added")
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "Cannot Add label from this UserId" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        [Route("addLabelWithNoteId")]
        public async Task<IActionResult> AddLabelWithNoteId(LabelModel labelModel)
        {
            try
            {
                var result = await this.manager.AddLabelWithNoteId(labelModel);
                if (result == "Label Added")
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "Cannot Add label from this NoteId" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpDelete]
        [Route("deleteLabel")]
        public async Task<IActionResult> DeleteLabel(int userId, string labelName)
        {
            try
            {
                var result = await this.manager.DeleteLabel(userId, labelName);
                if (result == "Label Deleted")
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = result });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpDelete]
        [Route("removeLabel")]
        public async Task<IActionResult> RemoveLabel(int labelId)
        {
            try
            {
                var result = await this.manager.RemoveLabel(labelId);
                if (result == "Label Removed")
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = result });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPut]
        [Route("editLabel")]
        public async Task<IActionResult> EditLabel(LabelModel labelModel)
        {
            try
            {
                var result = await this.manager.EditLabel(labelModel);
                if (result == "Label Edited")
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = result });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet]
        [Route("getLabelUserId")]
        public IActionResult GetLabelUserId(int userId)
        {
            try
            {
                var result = this.manager.GetLabelUserId(userId);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "label does not exist" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet]
        [Route("getLabelNoteId")]
        public IActionResult GetLabelNoteId(int noteId)
        {
            try
            {
                var result = this.manager.GetLabelNoteId(noteId);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "label does not exist" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
