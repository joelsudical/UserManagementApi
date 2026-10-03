using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.Models;

public sealed class UserRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; init; } = string.Empty;

    [Range(13, 120)]
    public int Age { get; init; }
}