using System.ComponentModel.DataAnnotations;

namespace SimpleService.Contracts.Requests;

public record PutUserRequest(
    [Required] string Email,
    [Required] string Password,
    [Required] string Username
    );