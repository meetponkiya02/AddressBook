using AddressBook.DAL;
using AddressBook.Areas.LOC_Country.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AddressBook.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/[Controller]/[action]")]
    public class LOC_CountryController : Controller
    {

        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region Index
        #region Select All
        public IActionResult Index()
        {

            string connectionstr = Configuration.GetConnectionString("myConnectionStrings");
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_LOC_Country_SelectAll(connectionstr);

            return View("LOC_CountryList", dt);


        }
        #endregion
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            string connectionstr = Configuration.GetConnectionString("myConnectionStrings");

            LOC_DAL dalLOC = new LOC_DAL();

            if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_Country_DeleteByPK(connectionstr, CountryID)))
            {
                return RedirectToAction("Index");
            }
            return View("Index");


            /*DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_DeleteByPK";
           
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
            objCmd.ExecuteNonQuery();
            
            conn.Close();*/


        }
        #endregion

        #region Save 
        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {

            string connectionstr = Configuration.GetConnectionString("myConnectionStrings");

            LOC_DAL dalLOC = new LOC_DAL();


            if (modelLOC_Country.CountryID == null)
            {

                if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_Country_Insert(connectionstr, modelLOC_Country)))
                {
                    TempData["CountryInsertMessage"] = "Record inserted successfully";

                }
            }
            else
            {
                if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_Country_UpdateByPK(connectionstr, modelLOC_Country)))
                {

                    TempData["CountryUpdateMessage"] = "Record Update Successfully";

                }
                return RedirectToAction("Index");
            }



            return RedirectToAction("Add");
        }
        #endregion

        #region Add
        public IActionResult Add(int CountryID)
        {
            if (CountryID != null)
            {
                string connectionstr = Configuration.GetConnectionString("myConnectionStrings");
                LOC_DAL dalLOC = new LOC_DAL();

                DataTable dt = dalLOC.dbo_PR_LOC_Country_SelectByPK(connectionstr, CountryID);
                if (dt.Rows.Count > 0)
                {
                    LOC_CountryModel model = new LOC_CountryModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.CountryName = dr["CountryName"].ToString();
                        model.CountryCode = dr["CountryCode"].ToString();
                        model.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        model.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

                    }
                    return View("LOC_CountryAddEdit", model);
                }
            }
            return View("LOC_CountryAddEdit");
        }

        #endregion

        #region Filter Records
        public IActionResult Filter(string? CountryName, string? CountryCode)
        {
            string connectionstr = Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_Filter";

            if (CountryName == null)
            {
                CountryName = "";
            }
            if (CountryCode == null)
            {
                CountryCode = "";
            }

            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = CountryCode;



            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            return View("LOC_CountryList", dt);
        }
        #endregion

    }
}
