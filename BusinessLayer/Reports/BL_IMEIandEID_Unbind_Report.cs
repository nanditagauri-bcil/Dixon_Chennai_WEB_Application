using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_IMEIandEID_Unbind_Report
    {
        DL_IMEIandEID_Unbind_Report dlboj = null;

        public DataTable GetReport(string sFromDate, string sTODate, string sPCBID, string sMACID, string sIMEI, string sEID, string sCHIPID)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_IMEIandEID_Unbind_Report();
            try
            {
                dtReport = dlboj.GetReport(sFromDate, sTODate, sPCBID, sMACID,sIMEI,sEID, sCHIPID);
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
