using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AddressBook.Models;
using AddressBook.DAL;

namespace AddressBook.Controllers
{
    public class LOC_StateController : Controller
    {
        private IConfiguration Configuration;
        public LOC_StateController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region Index
        public IActionResult Index()
        {
            ;
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_LOC_State_SelectAll(connectionstr);

            return View("LOC_StateList", dt);
            //DataTable dt = new DataTable();
            //SqlConnection conn = new SqlConnection(connectionstr);

            //conn.Open();

            //SqlCommand objCmd = conn.CreateCommand();
            //objCmd.CommandType = CommandType.StoredProcedure;
            //objCmd.CommandText = "PR_LOC_State_SelectAll";
            //SqlDataReader objSDR = objCmd.ExecuteReader();
            //dt.Load(objSDR);

            //conn.Close();

            //return View("LOC_StateList", dt);


        }
        #endregion

        #region Delete
        public IActionResult Delete(int StateID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_State_DeleteByPK";
            objCmd.Parameters.AddWithValue("@StateID", StateID);
            objCmd.ExecuteNonQuery(); 
            conn.Close();

            return RedirectToAction("Index");


        }

        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            if(modelLOC_State.StateID == null)
            {
                objCmd.CommandText = "PR_LOC_State_Insert";
            }
            else 
            {
                objCmd.CommandText = "PR_LOC_State_UpdateByPK";
                objCmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLOC_State.StateID;
            }
            objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_State.CountryID;
            objCmd.Parameters.Add("@StateName", SqlDbType.NVarChar).Value = modelLOC_State.StateName;
            objCmd.Parameters.Add("@StateCode", SqlDbType.NVarChar).Value = modelLOC_State.StateCode;
            objCmd.Parameters.Add("@CreationDate",SqlDbType.DateTime).Value= DBNull.Value;
            objCmd.Parameters.Add("@ModificationDate", SqlDbType.DateTime).Value = DBNull.Value;
            
            if(Convert.ToBoolean(objCmd.ExecuteNonQuery()))
            {
                if(modelLOC_State.StateID == null)
                {
                    TempData["StateIinsertMsg"] = "Record Insert Sucessfully....!!!";
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
        public IActionResult Add(int? StateID)
        {
            string connectionstr1 = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt1 = new DataTable();
            SqlConnection conn1 = new SqlConnection(connectionstr1);

            conn1.Open();

            SqlCommand objCmd1 = conn1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_LOC_Country_SelectForDropDown";
            SqlDataReader objSDR1 = objCmd1.ExecuteReader();
            dt1.Load(objSDR1);
            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach(DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;
            conn1.Close();

            #region Selectbypk
            if (StateID != null)
            { 
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();

            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_State_SelectByPK";
            objCmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            LOC_StateModel modelLOC_State = new LOC_StateModel();
            foreach (DataRow dr in dt.Rows)
            {
                modelLOC_State.CountryID = Convert.ToInt32(dr["CountryID"]);
                modelLOC_State.StateID = Convert.ToInt32(dr["StateID"]);
                modelLOC_State.StateCode =dr["StateCode"].ToString();
                modelLOC_State.StateName = dr["StateName"].ToString();
                modelLOC_State.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                modelLOC_State.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

            }

            conn.Close();
            return View("LOC_StateAddEdit",modelLOC_State);
        }
            #endregion

            return View("LOC_StateAddEdit");
        }
        #endregion
       
    }
}
