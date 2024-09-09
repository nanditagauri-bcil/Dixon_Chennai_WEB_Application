using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_WIPHoldPartReport
    {
        DL_WIPHoldPartReport dlobj;
        public DataTable BindPartCode()
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_WIPHoldPartReport();
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

        public DataTable GetWIPHoldPartReport(string sPartCode , string sFromDate, string sToDate )
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_WIPHoldPartReport();
            try
            {
                dtGRN = dlobj.GetWIPHoldPartReport(sPartCode  ,  sFromDate, sToDate );
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
