using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_MasterReports
    {
        public DataTable GetReports(string sSiteCode, string sType, string sFilter)
        {
            DataTable dtGRN = new DataTable();
            DL_MasterReport dlobj = new DL_MasterReport();
            try
            {
                dtGRN = dlobj.GetReport(sSiteCode, sType, sFilter);
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
