using System.ComponentModel.DataAnnotations;
using SimpleService.SharedKernel.Enums;

namespace SimpleService.Contracts.Requests;

public record RegisterRequest(
    [Required] string Email,
    [Required] string Password,
    [Required] string Username, 
    [Required] Gender Gender
    ) : AuthRequest(Email, Password);