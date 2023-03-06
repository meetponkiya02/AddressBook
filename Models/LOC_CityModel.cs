using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class LOC_CityModel
    {
        public int? CityID{ get; set; }
        [Required(ErrorMessage = "Please Select DropDown")]
        public int StateID { get; set; }

        [Required(ErrorMessage = "Please Select DropDown")]
        public int CountryID{ get; set; }
        
        public int StateName { get; set; }
        [Required(ErrorMessage = "Please enter city name")]
        public string? CityName{ get  ; set; }

        [Required(ErrorMessage = "Please enter PinCode")]
        public string? PinCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
    public class LOC_CityDropDownModel
    {
        public int CityID { get; set; }    

        public string CityName { get; set; }
    }
}
