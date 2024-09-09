using Common;
using System;
using System.Data;

namespace DataLayer.Reports
{
    public class DL_ODU
    {
        DBManager oDbm;

        public DL_ODU()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }
        public DataTable GetReportODU(string sPONO, string Type)
        {
            DataTable dtResult = new DataTable();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@PONO", sPONO);
                oDbm.AddParameters(1, "@TYPE", Type);
                dtResult = oDbm.ExecuteDataSet(CommandType.StoredProcedure, "USP_ODU_Report").Tables[0];
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
