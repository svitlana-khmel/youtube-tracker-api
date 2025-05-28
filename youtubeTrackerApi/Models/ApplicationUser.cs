
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace EyeTrackingApi.Models;
public class ApplicationUser : IdentityUser
{
    [Key]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? CreatedAt { get; set; }
}