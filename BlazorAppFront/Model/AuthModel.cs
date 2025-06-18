using System.ComponentModel.DataAnnotations;
using ShareLib;

namespace BlazorAppFront.Model
{
    //todo: replace to share project if model use in api and front
    //todo: convert each calss to seperate class
    public class LoginResult
    {
        public string message { get; set; }
        public string email { get; set; }
        public string jwtBearer { get; set; }
        public bool success { get; set; }
    }

    public class RegModel : LoginDto 
    {
        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password and confirm password do not match.")]
        public string confirmed { get; set; }
    }




}
