using Services.Customer;
using System.ComponentModel.DataAnnotations;

namespace BankAppProject.ViewModels
{
    public class EditCustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [MaxLength(30, ErrorMessage = "First name not valid, to long")]
        [Required(ErrorMessage = "First name required.")]
        public string Givenname { get; set; } = string.Empty;

        [MaxLength(30, ErrorMessage = "Last name not valid, to long")]
        [Required(ErrorMessage = "Last name required.")]
        public string Surname { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Street address not valid, to long.")]
        [Required(ErrorMessage = "Street address required.")]
        public string Streetaddress { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "City name is to long.")]
        [Required(ErrorMessage = "City required.")]
        public string City { get; set; } = string.Empty;

        [MaxLength(8, ErrorMessage = "Zipcode not valid.")]
        [Required(ErrorMessage = "Zipcode required.")]
        public string Zipcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; } = string.Empty;
        public string? CountryCode { get; set; }


        [Required(ErrorMessage = "Birthday required.")]
        [DataType(DataType.Date)]
        public DateOnly? Birthday { get; set; }

        [MaxLength(12, ErrorMessage = "Social security number not valid.")]
        [Required(ErrorMessage = "Social security number required.")]
        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        [MaxLength(20, ErrorMessage = "Phone number not valid, to long.")]
        [Required(ErrorMessage = "Phone number required.")]
        public string? Telephonenumber { get; set; }

        [MaxLength(50, ErrorMessage = "Email address not valid, to long.")]
        [Required(ErrorMessage = "Email address required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string? Emailaddress { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
