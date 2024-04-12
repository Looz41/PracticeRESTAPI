using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class UsersService : IUsersService
{
    private readonly DataBase _context;
    private readonly TokenGenerator _tokenGenerator;
    private readonly PasswordHasher _passwordHasher;
    public UsersService(DataBase context)
    {
        _tokenGenerator = new TokenGenerator();
        _context = context;
        _passwordHasher = new PasswordHasher();
    }

    public async Task<string?> Registration(UserRegistrationModel body)
    {
        var checkEmail = await _context.Users.AllAsync(u => u.Email != body.Email);
        if (!checkEmail)
        {
            throw new Exception($"User with email:{body.Email} already exists");
        }

        var token = _tokenGenerator.GenerateToken();
        var hashedPassword = _passwordHasher.HashPassword(body.Password);
        var user = new User
        {
            Name = body.Name,
            Password = hashedPassword,
            Email = body.Email,
            Token = token
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<string?> Authorisation(UserAuthorisationModel body)
    {
        var hashedPassword = _passwordHasher.HashPassword(body.Password);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == body.Email && u.Password == hashedPassword);
        if (user == null)
        {
            throw new Exception("Wrong email or password");
        }

        var token = _tokenGenerator.GenerateToken();
        user.Token = token;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return token;
    }
}
