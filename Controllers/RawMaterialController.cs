using HDBAPI.Model;
using HDBAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawMaterialController : ControllerBase
    {
        string sql;

        [HttpGet("RawMaterial")]
        public IEnumerable<RMModel> Get()
        {
            sql = " Select ss.\"FilmGroup\", Round(sum(ss.\"OnHand\"),2) as \"OnHandM\", Round(sum(coalesce(ss.\"PlannedQty\",0)),2) as \"PlannedQtyM\", " +
				  " Round(sum(ss.\"OnOrder\"),2) as \"OnOrderM\", Round(sum(ss.\"OnHandKG\"),2) as \"OnHandKG\", Round(sum(ss.\"PlannedKG\"),2) as \"PlannedKG\",  " +
				  " Round(sum(ss.\"OnOrderKG\"),2) as \"OnOrderKG\", Round(sum(ss.\"OnHandKG\") + sum(ss.\"OnOrderKG\") - sum(ss.\"PlannedKG\"),2) as \"Variance\" from ( " +
				  " Select h.\"UgpEntry\", h.\"ItemCode\", h.\"ItemName\", Case " + 
				  " When h.\"ItemCode\" Like 'RFA%' then 'ALU' When h.\"ItemCode\" Like 'RFL%' then 'LLDPE' When h.\"ItemCode\" Like 'RFN%' then 'NYLON'  " +
				  " When h.\"ItemCode\" Like 'RFC%' then 'CPP STD' When h.\"ItemCode\" Like 'RFMC%' then 'CPP VM' When h.\"ItemCode\" Like 'RFMP%' then 'PET VM' " +
				  " When h.\"ItemCode\" Like 'RFPETP%' then 'PET PLAIN' When h.\"ItemCode\" Like 'RFPVDC%' then 'PET PVDC' When h.\"ItemCode\" Like 'RFOPPP%' then 'OPP PLAIN' " + 
				  " When h.\"ItemCode\" Like 'RFOPPM%' then 'OPP MATTE'	When h.\"ItemCode\" like 'RFMOPP%' then 'OPP VM' When h.\"ItemCode\" Like 'RFOPPZ%' then 'OPP PEARLIZE' " +
				  " When h.\"ItemCode\" Like 'RFOPPL%' then 'OPP PEARLIZE' When h.\"ItemCode\" = 'RFOPHS0003' then 'OPP POB/PQH' When h.\"ItemCode\" = 'RFOPHS0004' then 'OPP POB/PQH' " +
				  " When h.\"ItemCode\" = 'RFOPHS0005' then 'OPP POB/PQH' When h.\"ItemCode\" = 'RFOPHS0006' then 'OPP POB/PQH'	When h.\"ItemCode\" = 'RFOPHS0007' then 'OPP POB/PQH' " +
				  " When h.\"ItemCode\" = 'RFOPHS0010' then 'OPP HS' When h.\"ItemCode\" = 'RFOPHS0012' then 'OPP HS' End As \"FilmGroup\",	" +
				  " h.\"WhsCode\",h.\"OnHand\", h.\"PlannedQty\", h.\"OnOrder\",b3.\"BaseQty\", h.\"OnHand\" / b3.\"BaseQty\" as \"OnHandKG\", " + 
				  " h.\"PlannedQty\" / b3.\"BaseQty\" as \"PlannedKG\", h.\"OnOrder\" / b3.\"BaseQty\" as \"OnOrderKG\" from ( " +
				  " Select b2.\"UgpEntry\", b1.\"ItemCode\", b2.\"ItemName\", b1.\"WhsCode\", b1.\"OnHand\", coalesce(b1.\"PlannedQty\",0) as \"PlannedQty\", b1.\"OnOrder\" from ( " + 
				  " Select tx.\"ItemCode\", tx.\"WhsCode\", tx.\"OnHand\", x1.\"PlannedQty\", tx.\"OnOrder\" from IPM_LIVE.OITW tx " +
				  " Left Join (	select x0.\"ItemCode\", sum(x0.\"PlannedQty\") as \"PlannedQty\" from ( " +
				  "	Select t0.\"DocEntry\", t1.\"DocNum\", t0.\"ItemCode\", ((t0.\"PlannedQty\") - (t0.\"IssuedQty\")) as \"PlannedQty\", t1.\"Status\" from IPM_LIVE.WOR1 t0 " + 
				  " Left Join IPM_LIVE.OWOR t1 on t0.\"DocEntry\"=t1.\"DocEntry\" " +
				  " Where t0.\"ItemCode\" Like 'RF%' and (t1.\"Status\"='P' or t1.\"Status\"='R'))x0 Group by x0.\"ItemCode\" )x1 on tx.\"ItemCode\"=x1.\"ItemCode\" " +
				  " Where tx.\"ItemCode\" Like 'RF%' and \"WhsCode\"='WHRM')b1 "+
                  " Left Join IPM_LIVE.OITM b2 on b1.\"ItemCode\"=b2.\"ItemCode\")h " +
                  " Left Join IPM_LIVE.UGP1 b3 on h.\"UgpEntry\"=b3.\"UgpEntry\" " +
                  " Where b3.\"UomEntry\"='22' and(h.\"OnHand\" + coalesce(h.\"PlannedQty\",0) + h.\"OnOrder\") !=0)ss " +
                  " group by ss.\"FilmGroup\" Order by ss.\"FilmGroup\" ";
            HRunQuery qu_ALL = new();
            DataTable _myData = qu_ALL.HOpenAdoQuery(sql, "Bosku");
            List<RMModel> listdata = new(_myData.Rows.Count);
            if (_myData.Rows.Count > 0)
            {

                foreach (DataRow datarecord in _myData.Rows)
                {
                    listdata.Add(new ReadRM(datarecord));
                }

            }
            return listdata;
        }
    }
}
