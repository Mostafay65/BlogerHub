using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using BlogerHub.Models;
using BlogerHub.Models.DTOs;
using BlogerHub.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUserRepository userRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDto)
        {
            LoginResponseDTO loginResponseDto = await _userRepository.Login(loginRequestDto);
            if (loginResponseDto.Success)
            {
                return Ok(new APIResponse()
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = loginResponseDto.Token
                });
            }
            else
            {
                return BadRequest(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string>() { loginResponseDto.ErrorMessage }
                });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var Errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        Errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = Errors
                });
            }
            RegisterResponseDTO registerResponseDto =  await _userRepository.Register(registerRequestDto);
            if (registerResponseDto.Success)
            {
                return Ok(new APIResponse()
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = registerResponseDto.User
                });
            }
            else
            {
                return BadRequest(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string>() { registerResponseDto.ErrorMessage }
                });
            }
        }
    }
}
