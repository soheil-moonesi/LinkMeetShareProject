  using System.ComponentModel.DataAnnotations;

  namespace ShareLib;

public class LoginDto 
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email address is not valid.")]
    public string email { get; set; }

    //[DataType(DataType.Password)]
    [Required(ErrorMessage = "password is required.")]
    [StringLength(15, ErrorMessage =
        "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
    public string password { get; set; }
}