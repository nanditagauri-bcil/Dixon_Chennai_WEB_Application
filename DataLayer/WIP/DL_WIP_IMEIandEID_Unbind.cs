using BcilLib;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DataLayer.WIP
{
    public class DL_WIP_IMEIandEID_Unbind : DCommon
    {
        DBManager odb;
        DataTable dtobj;
        public DL_WIP_IMEIandEID_Unbind()
        {
            odb = SqlDBProvider();
        }
        public DataTable UNBINDIDs(string sPcbid, string sMacid, string sImeiid, string sEidid,string sSiteCode, 
                            string sUserID, string sChipid, string sRadiolistType)
        {
            dtobj = new DataTable();
            try
            {
                odb.CreateParameters(9);
                odb.AddParameters(0, "@TYPE", "UNBINDID");
                odb.AddParameters(1, "@PCBID", sPcbid);
                odb.AddParameters(2, "@MACID", sMacid);
                odb.AddParameters(3, "@IMEIID", sImeiid);
                odb.AddParameters(4, "@EIDID", sEidid);
                odb.AddParameters(5, "@SITECODE", sSiteCode);
                odb.AddParameters(6, "@USERID", sUserID);
                odb.AddParameters(7, "@CHIPID", sChipid);
                odb.AddParameters(8, "@RADIOLISTTYPE", sRadiolistType);
                odb.Open();
                dtobj = odb.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WIP_IMEIandEID_UNBIND").Tables[0];
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
