using ModelLayer.RegisterModel;

namespace BusinessLayer.Interfaces
{
    public interface IRegisterationBL
    {
        public Task<bool> UserRegister(RegisterUserModel registerModel);
    }
}
