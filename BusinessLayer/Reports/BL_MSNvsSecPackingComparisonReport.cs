using Common;
using DataLayer.Reports;
using System;
using System.Data;


namespace BusinessLayer.Reports
{
    public class BL_MSNvsSecPackingComparisonReport
    {
        DL_MSNvsSecPackingComparisonReport dlboj = null;
        public DataTable BindFGItemCode(string sSiteCode)
        {
            DataTable dt = new DataTable();
            dlboj = new DL_MSNvsSecPackingComparisonReport();
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
        public DataTable GetReport(string sFromDate, string sTODate, string sFGitemCode, string sSecPacking, string sInvoiceNo)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_MSNvsSecPackingComparisonReport();
            try
            {
                dtReport = dlboj.GetReport(sFromDate, sTODate, sFGitemCode, sSecPacking, sInvoiceNo);
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
