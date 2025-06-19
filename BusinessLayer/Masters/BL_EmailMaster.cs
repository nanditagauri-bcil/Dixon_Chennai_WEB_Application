using Common;
using DataLayer;
using System;
using System.Data;

namespace BusinessLayer
{
    public class BL_EmailMaster
    {
        DL_EmailMaster dlobj = null;
        public DataTable GetEmail()
        {
            DataTable dtLines = new DataTable();
            dlobj = new DL_EmailMaster();
            try
            {
                dtLines = dlobj.GetEmail();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLines;
        }

        public DataTable GetSeletedData(string _SN)
        {
            DataTable dt = new DataTable();
            dlobj = new DL_EmailMaster();
            try
            {
                dt = dlobj.GetEmailByID(_SN);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }
        public string SaveEmailDetails(string sEmailSub, string sEmailBody, string sSiteCode, string sFromEmail, string sToEmail,
                                       string sCCEmail, string sLineCode, string sBCCEmail, string sRemarks, string sUserID, string sType, string ID)
        {
            string sResult = string.Empty;
            dlobj = new DL_EmailMaster();
            try
            {
                DataTable dt = dlobj.SaveEmailDetails(sEmailSub, sEmailBody, sSiteCode, sFromEmail, sToEmail, sCCEmail, sLineCode,
                                                      sBCCEmail, sRemarks, sUserID, sType, ID);
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

        public string DeleteEmail(string sID)
        {
            string sResult = string.Empty;
            dlobj = new DL_EmailMaster();
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
    }
}
