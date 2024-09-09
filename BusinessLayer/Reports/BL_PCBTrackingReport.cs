using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_PCBTrackingReport
    {
        DL_PCBTrackingReport dlboj = null;
        public DataTable GetMachineID()
        {
            dlboj = new DL_PCBTrackingReport();
            try
            {
                return dlboj.dtBindMachineID();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }
        public DataTable GetFGItemCode()
        {
            dlboj = new DL_PCBTrackingReport();
            try
            {
                return dlboj.dtBindFGItemCode();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }

        public DataTable GetReport(string sMachineID,
            string sFromDate, string sTODate
            , string sFgItemCode, string sType,
            string sBarcode, string sWorkOrderNo, string sReportType, string sEID, string sIMEI)
        {
            dlboj = new DL_PCBTrackingReport();
            try
            {
                return dlboj.GetReportData(sMachineID,
                      sFromDate, sTODate
            , sFgItemCode, sType, sBarcode, sWorkOrderNo, sReportType, sEID, sIMEI
                    );
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }

        public DataTable Getmasterdata(string sBarocode)
        {
            dlboj = new DL_PCBTrackingReport();
            try
            {
                return dlboj.Getmasterdata(sBarocode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }
    }
}
