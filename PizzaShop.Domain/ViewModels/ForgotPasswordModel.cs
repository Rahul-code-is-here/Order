using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "The email address is not valid.")]
        public string Email { get; set; }
    }