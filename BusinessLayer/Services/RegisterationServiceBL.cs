using BusinessLayer.Interfaces;
using ModelLayer.RegisterModel;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class RegisterationServiceBL(IRegisterationRL registeration) : IRegisterationBL
    {
        private readonly IRegisterationRL _registeration = registeration;

        public async Task<bool> UserRegister(RegisterUserModel registerModel)
        {
            return await _registeration.RegisterUser(registerModel);
        }
    }
}
