using Common;
using System;
using System.Data;

namespace DataLayer
{
    public class DL_DDRPartMaster
    {
        DBManager oDbm;
        public DL_DDRPartMaster()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }

        public DataTable BindFGItemCode(string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "BINDFGITEMCODE");
                oDbm.AddParameters(1, "@SITECODE", sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DDRPART_MASTER").Tables[0];
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
        public DataTable GetDDR()
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "GETDDRDETAILS");
                oDbm.AddParameters(1, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DDRPART_MASTER").Tables[0];
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
        public DataTable Deleteid(string sID)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "DELETEDDRDETAILS");
                oDbm.AddParameters(1, "@ID", sID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DDRPART_MASTER").Tables[0];
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
        public DataSet GetDDRByID(string sFGITEMCODE, string sID)
        {
            DataSet ds = new DataSet();
            try
            {
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@TYPE", "GETDDRDETAILSBYID");
                oDbm.AddParameters(1, "@ID", sID);
                oDbm.AddParameters(2, "@FGITEMCODE", sFGITEMCODE);
                oDbm.AddParameters(3, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                ds = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DDRPART_MASTER");
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
            return ds;
        }

        public DataTable SaveDDR(string sFGITEMCODE, string sBOMPARTCODE, string sDDRPARTCODE, string sDESC, string sUSERID,
                              string sSITECODE, string sLINECODE, string sTYPE, string sID)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(9);
                oDbm.AddParameters(0, "@TYPE", sTYPE);
                oDbm.AddParameters(1, "@LINEID", sLINECODE);
                oDbm.AddParameters(2, "@SITECODE", sSITECODE);
                oDbm.AddParameters(3, "@DDRDESC", sDESC);
                oDbm.AddParameters(4, "@CREATEDBY", sUSERID);
                oDbm.AddParameters(5, "@DDRPARTCODE", sDDRPARTCODE);
                oDbm.AddParameters(6, "@BOMPARTCODE", sBOMPARTCODE);
                oDbm.AddParameters(7, "@FGITEMCODE", sFGITEMCODE);
                oDbm.AddParameters(8, "@ID", sID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DDRPART_MASTER").Tables[0];
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
