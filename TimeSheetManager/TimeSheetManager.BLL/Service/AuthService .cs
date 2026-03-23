
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TimeSheetManager.BLL.DTO;
using TimeSheetManager.BLL.IService;
using TimeSheetManager.DAL.Repository;
using TimeSheetManager.Models;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public LoginResponse Login(LoginRequest request)
    {
        var user = _repo.GetByUsername(request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = GenerateJwt(user);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.RoleId.ToString()
        };
    }

    public void Register(RegisterRequest request)
    {
        // Kiểm tra username đã tồn tại
        if (_repo.ExistsByUsername(request.Username))
            throw new Exception("Username already exists");

        // Tạo User với RoleId = 4 (Employee)
        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            RoleId = 4,
            Employee = new Employee
            {
                FullName = request.FullName,
                Email = request.Email,
                Department = request.Department,
                Position = request.Position,
                Status = "Active"
            }
        };

        _repo.CreateUser(user);
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}