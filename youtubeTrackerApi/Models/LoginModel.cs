using System.ComponentModel.DataAnnotations;

namespace EyeTrackingApi.Models;
public class LoginModel
{
    [Key]
    public required string Email { get; set; }
    public required string Password { get; set; }
}