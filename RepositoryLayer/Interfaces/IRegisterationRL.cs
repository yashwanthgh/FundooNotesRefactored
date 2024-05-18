using ModelLayer.RegisterModel;


namespace RepositoryLayer.Interfaces;

public interface IRegisterationRL
{
    public Task<bool> RegisterUser(RegisterUserModel registerUserModel);
}
