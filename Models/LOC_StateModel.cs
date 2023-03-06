using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace AddressBook.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }
        [Required(ErrorMessage = "Please Select DropDown")]
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        [Required(ErrorMessage = "Please enter State name")]
        public string? StateName { get; set; }
        [Required(ErrorMessage = "Please enter State Code")]
        public string?  StateCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}
