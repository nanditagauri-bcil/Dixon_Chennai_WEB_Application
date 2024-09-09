using Common;
using DataLayer.Reports;
using System;
using System.Data;


namespace BusinessLayer.Reports
{
    public class BL_WallMountKitReport
    {
        DL_WallMountKitReport dlboj = null;
        public DataTable BindFGItemCode(string sSiteCode)
        {
            DataTable dt = new DataTable();
            dlboj = new DL_WallMountKitReport();
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
        public DataTable GetReport(string sFromDate, string sTODate, string sFGitemCode, string sWallMountKitBarcode)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_WallMountKitReport();
            try
            {
                dtReport = dlboj.GetReport(sFromDate, sTODate, sFGitemCode, sWallMountKitBarcode);
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
