using System.ComponentModel.DataAnnotations;
using PizzaShop.Domain.DataModels;

namespace PizzaShop.Domain.ViewModels;

public class UserModel
{
    [Required(ErrorMessage = "First Name is required")]

    // [RegularExpression(@"^\S+$", ErrorMessage = "No whiteSpaces are allowed")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First Name must start with a letter and contain only letters (no spaces).")]
    [MaxLength(12)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]

    // [RegularExpression(@"^\S+$", ErrorMessage = "No whiteSpaces are allowed")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last Name must start with a letter and contain only letters (no spaces).")]

    [MaxLength(12)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [RegularExpression(@"^\S+$", ErrorMessage = "No whiteSpaces are allowed")]
    [MaxLength(12)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [MaxLength(44)]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    
    // [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Password Must contain Special Symbol, Number,Alphabet")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be like eg.Abc@123 and length should be 8 to 15")]
    public string Password { get; set; }
    public int CountryID { get; set; }
    public int StateID { get; set; }
    public int CityID { get; set; }

    [Required(ErrorMessage = "You must provide a phone number")]
    [Display(Name = "Home Phone")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

    public string PhoneNumber { get; set; }


    public int RoleId { get; set; }

    [Required(ErrorMessage = "Address is required")]
    //     [RegularExpression(@"^[a-zA-Z0-9\s,.\-]{1,50}$
    // ", ErrorMessage = "Adress must be in 50 words")]
    [MaxLength(35)]
    public string Address { get; set; }

    [Required(ErrorMessage = "Pincode is required")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be exactly 6 digits")]
    public string Zipcode { get; set; }

    public IFormFile? ProfileImage { get; set; }

    public string? ProfileImagePath { get; set; }
    public List<Country> Countries { get; set; }
    public List<State> States { get; set; }
    public List<City> Cities { get; set; }
}