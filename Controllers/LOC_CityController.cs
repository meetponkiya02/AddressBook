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
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_LOC_City_SelectAll(connectionstr);

            return View("LOC_CityList", dt);


            //DataTable dt = new DataTable();
            //SqlConnection conn = new SqlConnection(connectionstr);

            //conn.Open();

            //SqlCommand objCmd = conn.CreateCommand();
            //objCmd.CommandType = CommandType.StoredProcedure;
            //objCmd.CommandText = "PR_LOC_City_SelectAll";
            //SqlDataReader objSDR = objCmd.ExecuteReader();
            //dt.Load(objSDR);
            //conn.Close();

            //return View("LOC_CityList", dt);


        }
        #endregion
          
        #region Delete
        public IActionResult Delete(int CityID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CityID", CityID);
            objCmd.ExecuteNonQuery();   
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion  

        #region Add
        public IActionResult Add(int? CityID)
        {
            #region DropDownCountry

            string connectionstr2 = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt2 = new DataTable();
            SqlConnection conn2 = new SqlConnection(connectionstr2);

            conn2.Open();

            SqlCommand objCmd2 = conn2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_LOC_Country_SelectForDropDown";
            SqlDataReader objSDR2 = objCmd2.ExecuteReader();
            dt2.Load(objSDR2);
            conn2.Close();
            List<LOC_CountryDropDownModel> list2 = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt2.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list2.Add(vlst);
            }
            ViewBag.CountryList = list2;

            

            List<LOC_StateDropDownModel> list = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list;

            #endregion

            if (CityID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                DataTable dt = new DataTable();
                SqlConnection conn = new SqlConnection(connectionstr);
                conn.Open();
                SqlCommand objCmd = conn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_City_SelectByPK";
                objCmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);

                LOC_CityModel modelLOC_City = new LOC_CityModel();
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

                }
                conn.Close();
                return View("LOC_CityAddEdit", modelLOC_City);

            }
            return View("LOC_CityAddEdit");
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
           
            if(modelLOC_City.CityID == null)
            {
                objCmd.CommandText = "PR_LOC_City_Insert";
            }
            else  
            {
                objCmd.CommandText = "PR_LOC_City_UpdateByPK";
                objCmd.Parameters.Add("@CityID",SqlDbType.Int).Value=modelLOC_City.CityID;
            }
            objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_City.CountryID;
            objCmd.Parameters.Add("@StateID",SqlDbType.Int).Value=modelLOC_City.StateID;
            objCmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = modelLOC_City.CityName;
            objCmd.Parameters.Add("@PinCode", SqlDbType.NVarChar).Value = modelLOC_City.PinCode;
            objCmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = DBNull.Value;
            objCmd.Parameters.Add("@ModificationDate", SqlDbType.DateTime).Value = DBNull.Value;
            
            if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
            {
                if(modelLOC_City.CityID == null)
                {
                    TempData["CityIinsertMsg"] = "Record Insert Sucessfully....!!!";
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            conn.Close();
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
