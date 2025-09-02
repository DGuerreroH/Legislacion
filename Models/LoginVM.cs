using System.ComponentModel.DataAnnotations;
namespace LegislacionAPP.Models;

public class LoginVM
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
}
