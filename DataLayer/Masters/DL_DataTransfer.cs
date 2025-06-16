using Common;
using System;
using System.Data;

namespace DataLayer.Masters
{
    public class DL_DataTransfer
    {
        DBManager oDbm;
        public DL_DataTransfer()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }

        public DataTable BindWorkOrderNo(string siteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "BindWorkOrder");
                oDbm.AddParameters(1, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "data_transfer").Tables[0];
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
            return dt;
        }

        public DataTable DataTransfer(string sType, string issueSlipNo, string workOrderNo, string sUserID)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@TYPE", sType);
                oDbm.AddParameters(1, "@VALUE", issueSlipNo);
                oDbm.AddParameters(1, "@WORKORDERNO", workOrderNo);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.AddParameters(3, "@USERID", sUserID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "data_transfer").Tables[0];
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
            return dt;
        }
    }
}
