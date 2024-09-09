using Common;
using DataLayer.Reports;
using System;
using System.Data;

namespace BusinessLayer.Reports
{
    public class BL_WIP_Wave_Pallet_Report
    {
        DL_WIP_Wave_Pallet_Report dlobj;

        public DataTable BindFGItemCode()
        {
            DataTable dt = new DataTable();
            dlobj = new DL_WIP_Wave_Pallet_Report();
            try
            {
                dt = dlobj.BindFGItemCode();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }

        public DataTable GetReport(string sFGItemCode)
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_WIP_Wave_Pallet_Report();
            try
            {
                dtGRN = dlobj.GetReport(sFGItemCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtGRN;
        }
        public DataTable GetToolDetailsReport(string sFromDate, string sTODate, string sFGItemCode)
        {
            DataTable dtGRN = new DataTable();
            dlobj = new DL_WIP_Wave_Pallet_Report();
            try
            {
                dtGRN = dlobj.GetToolDetailsReport(sFromDate, sTODate, sFGItemCode);
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
