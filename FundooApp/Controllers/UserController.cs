using BusinessLogicLayer.Interface;
using CommonLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace FundooApp.Controllers
{
    //[ApiController]
    //[Route("api/[Controller]")]

    public class UserController : Controller
    {
        private readonly IUserManager manager;
        private readonly ILogger<UserController> logger;
        public UserController(IUserManager manager, ILogger<UserController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel userData) ////frombody attribute says value read from body of the request
        {
            try
            {
                this.logger.LogInformation("New user added successfully with userid " + userData.UserId + " & firstname:" + userData.FirstName);
                HttpContext.Session.SetString("User Name ", userData.FirstName + " " + userData.LastName);
                HttpContext.Session.SetString("User Email ", userData.Email);
                string result = await this.manager.Register(userData);
                if (result.Equals("Registration Succesful"))
                {
                    var userName = HttpContext.Session.GetString("User Name");
                    this.logger.LogInformation("User Name" + userData + result);
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = result });
                }
                else
                {
                    this.logger.LogInformation(userData.FirstName + " " + userData.LastName + " " + result);
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody] LoginModel userData)
        {
            try
            {
                var result = this.manager.Login(userData);
                if (result != null)
                {
                    //ConnectionMultiplexer cMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                    //IDatabase database = cMultiplexer.GetDatabase();
                    //string firstName = database.StringGet("First Name");
                    //string lastnNme = database.StringGet("Last Name");
                    //int userId = Convert.ToInt32(database.StringGet("User Id"));
                    //RegisterModel reg = new RegisterModel
                    //{
                    //    FirstName = firstName,
                    //    LastName = lastnNme,
                    //    Email = userData.Email,
                    //    UserId = userId
                    //};

                    string token = this.manager.JWTTokenGeneration(userData.Email); //, result.userId
                    return this.Ok(new { Status = true, Message = "Login Successful", Data = result, Token = token});
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string> { Status = false, Message = "Login Failed" });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut]
        [Route("api/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPsModel reset)
        {
            try
            {
                string result = await this.manager.ResetPassword(reset);
                if (result.Equals("Password Updated Successfully"))
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/forgot")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                var result = this.manager.ForgetPassword(email);
                if (result.Equals(true))
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Forget password" });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Bad Reuest forget password" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}