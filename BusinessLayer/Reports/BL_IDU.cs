using Common;
using DataLayer.Reports;
using System;
using System.Data;


namespace BusinessLayer.Reports
{
    public class BL_IDU
    {
        DL_IDU dlobj;
        public DataTable GetReportIDU(string sPONO, string Type)
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_IDU();
            try
            {
                dtGRN = dlobj.GetReportIDU(sPONO, Type);
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
