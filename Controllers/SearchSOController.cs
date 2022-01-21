using HDBAPI.Model;
using HDBAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace HDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchSOController : ControllerBase
    {
        private readonly IConfiguration _config;
        //private readonly UserManager<ApplicationUser> _userMgr;
        //private readonly SignInManager<ApplicationUser> _signMgr;
        public SearchSOController(IConfiguration config)
        {
            _config = config;
        }

        string sql;

        [HttpGet("SO/{docnum}")]
        public IEnumerable<SOModel> GetWhere(int docnum)
        {
            sql = " Select \"DocNum\", \"DocDate\", \"DocDueDate\", \"NumAtCard\" from IPM_LIVE.ORDR Where \"DocNum\" = " + docnum;

            HRunQuery qu_ALL = new HRunQuery();
            DataTable _myData = qu_ALL.HOpenAdoQuery(sql, "Bosku");
            List<SOModel> listdata = new List<SOModel>(_myData.Rows.Count);
            if (_myData.Rows.Count > 0)
            {

                foreach (DataRow datarecord in _myData.Rows)
                {
                    listdata.Add(new ReadData(datarecord));
                }

            }
            return listdata;
        }
    }
}
