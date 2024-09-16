using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_WIPRsn_And_MacID_Mapping_Report
    {
        DL_WIPRsn_And_MacID_Mapping_Report dlboj = null;

        public DataTable BindFGItemCode(string sSiteCode)
        {
            DataTable dt = new DataTable();
            dlboj = new DL_WIPRsn_And_MacID_Mapping_Report();
            try
            {
                dt = dlboj.BindFGItemCode(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }
        public DataTable GetReport(string sFromDate, string sTODate, string sPCBID, string sFgItemCode)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_WIPRsn_And_MacID_Mapping_Report();
            try
            {
                dtReport = dlboj.GetReport(sFromDate, sTODate, sPCBID, sFgItemCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReport;
        }
    }
}
