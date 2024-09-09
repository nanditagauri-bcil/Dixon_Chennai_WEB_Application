using Common;
using System;
using System.Data;

namespace DataLayer.Reports
{
    public class DL_IMEIandEID_Unbind_Report
    {
        DBManager oDbm;
        public DL_IMEIandEID_Unbind_Report()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }
        public DataTable GetReport(string sFromDate, string sTODate, string sPCBID, string sMACID, string sIMEI, string sEID, string sCHIPID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(8);
                oDbm.AddParameters(0, "@TYPE", "GETREPORT");
                oDbm.AddParameters(1, "@FROMDATE", sFromDate);
                oDbm.AddParameters(2, "@TODATE", sTODate);
                oDbm.AddParameters(3, "@PCBID", sPCBID);
                oDbm.AddParameters(4, "@MACID", sMACID);
                oDbm.AddParameters(5, "@IMEI", sIMEI);
                oDbm.AddParameters(6, "@EID", sEID);
                oDbm.AddParameters(7, "@CHIPID", sCHIPID);
                dtResult = oDbm.ExecuteDataSet(CommandType.StoredProcedure, "USP_IMEIandEID_Unbind_Report").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dtResult;
        }
    }
}
