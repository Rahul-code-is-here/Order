using System.ComponentModel.DataAnnotations;
using PizzaShop.Domain.DataModels;

namespace PizzaShop.Domain.ViewModels;

public class UserListModel
{
     public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First Name must start with a letter and contain only letters (no spaces).")]
    [MaxLength(12)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last Name must start with a letter and contain only letters (no spaces).")]
    [MaxLength(12)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(12)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [MaxLength(44)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    // [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be like eg.Abc@123 and length should be 8 to 15")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Role selection is required")]
    public int RoleId { get; set; }

    public int CountryID { get; set; }
    public int StateID { get; set; }
    public int CityID { get; set; }

    public string status{get;set;}

    public bool IsDeleted { get; set; }

    [Required(ErrorMessage = "Address is required")]
    //      [RegularExpression(@"^[a-zA-Z0-9\s,.-]{1,50}$
    // ", ErrorMessage = "Adress must be in 50 words")]
    [MaxLength(35)]
    public string Address { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    public string Phone { get; set; }

//     public string? PathOfProfilePicture { get; set; }
//     public IFormFile? ProfilePicture { get; set; }

        public IFormFile? ProfileImage { get; set; }

    public string? ProfileImagePath { get; set; }
    public string Zipcode { get; set; }

    
         public List<Country> Countries { get; set; } = new List<Country>(); 
        public List<State> States { get; set; } = new List<State>();
        public List<City> Cities { get; set; } = new List<City>();
}

