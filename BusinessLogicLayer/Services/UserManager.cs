using BusinessLogicLayer.Interface;
using CommonLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repository;
        public IConfiguration Configuration { get; }
        
        public UserManager(IUserRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.Configuration = configuration;
        }

        public async Task<string> Register(RegisterModel data)
        {
            try
            {
                return await this.repository.Register(data);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public LoginModel Login(LoginModel userData)
        {
            try
            {
                return this.repository.Login(userData);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> ResetPassword(ResetPsModel reset)
        {
            try
            {
                return await this.repository.ResetPassword(reset);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool ForgetPassword(string email)
        {
            try
            {
                return this.repository.ForgetPassword(email);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public string JWTTokenGeneration(string email) //, int userId
        {
            byte[] key = Encoding.UTF8.GetBytes(this.Configuration["SecretKey"]); //encrypting secret key
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email),
                    //new Claim("UserId", userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30), //expiry time
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler(); //creating and validating jwt
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token); //write serialize security token to web token
        }
    }
}
