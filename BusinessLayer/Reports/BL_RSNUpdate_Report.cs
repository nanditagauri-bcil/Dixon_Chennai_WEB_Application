using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_RSNUpdate_Report
    {
        DL_RSNUpdate_Report dlboj = null;

        public DataTable GetReport(string sFromDate, string sTODate, string sRSN)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_RSNUpdate_Report();
            try
            {
                dtReport = dlboj.GetReport(sFromDate, sTODate, sRSN);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReport;
        }


        public DataTable BindProdFGItemCode()
        {
            DataTable dtGRN = new DataTable();
            dlboj = new DL_RSNUpdate_Report();
            try
            {
                dtGRN = dlboj.BindModel();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtGRN;
        }
        public DataTable GetProdDataReport(string sFromDate, string sTODate, string sFGitemCode)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_RSNUpdate_Report();
            try
            {
                dtReport = dlboj.GetProdDataReport(sFromDate, sTODate, sFGitemCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReport;
        }

        public DataTable BindpdiFGItemCode()
        {
            DataTable dtGRN = new DataTable();
            dlboj = new DL_RSNUpdate_Report();
            try
            {
                dtGRN = dlboj.BindpdiFGItemCode();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtGRN;
        }
        public DataTable GetpdiReport(string sFromDate, string sTODate, string sFGitemCode, string sPCBID)
        {
            DataTable dtReport = new DataTable();
            dlboj = new DL_RSNUpdate_Report();
            try
            {
                dtReport = dlboj.GetpdiReport(sFromDate, sTODate, sFGitemCode, sPCBID);
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
