using Common;
using DataLayer.WIP;
using System;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIP_RSN_Update
    {
        DL_WIP_RSN_Update dlobj;

        public DataTable blRSN(string sRSn, string sSiteCode)
        {
            dlobj = new DL_WIP_RSN_Update();
            return dlobj.dlRSN(sRSn, sSiteCode);
        }

        public string SaveUpdateRsnMonth(string sRSN, string smacID, string sCurrentRsnMonth,
            string _sNewRsn, string _sCreBy, string sSiteCode, string sLineCode, string swifikey,
            string sSSID)
        {
            dlobj = new DL_WIP_RSN_Update();
            string sResult = string.Empty;
            try
            {
                DataTable dt = dlobj.SaveUpdateRsnMonth(sRSN, smacID, sCurrentRsnMonth, _sNewRsn,
           _sCreBy, sSiteCode, sLineCode, swifikey, sSSID);
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
