using Common;
using DataLayer.Reports;
using System;
using System.Data;


namespace BusinessLayer.Reports
{
    public class BL_ODU
    {
        DL_ODU dlobj;
        public DataTable GetReportODU(string sPONO, string Type)
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_ODU();
            try
            {
                dtGRN = dlobj.GetReportODU(sPONO, Type);
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
