using AddressBook.DAL;
using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AddressBook.Controllers
{
    public class LOC_CountryController : Controller
    {

        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region Index
        public IActionResult Index()
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");

            LOC_DAL dalLOC = new LOC_DAL(); 
            DataTable dt = dalLOC.dbo_PR_LOC_Country_SelectAll(connectionstr);

            return View("LOC_CountryList",dt);
            //DataTable dt = new DataTable();
            //SqlConnection conn = new SqlConnection(connectionstr);

            //conn.Open();

            //SqlCommand objCmd = conn.CreateCommand();
            //objCmd.CommandType = CommandType.StoredProcedure;
            //objCmd.CommandText = "PR_LOC_Country_SelectAll";
            //SqlDataReader objSDR = objCmd.ExecuteReader();
            //dt.Load(objSDR);

            //conn.Close();

            return View("LOC_CountryList", dt);

           
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
            objCmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Index") ;


        }

        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_Country.CountryID == null)
            {

                objCmd.CommandText = "PR_LOC_Country_Insert";

            }
            else
            {
                objCmd.CommandText = "PR_LOC_Country_UpdateByPK";
                objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_Country.CountryID;
            }

            objCmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = modelLOC_Country.CountryName;
            objCmd.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = modelLOC_Country.CountryCode;
            objCmd.Parameters.Add("@CreationDate", SqlDbType.Date).Value = DBNull.Value;
            objCmd.Parameters.Add("@ModificationDate", SqlDbType.Date).Value = DBNull.Value;


            if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
            {
                if (modelLOC_Country.CountryID == null)
                {
                    TempData["CountryInsertMessage"] = "Record Insert Successfully..!!!";
                }
                else
                {
                    return RedirectToAction("Index"); ;
                }

            }

            conn.Close();
            return View("LOC_CountryAddEdit");
        }

        #endregion

        #region Add
        public IActionResult Add(int? CountryID)
        {
            if (CountryID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(connectionstr);

                conn.Open();

                SqlCommand objCmd = conn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_Country_SelectByPK";
                objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);

                LOC_CountryModel modellOC_CountryModel = new LOC_CountryModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modellOC_CountryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modellOC_CountryModel.CountryName = dr["CountryName"].ToString();
                    modellOC_CountryModel.CountryCode = dr["CountryCode"].ToString();
                    modellOC_CountryModel.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modellOC_CountryModel.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

                    return View("LOC_CountryAddEdit", modellOC_CountryModel);
                }
                conn.Close();
            }
            return View("LOC_CountryAddEdit");
        }

        #endregion
        public IActionResult LOC_CountryList()
        {
            return View();
        }
      
    }
}
