using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }
        [Required(ErrorMessage ="Please enter country name")]
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }

        public IFormFile File { get; set; }
        public string PhotoPath { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

    }
    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }  
        public string CountryName { get; set; } 
    }
}
