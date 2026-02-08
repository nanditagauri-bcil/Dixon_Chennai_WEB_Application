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

        //public DataTable BindPartCode()
        //{
        //    DataTable dtResult = new DataTable();
        //    try
        //    {
        //        oDbm.Open();
        //        oDbm.CreateParameters(1);
        //        oDbm.AddParameters(0, "@TYPE", "BINDPARTCODE");
        //        dtResult = oDbm.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_GET_SPLIT_HISTORY").Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oDbm.Close();
        //        oDbm.Dispose();
        //    }
        //    return dtResult;
        //}

        public DataTable GetReport(string fromDate, string toDate, string sPartCode)
        {
            DataTable dtResult = new DataTable();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(3);
                oDbm.AddParameters(0, "@FROMDATE", fromDate);
                oDbm.AddParameters(1, "@TODATE", toDate);
                oDbm.AddParameters(2, "@BARCODE", sPartCode);
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
