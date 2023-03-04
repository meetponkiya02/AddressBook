using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook.Models;
using System.Reflection.Metadata.Ecma335;

namespace AddressBook.Controllers
{
    public class MST_ContactCategoryController : Controller
    {
        private IConfiguration Configuration;
        public MST_ContactCategoryController(IConfiguration _configuration)
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
            objCmd.CommandText = "PR_MST_ContactCategory_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            conn.Close();

            return View("MST_ContactCategoryList", dt);


        }
        #endregion

        #region Dalete
        public IActionResult Delete(int ContactCategoryID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_MST_ContactCategory_DeleteByPK";
            objCmd.Parameters.AddWithValue("@ContactCategoryID", ContactCategoryID);
            objCmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index");


        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(MST_ContactCategoryModel modelMST_ContactCategory) 
        {

            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;   
            if (modelMST_ContactCategory.ContactCategoryID == null)
            {
                objCmd.CommandText = "PR_MST_ContactCategory_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_MST_ContactCategory_UpdateByPK";
                objCmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = modelMST_ContactCategory.ContactCategoryID;
            }
            objCmd.Parameters.Add("@ContactCategoryName",SqlDbType.NVarChar).Value=modelMST_ContactCategory.ContactCategoryName;
            objCmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = DBNull.Value;
            objCmd.Parameters.Add("@ModificationDate", SqlDbType.Date).Value = DBNull.Value;


            if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
            {
                if (modelMST_ContactCategory.ContactCategoryID == null)
                {
                    TempData["ContactcategoryIinsertMsg"] = "Record Insert Sucessfully....!!!";

                }
                else
                {
                    return RedirectToAction("Index"); ;
                }
            }
            conn.Close();
            return View("MST_ContactCategoryAddEdit");
        }
        #endregion

        #region Add
        public IActionResult Add(int? ContactCategoryID)
        { 
            if(ContactCategoryID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(connectionstr);

                conn.Open();

                SqlCommand objCmd = conn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_MST_ContactCategory_SelectByPK";
                objCmd.Parameters.Add("@ContactCategoryID", SqlDbType.Int).Value = ContactCategoryID;
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);

                MST_ContactCategoryModel modelMST_ContactCategory = new MST_ContactCategoryModel();
                foreach (DataRow dr in dt.Rows)
                {
                 
                    modelMST_ContactCategory.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                    modelMST_ContactCategory.ContactCategoryName = dr["ContactCategoryName"].ToString();
                    modelMST_ContactCategory.CreationDate =Convert.ToDateTime(dr["CreationDate"]);
                    modelMST_ContactCategory.ModificationDate =Convert.ToDateTime(dr["ModificationDate"]);

                }
                conn.Close();
                return View("MST_ContactCategoryAddEdit", modelMST_ContactCategory);
            }  
            return View("MST_ContactCategoryAddEdit");
        }
        #endregion


        #region Filter Records
        public IActionResult Filter(string? ContactCategoryName)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_MST_ContactCategory_Filter";

            if (ContactCategoryName == null)
            {
                ContactCategoryName = "";
            }
           

            cmd.Parameters.Add("@ContactCategoryName", SqlDbType.NVarChar).Value = ContactCategoryName;
        



            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            return View("MST_ContactCategoryList", dt);
        }
        #endregion

        //public IActionResult MST_ContactCategoryList()
        //{
        //    return View();
        //}
    }
}
