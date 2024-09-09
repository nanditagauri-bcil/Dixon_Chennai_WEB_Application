using Common;
using DataLayer.Reports;
using System;
using System.Data;


namespace BusinessLayer.Reports
{
    public class BL_MSNvsGBComparisonReport
    {
        DL_MSNvsGBComparisonReport dlboj = null;
        public DataTable BindFGItemCode(string sSiteCode)
        {
            DataTable dtGRN = new DataTable();
            dlboj = new DL_MSNvsGBComparisonReport();
            try
            {
                dtGRN = dlboj.BindFGItemCode(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtGRN;
        }
        public DataTable GetReport(string sFromDate, string sTODate, string sFGitemCode, string sMSN)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_MSNvsGBComparisonReport();
            try
            {
                dtReport = dlboj.GetReport(sFromDate, sTODate, sFGitemCode, sMSN);
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
