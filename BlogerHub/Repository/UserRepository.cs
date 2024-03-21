using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using BlogerHub.Models;
using BlogerHub.Models.DTOs;
using BlogerHub.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BlogerHub.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public UserRepository(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    private async Task<bool> is_valid(LoginRequestDTO loginRequestDto)
    {
        var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
        var res = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        return user is not null && res is true;
    }
    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
    {
        if (await is_valid(loginRequestDto) is false)
        {
            return new LoginResponseDTO()
            {
                Success = false,
                ErrorMessage = "Invalid Username or Password",
                Token = null
            };
        }

        var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
        var Token = await CreatToken(user);
        LoginResponseDTO loginResponseDto = new()
        {
            Success = true,
            ErrorMessage = null,
            Token = new TokenDTO { AccessToken = Token }
        };
        return loginResponseDto;
    }

    public async Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDto)
    {
        ApplicationUser user = new ApplicationUser()
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.Email,
            CreatedAt = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
        };
        var res = await _userManager.CreateAsync(user, registerRequestDto.Password);
        if (res.Succeeded)
        {
            if (await _roleManager.FindByNameAsync(registerRequestDto.Role) is null)
            {
                await _roleManager.CreateAsync(new IdentityRole(registerRequestDto.Role));
            }
            await _userManager.AddToRoleAsync(user, registerRequestDto.Role);
            return new RegisterResponseDTO()
            {
                Success = true,
                User = new(){UserName = registerRequestDto.UserName,Email = registerRequestDto.Email}
            };
        }
        else
        {
            return new RegisterResponseDTO()
            {
                Success = false,
                ErrorMessage = res.Errors.FirstOrDefault().Description
            };
        }
    }
    
    public async Task<string> CreatToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var SecretKey = _configuration["APISettings:SecretKey"];
        var Key = Encoding.ASCII.GetBytes(SecretKey);
        var TokenHandler = new JwtSecurityTokenHandler();
        var TokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Role, roles.FirstOrDefault()),
                    new(JwtRegisteredClaimNames.Sub, user.Id)
                }),
            Expires = DateTime.Now.AddMinutes(120),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = TokenHandler.CreateToken(TokenDescriptor);
        return TokenHandler.WriteToken(token);
    }
    
}