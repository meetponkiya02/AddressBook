using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class CON_ContactModel
    {
        public int? ContactID { get; set; }

        [Required(ErrorMessage = "Please Select Country DropDown")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Please Select State DropDown")]
        public int StateID { get; set; }

        [Required(ErrorMessage = "Please Select City DropDown")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Please Select ContactCategory DropDown")]
        public int ContactCategoryID { get; set; }

        [Required(ErrorMessage = "Please Enter ContactName")]
        public string? ContactName { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter PinCode")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Please Enter AlternetContact")]
        public string AlternetContact { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Select BirthDate")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Please Enter LinkedIn")]
        public string LinkedIn { get; set; }

        [Required(ErrorMessage = "Please Enter Twitter")]
        public string Twitter { get; set; }

        [Required(ErrorMessage = "Please Enter Insta")]
        public string Insta { get; set; }

        [Required(ErrorMessage = "Please Enter Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Select File")]
        public IFormFile File { get; set; }
        public string PhotoPath { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
