using Common;
using DataLayer;
using System;
using System.Data;
namespace BusinessLayer
{

    public class BL_ApplicationSetting
    {
        DL_ApplicationSetting dlobj = null;
        public DataTable GetDATA()
        {
            DataTable dt  = new DataTable();
            dlobj = new DL_ApplicationSetting();
            try
            {
                dt  = dlobj.GetDATA();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }

        public DataTable GetAppSettingDataByID(string _SN)
        {
            DataTable dt = new DataTable();
            dlobj = new DL_ApplicationSetting();
            try
            {
                dt = dlobj.GetAppSettingDataByID(_SN);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }
        public string SaveAppSetting(string sMachineTestCount, string sReworkInOutMaxLimit, 
            string sReworkOutMaxTime, string sReworkInMinTime, string _CreatedBy,string sFGItemCode)
        {
            string sResult = string.Empty;
            dlobj = new DL_ApplicationSetting();
            try
            {
                DataTable dt = dlobj.SaveAppSetting(sMachineTestCount, sReworkInOutMaxLimit, sReworkOutMaxTime,
                                sReworkInMinTime, _CreatedBy, sFGItemCode);
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

        public DataTable BindFGITEMCOE()
        {
            DataTable dtFgItemCode = new DataTable();
            dlobj = new DL_ApplicationSetting();
            try
            {
                dtFgItemCode = dlobj.BindFGItemCode();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFgItemCode;
        }

        public string Deleteid(string sID)
        {
            string sResult = string.Empty;
            dlobj = new DL_ApplicationSetting();
            try
            {
                DataTable dt = dlobj.Deleteid(sID);
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
        public string UpdateAppSetting(string sMachineTestCount, string sReworkInOutMaxLimit,
                     string sReworkOutMaxTime, string sReworkInMinTime, string _CreatedBy,string ID)
        {
            string sResult = string.Empty;
            dlobj = new DL_ApplicationSetting();
            try
            {
                DataTable dt = dlobj.UpdateAppSetting(sMachineTestCount, sReworkInOutMaxLimit, sReworkOutMaxTime,
                                sReworkInMinTime, _CreatedBy,ID);
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
