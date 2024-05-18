using Dapper;
using ModelLayer.RegisterModel;
using RepositoryLayer.Contexts;
using RepositoryLayer.GlobalExceptions;
using RepositoryLayer.Interfaces;
using System.Data;
using System.Text.RegularExpressions;


namespace RepositoryLayer.Services;

public class RegisterationServiceRL(DapperContext context) : IRegisterationRL
{
    private readonly DapperContext _context = context;

    public async Task<bool> RegisterUser(RegisterUserModel userModel)
    {
        if (!IsEmailValid(userModel.Email)) throw new InvalidEmailFormateException("Invalid email formate");
        if (!IsPasswordValid(userModel.Password)) throw new InvalidPasswordFormateException("Invalid password formate");

        var gettingEmailFromRegisteringUser = new DynamicParameters();
        gettingEmailFromRegisteringUser.Add("Email", userModel.Email, DbType.String);

        var parameters = new DynamicParameters();

        parameters.Add("FirstName", userModel.FirstName, DbType.String);
        parameters.Add("LastName", userModel.LastName, DbType.String);
        parameters.Add("Email", userModel.Email, DbType.String);

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
        parameters.Add("Password", hashPassword, DbType.String);

        using (var connection = _context.CreateConnection())
        {
            var emailExists = connection.QueryFirstOrDefault<bool>("spToCheckIfEmailIsDuplicate", gettingEmailFromRegisteringUser, commandType: CommandType.StoredProcedure);

            if (emailExists)
            {
                throw new DuplicateEmailException("Email alredy exists!");
            }

            await connection.ExecuteAsync("spRegisterUser", parameters, commandType: CommandType.StoredProcedure);
            return true;
        }
    }

    private static bool IsEmailValid(string? email)
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (email == null) return false;
        return Regex.IsMatch(email, pattern);
    }

    private static bool IsPasswordValid(string? password)
    {
        var pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*])(?=.*[0-9]).{8,15}$";
        if (password == null) return false;
        return Regex.IsMatch(password, pattern);
    }
}
