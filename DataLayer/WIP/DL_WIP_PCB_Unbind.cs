using BcilLib;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DataLayer.WIP
{
    public class DL_WIP_PCB_Unbind : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_WIP_PCB_Unbind()
        {
            odb = SqlDBProvider();
        }

        public DataTable VALIDATEPCBID(string spcbid, string sSiteCode)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(3);
                odb.AddParameters(0, "@TYPE", "VALIDATEPCBID");
                odb.AddParameters(1, "@PCBID", spcbid);
                odb.AddParameters(2, "@SITECODE", sSiteCode);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_PCB_UNBIND").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(EventNotice.EventTypes.evtError, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtobj;
        }

        public DataTable VALIDATESUBPCBID(string spcbid, string sSUBpcbid, string sSiteCode)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(4);
                odb.AddParameters(0, "@TYPE", "VALIDATESUBPCBID");
                odb.AddParameters(1, "@PCBID", spcbid);
                odb.AddParameters(2, "@SUBPCBID", sSUBpcbid);
                odb.AddParameters(3, "@SITECODE", sSiteCode);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_PCB_UNBIND").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(EventNotice.EventTypes.evtError, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtobj;
        }

        public DataTable UNBINDSUBPCBID(string spcbid, string sSUBpcbid, string sSiteCode, string sLineCode, string sUserID)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(6);
                odb.AddParameters(0, "@TYPE", "UNBINDSUBPCBID");
                odb.AddParameters(1, "@PCBID", spcbid);
                odb.AddParameters(2, "@SUBPCBID", sSUBpcbid);
                odb.AddParameters(3, "@SITECODE", sSiteCode);
                odb.AddParameters(4, "@LINECODE", sLineCode);
                odb.AddParameters(5, "@USERID", sUserID);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_PCB_UNBIND").Tables[0];
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(EventNotice.EventTypes.evtError, MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            finally
            {
                odb.Close();
                odb.Dispose();
            }
            return dtobj;
        }


    }
}
