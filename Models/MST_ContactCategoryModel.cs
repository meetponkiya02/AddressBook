﻿namespace AddressBook.Models
{
    public class MST_ContactCategoryModel
    {
        public int? ContactCategoryID { get; set; }
        public string ContactCategoryName { get; set; } 
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
    public class MST_ContactCategoryDropDownModel
    {
        public int ContactCategoryID { get; set; }
        public string ContactCategoryName { get; set; }
    }
}
