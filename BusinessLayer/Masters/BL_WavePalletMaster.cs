using Common;
using DataLayer;
using System;
using System.Data;
namespace BusinessLayer
{

    public class BL_WavePalletMaster
    {
        DL_WavePalletMaster dlobj = null;
        public DataTable GetWavePallet()
        {
            DataTable dtWavePallet = new DataTable();
            dlobj = new DL_WavePalletMaster();
            try
            {
                dtWavePallet = dlobj.GetWavePallet();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtWavePallet;
        }

        public DataTable GetWavePalletByID(string _SN)
        {
            DataTable dt = new DataTable();
            dlobj = new DL_WavePalletMaster();
            try
            {
                dt = dlobj.GetWavePalletByID(_SN);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }
        public string SaveWavePallet(string _WavePalletID, string _WavePalletName, string _WavePalletDesc, string _CreatedBy)
        {
            string sResult = string.Empty;
            dlobj = new DL_WavePalletMaster();
            try
            {
                DataTable dt = dlobj.SaveWavePallet(_WavePalletID, _WavePalletName, _WavePalletDesc, _CreatedBy, "SAVEWAVEPALLETDETAILS");
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

        public string DeleteWavePalletid(string sWavePalletiD)
        {
            string sResult = string.Empty;
            dlobj = new DL_WavePalletMaster();
            try
            {
                DataTable dt = dlobj.SaveWavePallet(sWavePalletiD, "", "", "", "DELETEWAVEPALLET");
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
        public string UpdateWavePallet(string _WavePalletName, string _WavePalletDescription, string WavePalletiD)
        {
            string sResult = string.Empty;
            dlobj = new DL_WavePalletMaster();
            try
            {
                DataTable dt = dlobj.SaveWavePallet(WavePalletiD, _WavePalletName, _WavePalletDescription, "", "UPDATEWAVEPALLET");
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
