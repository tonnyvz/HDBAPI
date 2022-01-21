using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HDBAPI.Model
{
    public class RMModel
    {
        public string Film { get; set; }
        public decimal OnHandKG { get; set; }
        public decimal PlannedKG { get; set; }
        public decimal OnOrderKG { get; set; }
        public decimal Variance { get; set; }
    }

    public class ReadRM : RMModel
    {
        public ReadRM(DataRow row)
        {
            Film = row["FilmGroup"].ToString();            
            OnHandKG = (decimal)row["OnHandKG"];
            PlannedKG = (decimal)row["PlannedKG"];
            OnOrderKG = (decimal)row["OnOrderKG"];
            Variance = (decimal)row["Variance"];
        }
    }
}
