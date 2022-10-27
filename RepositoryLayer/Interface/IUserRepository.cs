using CommonLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRepository
    {
        Task<string> Register(RegisterModel data);
        LoginModel Login(LoginModel userData);
        Task<string> ResetPassword(ResetPsModel reset);
        bool ForgetPassword(string email);
    }
}
