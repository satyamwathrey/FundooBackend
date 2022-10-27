using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interface
{
    public interface IUserManager
    {
        Task<string> Register(RegisterModel data);
        LoginModel Login(LoginModel userData);
        Task<string> ResetPassword(ResetPsModel reset);
        bool ForgetPassword(string email);
        string JWTTokenGeneration(string email); //, int userId)
    }
}
