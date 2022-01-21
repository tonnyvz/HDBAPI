using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HDBAPI.Model
{
    public class SOModel
    {
        public string DocNum { get; set; }
        public string DocDate { get; set; }
        public string NumAtCard { get; set; }
        public DateTime DocDueDate { get; set; }
    }

    public class ReadData : SOModel
    {
        public ReadData(DataRow row)
        {
            DocNum = row["DocNum"].ToString();
            DateTime date = Convert.ToDateTime(row["DocDate"]);
            DocDate = date.ToString("yyyy/MM/dd");
            NumAtCard = row["NumAtCard"].ToString();
            DocDueDate = Convert.ToDateTime(row["DocDueDate"]);
        }
    }
}
