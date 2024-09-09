using Common;
using System;
using System.Data;

namespace DataLayer
{
    public class DL_EmailMaster
    {
        DBManager oDbm;
        public DL_EmailMaster()
        {
            DCommon c = new DCommon();
            oDbm = c.SqlDBProvider();
        }
        public DataTable GetEmail()
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(2);
                oDbm.AddParameters(0, "@TYPE", "GETEMAILDETAILS");
                oDbm.AddParameters(1, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_EMAILMASTER").Tables[0];
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
        public DataTable GetEmailByID(string sID)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(3);
                oDbm.AddParameters(0, "@TYPE", "GETEMAILDETAILSBYID");
                oDbm.AddParameters(1, "@ID", sID);
                oDbm.AddParameters(2, "@SITECODE", PCommon.sSiteCode);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_EMAILMASTER").Tables[0];
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

        public DataTable SaveEmailDetails(string sEmailSub, string sEmailBody, string sSiteCode, string sFromEmail, string sToEmail,
                                       string sCCEmail, string sLineCode, string sBCCEmail, string sRemarks, string sUserID,string sType, string ID)
        {
            DataTable dt = new DataTable();
            try
            {
                oDbm.CreateParameters(12);
                oDbm.AddParameters(0, "@TYPE", sType);
                oDbm.AddParameters(1, "@EMAILSUB", sEmailSub);
                oDbm.AddParameters(2, "@EMAILBODY", sEmailBody);
                oDbm.AddParameters(3, "@SITECODE", sSiteCode);
                oDbm.AddParameters(4, "@FROMEMAIL", sFromEmail);
                oDbm.AddParameters(5, "@TOEMAIL", sToEmail);
                oDbm.AddParameters(6, "@CCEMAIL", sCCEmail);
                oDbm.AddParameters(7, "@LINECODE", sLineCode);
                oDbm.AddParameters(8, "@BCCEMAIL", sBCCEmail);
                oDbm.AddParameters(9, "@REMARKS", sRemarks);
                oDbm.AddParameters(10, "@USERID", sUserID);
                oDbm.AddParameters(11, "@ID", ID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_EMAILMASTER").Tables[0];
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
                oDbm.AddParameters(0, "@TYPE", "DELETEEMAILDETAILS");
                oDbm.AddParameters(1, "@ID", sID);
                oDbm.Open();
                dt = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_EMAILMASTER").Tables[0];
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
