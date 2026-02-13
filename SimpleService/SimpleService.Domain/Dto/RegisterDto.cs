using SimpleService.SharedKernel.Enums;

namespace SimpleService.Domain.Dto;

public class RegisterDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Username { get; init; }
    public required DateOnly CreatedDate { get; init; }
    public Gender Gender { get; set; }
}