using System.ComponentModel.DataAnnotations;

namespace SimpleService.Contracts.Requests;

public record AuthRequest(
    [Required] string Email,
    [Required] string Password
    );