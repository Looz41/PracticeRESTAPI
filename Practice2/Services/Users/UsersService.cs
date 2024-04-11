using Microsoft.EntityFrameworkCore;

public class UsersService: IUsersService
{
    private DataBase _context;
    private TokenGenerator _tokenGenerator;
    public UsersService(DataBase context)
    {
        _tokenGenerator = new TokenGenerator();
        _context = context;
        
    }
    public async Task<string?> Registration(UserRegistrationModel body)
    {
        var checkEmail = await _context.Users.AllAsync(u => u.Email != body.Email);
        if (!checkEmail)
        {
            throw new Exception($"User with email:{body.Email} already exists");
        }

        var token = _tokenGenerator.GenerateToken();
        var user = new User
        {
            Name = body.Name,
            Password = body.Password,
            Email = body.Email,
            Token = token
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<string?> Authorisation(UserAuthorisationModel body)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == body.Email && u.Password == body.Password);
        if (user == null)
        {
            return null;
        }

        var token = _tokenGenerator.GenerateToken();
        user.Token = token;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return token;
    }
}