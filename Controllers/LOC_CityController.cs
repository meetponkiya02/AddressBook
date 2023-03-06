using AddressBook.DAL;
using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;



namespace AddressBook.Controllers
{
    public class LOC_CityController : Controller
    {
        private IConfiguration Configuration;
        public LOC_CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region Index
        public IActionResult Index()
        {
            #region SelectAll


            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_LOC_City_SelectAll(connectionstr);
            return View("LOC_CityList", dt);

            /* DataTable dt = new DataTable();
             SqlConnection conn = new SqlConnection(connectionstr);
             conn.Open();
             SqlCommand objCmd = conn.CreateCommand();
             objCmd.CommandType = CommandType.StoredProcedure;
             objCmd.CommandText = "PR_LOC_City_SelectAll";
             SqlDataReader objSDR = objCmd.ExecuteReader();
             dt.Load(objSDR);
             conn.Close();*/

            #endregion
        }
        #endregion#region Index


        #region Delete
        public IActionResult Delete(int CityID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");

            LOC_DAL dalLOC = new LOC_DAL();

            if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_City_DeleteByPK(connectionstr, CityID)))
            {
                return RedirectToAction("Index");
            }
            return View("Index");

            /*SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CityID", CityID);
            objCmd.ExecuteNonQuery();
            conn.Close();
*/

        }
        #endregion  


        #region Add
        public IActionResult Add(int CityID)
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



            #endregion


            #region Select By PK

            if (CityID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                LOC_DAL dalLOC = new LOC_DAL();

                DataTable dt = dalLOC.dbo_PR_LOC_City_SelectByPK(connectionstr, CityID);
                if (dt.Rows.Count > 0)
                {
                    LOC_CityModel model = new LOC_CityModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        DropDownByCountry(Convert.ToInt32(dr["CountryID"]));
                        model.StateID = Convert.ToInt32(dr["StateID"]);
                        model.CityID = Convert.ToInt32(dr["CityID"]);
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.CityName = dr["CityName"].ToString();
                        model.PinCode = dr["PinCode"].ToString();
                        model.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        model.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);
                    }
                    return View("LOC_CityAddEdit", model);
                }


                /*string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand objCmd = conn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_City_SelectByPK";
                objCmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);
                LOC_CityModel  modelLOC_City = new LOC_CityModel();
                foreach (DataRow dr in dt.Rows)
                {
                    DropDownByCountry(Convert.ToInt32(dr["CountryID"]));
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_City.CityName = dr["CityName"].ToString();
                    modelLOC_City.PinCode = dr["PinCode"].ToString();
                    modelLOC_City.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                    modelLOC_City.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);
                    return View("LOC_CityAddEdit", modelLOC_City);
                }
                conn.Close();
                // Aya Levanu baki chhe*/
            }
            #endregion

            return View("LOC_CityAddEdit");
        }
        #endregion


        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {


            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");

            LOC_DAL dalLOC = new LOC_DAL();


            if (modelLOC_City.CityID == null)
            {

                if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_City_Insert(connectionstr, modelLOC_City)))
                {
                    TempData["CityInsertMessage"] = "Record inserted successfully";

                }
            }
            else
            {
                if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_City_UpdateByPK(connectionstr, modelLOC_City)))
                {

                    TempData["CityUpdateMessage"] = "Record Update Successfully";

                }
                return RedirectToAction("Index");
            }



            return RedirectToAction("Add");
        }
        #endregion

        #region DropDownByCountry
        public IActionResult DropDownByCountry(int CountryID)
        {
            #region State DropDown
            string connectionstr1 = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt1 = new DataTable();
            SqlConnection conn1 = new SqlConnection(connectionstr1);
            conn1.Open();
            SqlCommand objCmd1 = conn1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_LOC_State_SelectForDropDown";
            objCmd1.Parameters.AddWithValue("@CountryID",CountryID);
            SqlDataReader objSDR1 = objCmd1.ExecuteReader();
            dt1.Load(objSDR1);
            conn1.Close();
            List<LOC_StateDropDownModel> list = new List<LOC_StateDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_StateDropDownModel vlst = new LOC_StateDropDownModel();
                vlst.StateID = Convert.ToInt32(dr["StateID"]);
                vlst.StateName = dr["StateName"].ToString();
                list.Add(vlst);
            }
            ViewBag.StateList=list; 
            var vModel = list;
            return Json(vModel);
            #endregion
        }
        #endregion

        #region Filter Records
        public IActionResult Filter(string? CityName, string? PinCode)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_Filter";

            if (CityName == null)
            {
                CityName = "";
            }
            if (PinCode == null)
            {
                PinCode = "";
            }

            cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = CityName;
            cmd.Parameters.Add("@PinCode", SqlDbType.NVarChar).Value = PinCode;



            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            return View("LOC_CityList", dt);
        }
        #endregion

    }
}
