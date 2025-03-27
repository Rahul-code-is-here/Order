using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;

public class ResetPasswordModel
{
    public string Token { get; set; }

     [DataType(DataType.Password)]
   [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be like eg.Abc@123 and length should be 8 to 15")]
    public string NewPassword { get; set; }

     [DataType(DataType.Password)]
   [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be like eg.Abc@123 and length should be 8 to 15")]
    public string ConfirmPassword { get; set; }
}
