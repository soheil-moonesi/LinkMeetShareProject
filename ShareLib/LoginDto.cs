  using System.ComponentModel.DataAnnotations;

  namespace ShareLib;

public class LoginDto 
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
    [Required]
    [StringLength(15, ErrorMessage =
        "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
    public string password { get; set; }
}