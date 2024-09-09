using BcilLib;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DataLayer.WIP
{
    public class DL_WIP_RSN_Update : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_WIP_RSN_Update()
        {
            odb = SqlDBProvider();
        }

        public DataTable dlRSN(string sRSN, string sSiteCode)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(3);
                odb.AddParameters(0, "@TYPE", "RSNSCAN");
                odb.AddParameters(1, "@RSN", sRSN);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_RSN_UPDATE").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(EventNotice.EventTypes.evtError, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtobj;
        }

        public DataTable SaveUpdateRsnMonth(string sRSN, string smacID, string sCurrentRsnMonth,
            string _sNewRsn, string _sCreBy, string sSiteCode, string sLineCode, string swifikey,
            string sSSID)
        {
            DataTable dt = new DataTable();
            try
            {
                odb.CreateParameters(10);
                odb.AddParameters(0, "@TYPE", "SAVEUPDATERSNMONTH");
                odb.AddParameters(1, "@RSN", sRSN);
                odb.AddParameters(2, "@MACID", smacID);
                odb.AddParameters(3, "@CURRENTRSNMONTH", sCurrentRsnMonth);
                odb.AddParameters(4, "@NEWRSN", _sNewRsn);
                odb.AddParameters(5, "@CREATED_BY", _sCreBy);
                odb.AddParameters(6, "@SITECODE", sSiteCode);
                odb.AddParameters(7, "@LINECODE", sLineCode);
                odb.AddParameters(8, "@WIFIKEY", swifikey);
                odb.AddParameters(9, "@SSID", sSSID);
                odb.Open();
                dt = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_RSN_UPDATE").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dt;
        }

    }
}
