using Common;
using System;
using System.Data;

namespace DataLayer
{
    public class DL_ApplicationSetting
    {
        DBManager oDbm;
        public DL_ApplicationSetting()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }

        public DataTable GetDATA()
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "GETAPPSETTING");
                oDbm.AddParameters(1, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_APPLICATIOPNSETTING").Tables[0];
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
        public DataTable GetAppSettingDataByID(string sID)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "GETDATABYID");
                oDbm.AddParameters(1, "@ID", sID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_APPLICATIOPNSETTING").Tables[0];
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

        public DataTable SaveAppSetting(string sMachineTestCount, string sReworkInOutMaxLimit,
            string sReworkOutMaxTime, string sReworkInMinTime, string sCreatedBy, string sFGItemCode)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(7);
                oDbm.AddParameters(0, "@TYPE", "SAVEAPPSETTING");
                oDbm.AddParameters(1, "@MACHINETESTCOUNT", sMachineTestCount);
                oDbm.AddParameters(2, "@REWORKINOUTMAXLIMIT", sReworkInOutMaxLimit);
                oDbm.AddParameters(3, "@REWORKOUTMAXTIME", sReworkOutMaxTime);
                oDbm.AddParameters(4, "@REWORKINMINTIME", sReworkInMinTime);
                oDbm.AddParameters(5, "@CREATEDBY", sCreatedBy);
                oDbm.AddParameters(6, "@FGITEMCODE", sFGItemCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_APPLICATIOPNSETTING").Tables[0];
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

        public DataTable BindFGItemCode()
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "BINDFGITEMCODE");
                oDbm.AddParameters(1, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_APPLICATIOPNSETTING").Tables[0];
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

        public DataTable UpdateAppSetting(string sMachineTestCount, string sReworkInOutMaxLimit,
            string sReworkOutMaxTime, string sReworkInMinTime, string sCreatedBy, string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(7);
                oDbm.AddParameters(0, "@TYPE", "UPDATEAPPSETTING");
                oDbm.AddParameters(1, "@MACHINETESTCOUNT", sMachineTestCount);
                oDbm.AddParameters(2, "@REWORKINOUTMAXLIMIT", sReworkInOutMaxLimit);
                oDbm.AddParameters(3, "@REWORKOUTMAXTIME", sReworkOutMaxTime);
                oDbm.AddParameters(4, "@REWORKINMINTIME", sReworkInMinTime);
                oDbm.AddParameters(5, "@CREATEDBY", sCreatedBy);
                oDbm.AddParameters(6, "@ID", ID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_APPLICATIOPNSETTING").Tables[0];
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
                oDbm.AddParameters(0, "@TYPE", "DELETEAPPSETTING");
                oDbm.AddParameters(1, "@ID", sID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_APPLICATIOPNSETTING").Tables[0];
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
