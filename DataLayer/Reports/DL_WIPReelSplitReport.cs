using Common;
using System;
using System.Data;

namespace DataLayer.Reports
{
    public class DL_WIPReelSplitReport
    {
        DBManager oDbm;
        public DL_WIPReelSplitReport()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }

        public DataTable GetReportTypes()
        {
            DataTable dtResult = new DataTable();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(1);
                oDbm.AddParameters(0, "@TYPE", "GET_REPORT_TYPE");
                dtResult = oDbm.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_GET_SPLIT_HISTORY").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dtResult;
        }

        public DataTable GetReport(string fromDate, string toDate, string sPartCode, string reporType)
        {
            DataTable dtResult = new DataTable();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(5);
                oDbm.AddParameters(0, "@TYPE", "GET_REPORT");
                oDbm.AddParameters(1, "@FROMDATE", fromDate);
                oDbm.AddParameters(2, "@TODATE", toDate);
                oDbm.AddParameters(3, "@BARCODE", sPartCode);
                oDbm.AddParameters(4, "@REPORT_TYPE", reporType);
                dtResult = oDbm.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_GET_SPLIT_HISTORY").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                oDbm.Close();
                oDbm.Dispose();
            }
            return dtResult;
        }

    }
}
