using CommonLayer;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext userContext;
        public UserRepository(IConfiguration configuration, UserContext userContext)
        {
            this.Configuration = configuration;
            this.userContext = userContext;
        }
        public IConfiguration Configuration { get; }
        public async Task<string> Register(RegisterModel data)
        {
            try
            {
                var validEmail = this.userContext.Users.Where(x => x.Email == data.Email).FirstOrDefault();
                if (validEmail == null)
                {
                    //Encrypting the password
                    data.Password = EncryptPassword(data.Password);
                    //adding the data in the database
                    this.userContext.Add(data);
                    //save the change in database
                    await this.userContext.SaveChangesAsync();
                    return "Registration Succesful";
                }
                return "Email Id Already Exists";
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public LoginModel Login(LoginModel userData)
        {
            try
            {
                var checkEmail = this.userContext.Users.Where(x => x.Email == userData.Email).FirstOrDefault();
                if (checkEmail != null)
                {
                    var checkPass = this.userContext.Users.Where(x => x.Password == EncryptPassword(userData.Password)).FirstOrDefault(); // && x.Email == userData.Email
                    if (checkPass != null)
                    {

                        //ConnectionMultiplexer cMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        //IDatabase database = cMultiplexer.GetDatabase();
                        //database.StringSet(key: "First Name", checkPass.FirstName);
                        //database.StringSet(key: "Last Name", checkPass.LastName);
                        //database.StringSet(key: "User Id", checkPass.UserId.ToString());
                        
                        return userData;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static string EncryptPassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodeData = Convert.ToBase64String(encData_byte);
                return encodeData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in Base64Encoding" + e.Message);
            }
        }
        public async Task<string> ResetPassword(ResetPsModel reset)
        {
            try
            {
                var checkEmail = this.userContext.Users.Where(x => x.Email == reset.Email).FirstOrDefault(); //checking the email
                if (reset != null)
                {
                    //Encrypting the password
                    checkEmail.Password = EncryptPassword(reset.Password);
                    //update the data in the database
                    this.userContext.Update(checkEmail);
                    //save  the change in database
                    await this.userContext.SaveChangesAsync();
                    return "Password Updated Successfully";
                }
                return "Reset Password is Unsuccessful";
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private bool SendEmail(string email)
        {
            string linkToBeSend = this.ReceiveQueue(email);
            if (this.SendMailUsingSMTP(email, linkToBeSend))
            {
                return true;
            }

            return false;
        }
        private MessageQueue QueueDetail()
        {
            MessageQueue messageQueue; //provide access to a queue in MSMQ
            ////checking this private queue exists or not
            if (MessageQueue.Exists(@".\Private$\ResetPasswordQueue"))
            {
                messageQueue = new MessageQueue(@".\Private$\ResetPasswordQueue");
            }
            else
            {
                messageQueue = MessageQueue.Create(@".\Private$\ResetPasswordQueue");
            }
            return messageQueue;
        }
        private void MSMQSend(string url)
        {
            try
            {
                MessageQueue messageQueue = this.QueueDetail();
                Message message = new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = url;
                messageQueue.Label = "url link";
                messageQueue.Send(message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private string ReceiveQueue(string email)
        {
            ////for reading from MSMQ
            var receiveQueue = new MessageQueue(@".\Private$\ResetPasswordQueue");
            var receiveMsg = receiveQueue.Receive();
            receiveMsg.Formatter = new BinaryMessageFormatter();

            string linkToBeSend = receiveMsg.Body.ToString();
            return linkToBeSend;
        }
        private bool SendMailUsingSMTP(string email, string message)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtp = new SmtpClient(); //allow app to sent email using SMTP 
            mailMessage.From = new MailAddress("akshaysayre8@gmail.com"); //contains mail id from where mail will send 
            mailMessage.To.Add(new MailAddress(email)); //the user mail to which mail will be send
            mailMessage.Subject = "Link to reset you password for fundoo Application";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message;
            smtp.Port = 587; //port no.
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true; //specify smtpserver use ssl or not, default setting is false
            smtp.Credentials = new NetworkCredential("akshaysayre8@gmail.com", "hzeapyzrotswdgbf");
            smtp.Send(mailMessage);
            return true;
        }
        public bool ForgetPassword(string email)
        {
            try
            {
                var validEmail = this.userContext.Users.Where(x => x.Email == email).FirstOrDefault(); //checking the email
                if (validEmail != null)
                {
                    //calling SMTP method to sent mail to the user
                    this.MSMQSend("Link for resetting the password");
                    return this.SendEmail(email);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
