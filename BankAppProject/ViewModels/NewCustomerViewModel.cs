using Services.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.ViewModels
{
    public class NewCustomerViewModel
    {
        [MaxLength(30, ErrorMessage = "First name not valid, to long")]
        [Required(ErrorMessage = "First name required.")]
        public string Givenname { get; set; } = string.Empty;



        [MaxLength(30, ErrorMessage = "Last name not valid, to long")]
        [Required(ErrorMessage = "Last name required.")]
        public string Surname { get; set; } = string.Empty;


        [Range(1, 99, ErrorMessage = "Invalid")]
        public Gender CustomerGender { get; set; }


        [MaxLength(50, ErrorMessage = "Street address not valid, to long.")]
        [Required(ErrorMessage = "Street address required.")]
        public string StreetAddress { get; set; } = string.Empty;


        [MaxLength(8, ErrorMessage = "Zipcode not valid.")]
        [Required(ErrorMessage = "Zipcode required.")]
        public string ZipCode { get; set; } = string.Empty;


        [MaxLength(20, ErrorMessage = "City name is to long.")]
        [Required(ErrorMessage = "City required.")]
        public string City { get; set; } = string.Empty;


        [Range(1, 10, ErrorMessage = "Invalid")]
        public Country CustomerCountry { get; set; }


        [Required(ErrorMessage = "Birthday required.")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }


        [Required(ErrorMessage = "Social security number required.")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Social security number must be between 10 and 12 characters.")]
        [RegularExpression(@"^\d{6}[-+A]?\d{4}$|^\d{8}\d{4}$", ErrorMessage = "Invalid format for social security number.")]
        public string NationalId { get; set; } = string.Empty;


        [MaxLength(20, ErrorMessage = "Phone number not valid, to long.")]
        [Required(ErrorMessage = "Phone number required.")]
        public string Telephonenumber { get; set; } = string.Empty;


        [MaxLength(50, ErrorMessage = "Email address not valid, to long.")]
        [Required(ErrorMessage = "Email address required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Emailaddress { get; set; } = string.Empty;
    }
}
