using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook.Models;

namespace AddressBook.Controllers
{
    public class CON_ContactController : Controller
    {
        private IConfiguration Configuration;
        public CON_ContactController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region Index
        public IActionResult Index()
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open(); 

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_CON_Contact_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            conn.Close();

            return View("CON_ContactList", dt);


        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_CON_Contact_DeleteByPK";
            objCmd.Parameters.AddWithValue("@ContactID", ContactID);    
            objCmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index");


        }

        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(CON_ContactModel modelCON_Contact)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            if(modelCON_Contact.ContactID == null)
            {
                objCmd.CommandText = "PR_CON_Contact_Insert";
            }
            else 
            {
                objCmd.CommandText = "PR_CON_Contact_UpdateByPK";
                objCmd.Parameters.Add("@ContactID",SqlDbType.Int).Value = modelCON_Contact.ContactID;
            }
            objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelCON_Contact.CountryID;
            objCmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelCON_Contact.StateID;
            objCmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelCON_Contact.CityID;
            objCmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelCON_Contact.ContactCategoryID;
            objCmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = modelCON_Contact.ContactName;
            objCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = modelCON_Contact.Address;
            objCmd.Parameters.Add("@PinCode", SqlDbType.Int).Value = modelCON_Contact.PinCode;
            objCmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = modelCON_Contact.Mobile;
            objCmd.Parameters.Add("@AlternetContact", SqlDbType.NVarChar).Value = modelCON_Contact.AlternetContact;
            objCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = modelCON_Contact.Email;
            objCmd.Parameters.Add("@BirthDate", SqlDbType.Date).Value = modelCON_Contact.BirthDate;
            objCmd.Parameters.Add("@LinkedIn", SqlDbType.NVarChar).Value = modelCON_Contact.LinkedIn;
            objCmd.Parameters.Add("@Twitter", SqlDbType.NVarChar).Value = modelCON_Contact.Twitter;
            objCmd.Parameters.Add("@Insta", SqlDbType.NVarChar).Value = modelCON_Contact.Insta;
            objCmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = modelCON_Contact.Gender;
            objCmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = DBNull.Value;
            objCmd.Parameters.Add("@modificationDate", SqlDbType.Date).Value = DBNull.Value;
            if(Convert.ToBoolean(objCmd.ExecuteNonQuery()))
            {
                if(modelCON_Contact.ContactID == null)
                {
                    TempData["ContactIinsertMsg"] = "Record Insert Sucessfully....!!!";
                }
                else 
                {
                    return RedirectToAction("Index"); ;
                }
            }
            conn.Close();
        
