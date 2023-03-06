using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook.Models;
using System.Reflection.Metadata.Ecma335;
using AddressBookMVC.DAL;

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
            #region SelectAll
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            CON_DAL dalCON = new CON_DAL();
            DataTable dt = dalCON.dbo_PR_MST_ContactCategory_SelectAll(connectionstr);

            return View("MST_ContactCategoryList", dt);
            /*DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_MST_ContactCategory_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View("MST_ContactCategoryList", dt);
            #endregion*/

            #endregion
        }

        #endregion

        #region Delete
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
            if (ModelState.IsValid)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");

                CON_DAL dalCON = new CON_DAL();


                if (modelMST_ContactCategory.ContactCategoryID == null)
                {

                    if (Convert.ToBoolean(dalCON.dbo_PR_MST_ContactCategory_Insert(connectionstr, modelMST_ContactCategory)))
                    {
                        TempData["ContactCategoryInsertMessage"] = "Record inserted successfully";

                    }
                }
                else
                {
                    if (Convert.ToBoolean(dalCON.dbo_PR_MST_ContactCategory_UpdateByPK(connectionstr, modelMST_ContactCategory)))
                    {

                        TempData["ContactCategoryUpdateMessage"] = "Record Update Successfully";

                    }
                    return RedirectToAction("Index");
                }

            }


            return RedirectToAction("Add");
        }
        #endregion

        #region Add
        public IActionResult Add(int ContactCategoryID)
        {
            #region Select By PK
            if (ContactCategoryID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                CON_DAL dalCON = new CON_DAL();

                DataTable dt = dalCON.dbo_PR_MST_ContactCategory_SelectByPK(connectionstr, ContactCategoryID);
                if (dt.Rows.Count > 0)
                {
                    MST_ContactCategoryModel model = new MST_ContactCategoryModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.ContactCategoryID = Convert.ToInt32(dr["ContactCategoryID"]);
                        model.ContactCategoryName = dr["ContactCategoryName"].ToString();

                        model.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        model.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

                    }
                    return View("MST_ContactCategoryAddEdit", model);


                }
            }
            #endregion

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

    }
}
