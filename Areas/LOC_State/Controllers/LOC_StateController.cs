using AddressBook.DAL;
using AddressBook.Areas.LOC_State.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using AddressBook.Areas.LOC_Country.Models;

namespace AddressBook.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/[Controller]/[action]")]
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
            
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            LOC_DAL dalLOC = new LOC_DAL();
            DataTable dt = dalLOC.dbo_PR_LOC_State_SelectAll(connectionstr);

            return View("LOC_StateList", dt);
            

        }
        #endregion

        #region Delete
        public IActionResult Delete(int StateID)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            LOC_DAL dalLOC = new LOC_DAL();

            if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_State_DeleteByPK(connectionstr, StateID)))
            {
                return RedirectToAction("Index");
            }
            return View("Index");

        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {

            #region Insert


            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");

            LOC_DAL dalLOC = new LOC_DAL();


            if (modelLOC_State.StateID == null)
            {

                if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_State_Insert(connectionstr, modelLOC_State)))
                {
                    TempData["StateInsertMessage"] = "Record inserted successfully....";

                }
            }
            else
            {
                if (Convert.ToBoolean(dalLOC.dbo_PR_LOC_State_UpdateByPK(connectionstr, modelLOC_State)))
                {

                    TempData["StateUpdateMessage"] = "Record Update Successfully....";

                }
                return RedirectToAction("Index");
            }


            return RedirectToAction("Add");

            #endregion
        }
        #endregion

        #region Add
        public IActionResult Add(int StateID)
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



            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }
            ViewBag.CountryList = list;
            conn1.Close();
            #endregion

            #region Select By PK

            if (StateID != null)
            {

                string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
                LOC_DAL dalLOC = new LOC_DAL();

                DataTable dt = dalLOC.dbo_PR_LOC_State_SelectByPK(connectionstr, StateID);
                if (dt.Rows.Count > 0)
                {
                    LOC_StateModel model = new LOC_StateModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CountryID = Convert.ToInt32(dr["CountryID"]);
                        model.StateID = Convert.ToInt32(dr["StateID"]);
                        model.StateName = dr["StateName"].ToString();
                        model.StateCode = dr["StateCode"].ToString();
                        model.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        model.ModificationDate = Convert.ToDateTime(dr["ModificationDate"]);

                    }
                    return View("LOC_StateAddEdit", model);
                }

               
            }
            #endregion

            return View("LOC_StateAddEdit");
        }
        #endregion
       
        #region Filter Records
        public IActionResult Filter(string? StateName, string? StateCode)
        {
            string connectionstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_Filter";

            if (StateName == null)
            {
                StateName = "";
            }
            if (StateCode == null)
            {
                StateCode = "";
            }
            
            cmd.Parameters.Add("@StateName", SqlDbType.NVarChar).Value = StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.NVarChar).Value = StateCode;


        
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            return View("LOC_StateList", dt);
        }
        #endregion

    }
}