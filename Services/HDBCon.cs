using System;
using System.Data;
using Sap.Data.Hana;

namespace HDBAPI.Services
{
    //public class HDBCon
    //{

    //}

    public class HRunQuery : DataTable
    {
        HanaConnection myCon;

        public DataTable HOpenAdoQuery(string StrQuery, string CompanyName)
        {
            string sConn = "server=192.168.1.199:30015;userid=SYSTEM;password=1Pmit2019#01";

            myCon = new HanaConnection(sConn);
            DataTable myData = new DataTable();

            try
            {
                myCon.Open();
                HanaDataAdapter myDA = new HanaDataAdapter();
                myDA.SelectCommand = new HanaCommand(StrQuery, myCon);
                myDA.Fill(myData);
                myCon.Close();
            }
            catch
            {
                myCon.Close();
            }
            return myData;
        }
    }
}
