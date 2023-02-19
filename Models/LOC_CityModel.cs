namespace AddressBook.Models
{
    public class LOC_CityModel
    {
        public int? CityID{ get; set; }
        public int StateID { get; set; }
        public int CountryID{ get; set; }   
        public int StateName { get; set; }
        public string CityName{ get; set; }
        public string PinCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
    public class LOC_CityDropDownModel
    {
        public int CityID { get; set; }    

        public string CityName { get; set; }
    }
}
