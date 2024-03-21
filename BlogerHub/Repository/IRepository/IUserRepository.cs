
using BlogerHub.Models.DTOs;

namespace BlogerHub.Repository.IRepository;

public interface IUserRepository
{
    public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto);
    public Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDto);
}