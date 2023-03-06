using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook.Models;
using AddressBookMVC.DAL;

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

            CON_DAL dalCON = new CON_DAL();
            DataTable dt = dalCON.dbo_PR_CON_Contact_SelectAll(connectionstr);

            return View("CON_ContactList", dt);

           
        }
        #endregion

        #region Delete
        public IActionResult Delete(int ContactID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");

            CON_DAL dalCON = new CON_DAL();

            if (Convert.ToBoolean(dalCON.dbo_PR_CON_Contact_DeleteByPK(connectionstr, ContactID)))
            {
                return RedirectToAction("Index");
            }
            return View("Index");
            //DataTable dt = new DataTable();
            //SqlConnection conn = new SqlConnection(connectionstr);

            //conn.Open();

            //SqlCommand objCmd = conn.CreateCommand();
            //objCmd.CommandType = CommandType.StoredProcedure;
            //objCmd.CommandText = "PR_CON_Contact_DeleteByPK";
            //objCmd.Parameters.AddWithValue("@ContactID", ContactID);    
            //objCmd.ExecuteNonQuery();
            //conn.Close();

            //return RedirectToAction("Index");


        }

        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(CON_ContactModel modelCON_Contact)
        {
            #region PhotoPath
            if (modelCON_Contact.File != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, modelCON_Contact.File.FileName);
                modelCON_Contact.PhotoPath = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + modelCON_Contact.File.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    modelCON_Contact.File.CopyTo(stream);
                }

            }
            #endregion

            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            CON_DAL dalCON = new CON_DAL();


            if (modelCON_Contact.ContactID == null)
            {

                if (Convert.ToBoolean(dalCON.dbo_PR_CON_Contact_Insert(connectionstr, modelCON_Contact)))
                {
                    TempData["ContactInsertMessage"] = "Record inserted successfully";

                }
            }
            else
            {
                if (Convert.ToBoolean(dalCON.dbo_PR_CON_Contact_UpdateByPK(connectionstr, modelCON_Contact)))
                {

                    TempData["ContactUpdateMessage"] = "Record Update Successfully";

                }
                return RedirectToAction("Index");
            }

          

            return RedirectToAction("Add");
        }
        #endregion

        #region Add
        public IActionResult Add(int ContactID)
        {

            #region Country Drop Down

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


            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list1;
            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            ViewBag.CityList = list2;

            #endregion

            #region Contact Category Drop Down
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

            #region Select By PK
            if (ContactID != null)
            {

                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                CON_DAL dalCON = new CON_DAL();

                DataTable dt = dalCON.dbo_PR_CON_Contact_SelectByPK(connectionstr, ContactID);
                if (dt.Rows.Count > 0)
                {
                    CON_ContactModel model = new CON_ContactModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        DropDownByCountry(Convert.ToInt32(dr["CountryID"]));
                        DropDownByState(Convert.ToInt32(dr["StateID"]));
                        model.ContactID = Convert.ToInt32(dr["ContactID"]);
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.StateID = Convert.ToInt32(dr["StateID"]);
                        model.CityID = Convert.ToInt32(dr["CityID"]);
                        model.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                        model.ContactName = dr["ContactName"].ToString();
                        model.Address = dr["Address"].ToString();
                        model.PinCode = dr["PinCode"].ToString();
                        model.Mobile = dr["Mobile"].ToString();
                        model.AlternetContact = dr["AlternetContact"].ToString();
                        model.Email = dr["Email"].ToString();
                        model.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                        model.LinkedIn = dr["LinkedIn"].ToString();
                        model.Twitter = dr["Twitter"].ToString();
                        model.Insta = dr["Insta"].ToString();
                        model.Gender = dr["Gender"].ToString();

                        model.PhotoPath = dr["PhotoPath"].ToString();

                        model.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        model.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

                    }
                    return View("CON_ContactAddEdit", model);


                }


       
            }
            #endregion

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
            conn3.Close();
            var vModel=list3;   
            return Json(vModel);    
            

            #endregion
        }
        #endregion


        #region Filter Records
        public IActionResult Filter(string? ContactName = null)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_CON_Contact_Filter";

            if (ContactName == null)
            {
                ContactName = "";
            }
            

            cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar).Value = ContactName;
            

            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            return View("CON_ContactList", dt);
        }
        #endregion
        
    }
}
