using Common;
using System;
using System.Data;
namespace DataLayer
{
    public class DL_SplitReelQuality
    {
        DBManager oDbm;
        public DL_SplitReelQuality()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }

        public DataTable BINDINEL_PARTNO(string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "BINDPRINTINGTYPE");
                oDbm.AddParameters(1, "@SITECODE", sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(CommandType.StoredProcedure, "USP_WIP_REELSPLIT_QUALITY").Tables[0];
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

        public DataTable BindReelBarcode(string sPartCode, string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(3);
                oDbm.AddParameters(0, "@TYPE", "BINDREELBARCODE");
                oDbm.AddParameters(1, "@PARTCODE", sPartCode);
                oDbm.AddParameters(2, "@SITECODE", sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_REELSPLIT_QUALITY").Tables[0];
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

        public DataTable SaveQuality(string sPartCode, string sPartBarcode, int qualityType, string qualityBy, string sSiteCode, string sLineCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(7);
                oDbm.AddParameters(0, "@TYPE", "Save_Quality");
                oDbm.AddParameters(1, "@PARTCODE", sPartCode);
                oDbm.AddParameters(2, "@PARTBARCODE", sPartBarcode);
                oDbm.AddParameters(3, "@QualityType", qualityType);
                oDbm.AddParameters(4, "@PRINTEDBY", qualityBy);
                oDbm.AddParameters(5, "@SITECODE", sSiteCode);
                oDbm.AddParameters(6, "@LINECODE", sLineCode);

                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_REELSPLIT_QUALITY").Tables[0];
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
