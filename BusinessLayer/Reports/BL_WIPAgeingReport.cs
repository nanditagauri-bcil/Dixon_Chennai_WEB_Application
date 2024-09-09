using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_WIPAgeingReport
    {
        DL_WIPAgeingReport dlobj;
        public DataTable BindPartCode()
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_WIPAgeingReport();
            try
            {
                dtGRN = dlobj.BindFGItemCode();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtGRN;
        }

        public DataTable GetMachineID()
        {
            dlobj = new DL_WIPAgeingReport();
            try
            {
                return dlobj.dtBindMachineID();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }

        public DataTable GetWIPAgeingReport(string sPartCode , string sFromDate, string sToDate ,string sMachineID)
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_WIPAgeingReport();
            try
            {
                dtGRN = dlobj.GetWIPAgeingReport(sPartCode  ,  sFromDate, sToDate, sMachineID);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtGRN;
        }

         
    }
}
