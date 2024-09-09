using Common;
using DataLayer;
using System;
using System.Data;
using System.Text;

namespace BusinessLayer
{
    public class BL_SubMachineMaster
    {
        StringBuilder sb = null;
        DL_WIPSubMachineMaster dlboj = null;
        public DataTable GetMachineID(string sSiteCode)
        {
            DataTable dtSubMachineMaster = new DataTable();
            try
            {
                dlboj = new DL_WIPSubMachineMaster();
                dtSubMachineMaster = dlboj.BindMachine(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtSubMachineMaster;
        }
        public DataTable GetSubMachineRecord(string sSiteCode)
        {
            DataTable dtSubMachineMaster = new DataTable();
            try
            {
                dlboj = new DL_WIPSubMachineMaster();
                dtSubMachineMaster = dlboj.GetSubMachineRecord(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtSubMachineMaster;
        }

        public DataTable GetSeletedData(string sSubMachineCode, string sSiteCode)
        {
            DataTable dt = new DataTable();
            dlboj = new DL_WIPSubMachineMaster();
            try
            {
                dt = dlboj.FillSubMachineDetails(sSubMachineCode, sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }

        public string SaveSubMachineMstData(string _sMachineID, string sSubMachineCode, string sSubMachineName,
            string sSubMachineDesc, string _sCreBy, string sSiteCode)
        {
            dlboj = new DL_WIPSubMachineMaster();
            string sResult = string.Empty;
            try
            {
                DataTable dt = dlboj.SaveSubMachine(_sMachineID, sSubMachineCode, sSubMachineName, sSubMachineDesc
                                , _sCreBy, sSiteCode);
                if (dt.Rows.Count > 0)
                {
                    sResult = dt.Rows[0][0].ToString();
                }
                else
                {
                    sResult = "N~No result found";
                }
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return sResult;
        }
        public string UpdateSubMachineRecords(string _sMachineID, string sSubMachineCode, string sSubMachineName,
            string sSubMachineDesc, string _sCreBy, string sSiteCode)
        {
            string sResult = string.Empty;
            dlboj = new DL_WIPSubMachineMaster();
            try
            {
                DataTable dt = dlboj.UpdateSubMachine(_sMachineID, sSubMachineCode, sSubMachineName, sSubMachineDesc
                                , _sCreBy, sSiteCode);
                if (dt.Rows.Count > 0)
                {
                    sResult = dt.Rows[0][0].ToString();
                }
                else
                {
                    sResult = "N~No result found";
                }
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return sResult;
        }

        public string DeleteSubMachine(string _SN, string sSiteCode)
        {
            string sResult = string.Empty;
            dlboj = new DL_WIPSubMachineMaster();
            try
            {
                DataTable dt = dlboj.DeleteSubMachine(_SN, sSiteCode);
                if (dt.Rows.Count > 0)
                {
                    sResult = dt.Rows[0][0].ToString();
                }
                else
                {
                    sResult = "N~No result found";
                }
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return sResult;
        }




    }
}