            return RedirectToAction("Add");


        }
        #endregion
   
        #region Add
        public IActionResult Add(int? ContactID)
        {

            #region Country Dropdown
            string connectionstr1 = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt1 = new DataTable();
            SqlConnection conn1 = new SqlConnection(connectionstr1);

            conn1.Open();

            SqlCommand objCmd1 = conn1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_LOC_Country_SelectForDropDown";
            SqlDataReader objSDR1 = objCmd1.ExecuteReader();
            dt1.Load(objSDR1);
            conn1.Close();
            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;
           
            #endregion
            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            ViewBag.Statelist= list2; 
            
            #region ContactCategory DropDown
            string connectionstr4 = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt4 = new DataTable();
            SqlConnection conn4 = new SqlConnection(connectionstr4);
            conn4.Open();
            SqlCommand objCmd4 = conn4.CreateCommand();
            objCmd4.CommandType = CommandType.StoredProcedure;                                            
            objCmd4.CommandText = "PR_MST_ContactCategory_SelectForDropDown";
            SqlDataReader objSDR4 = objCmd4.ExecuteReader();
            dt4.Load(objSDR4);
            List<MST_ContactCategoryDropDownModel> list4 = new List<MST_ContactCategoryDropDownModel>();
            foreach (DataRow dr in dt4.Rows)
            {
                MST_ContactCategoryDropDownModel vlst4 = new MST_ContactCategoryDropDownModel();
                vlst4.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                vlst4.ContactCategoryName = dr["ContactCategoryName"].ToString();
                list4.Add(vlst4);
            }
            ViewBag.ContactCategoryList = list4;
            conn4.Close();

            #endregion
            List<LOC_StateDropDownModel> list3= new List<LOC_StateDropDownModel>();
            ViewBag.CityList = list3;   


            if (ContactID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(connectionstr);

                conn.Open();

                SqlCommand objCmd = conn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_CON_Contact_SelectByPK";         
                objCmd.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);
                CON_ContactModel modelCON_Contact = new CON_ContactModel();
                foreach (DataRow dr in dt.Rows)
                {
                    modelCON_Contact.ContactID = Convert.ToInt32(dr["ContactID"]);
                    modelCON_Contact.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelCON_Contact.StateID = Convert.ToInt32(dr["StateID"]);
                    modelCON_Contact.CityID = Convert.ToInt32(dr["CityID"]);
                    modelCON_Contact.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelCON_Contact.ContactName = dr["ContactName"].ToString();
                    modelCON_Contact.Address = dr["Address"].ToString();
                    modelCON_Contact.PinCode = dr["PinCode"].ToString();
                    modelCON_Contact.Mobile = dr["Mobile"].ToString();
                    modelCON_Contact.AlternetContact = dr["AlternetContact"].ToString();
                    modelCON_Contact.Email = dr["Email"].ToString();
                    modelCON_Contact.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    modelCON_Contact.LinkedIn = dr["LinkedIn"].ToString();
                    modelCON_Contact.Twitter = dr["Twitter"].ToString();
                    modelCON_Contact.Insta = dr["Insta"].ToString();
                    modelCON_Contact.Gender = dr["Gender"].ToString();
                    modelCON_Contact.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelCON_Contact.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);
                }

                conn.Close();
                return View("CON_ContactAddEdit", modelCON_Contact);
            }

               return View("CON_ContactAddEdit");
        }
        #endregion

        #region DropDownByCountry
        public IActionResult DropDownByCountry(int CountryID)
        {
            #region State DropDown
            string connectionstr2 = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt2 = new DataTable();
            SqlConnection conn2 = new SqlConnection(connectionstr2);
            conn2.Open();
            SqlCommand objCmd2 = conn2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_LOC_State_SelectForDropDown";
            objCmd2.Parameters.AddWithValue("@CountryID",CountryID);
            SqlDataReader objSDR2 = objCmd2.ExecuteReader();
            dt2.Load(objSDR2);
            conn2.Close();
            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dt2.Rows)
            {
                LOC_StateDropDownModel vlst = new LOC_StateDropDownModel();
                vlst.StateID = Convert.ToInt32(dr["StateID"]);
                vlst.StateName = dr["StateName"].ToString();
                list2.Add(vlst);
            }
            var vModel = list2;
            return Json(vModel);    
           
            #endregion
        }
        #endregion

        #region DropDownByState
        public IActionResult DropDownByState(int StateID)
        {
            #region City DropDown
            string connectionstr3 = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt3 = new DataTable();
            SqlConnection conn3 = new SqlConnection(connectionstr3);
            conn3.Open();
            SqlCommand objCmd3 = conn3.CreateCommand();
            objCmd3.CommandType = CommandType.StoredProcedure;
            objCmd3.CommandText = "PR_LOC_City_SelectForDropDown";
            objCmd3.Parameters.AddWithValue("@StateID",StateID);
            SqlDataReader objSDR3 = objCmd3.ExecuteReader();
            dt3.Load(objSDR3);
            List<LOC_CityDropDownModel> list3 = new List<LOC_CityDropDownModel>();
            foreach (DataRow dr in dt3.Rows)
            {
                LOC_CityDropDownModel vlst3 = new LOC_CityDropDownModel();
                vlst3.CityID = Convert.ToInt32(dr["CityID"]);
                vlst3.CityName = dr["CityName"].ToString();
                list3.Add(vlst3);
            }
            var vModel=list3;   
            return Json(vModel);    
            conn3.Close();

            #endregion
        }
        #endregion
        public IActionResult CON_ContactList()
        {
            return View();
        }
    }
}
