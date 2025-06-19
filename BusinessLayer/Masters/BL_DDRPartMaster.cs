using Common;
using DataLayer;
using System;
using System.Data;

namespace BusinessLayer
{
    public class BL_DDRPartMaster
    {
        DL_DDRPartMaster dlobj = null;

        public DataTable BindFGItemCode(string sSiteCode)
        {
            dlobj = new DL_DDRPartMaster();
            return dlobj.BindFGItemCode(sSiteCode);
        }
        public DataTable GetDDR()
        {
            DataTable dtLines = new DataTable();
            dlobj = new DL_DDRPartMaster();
            try
            {
                dtLines = dlobj.GetDDR();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtLines;
        }

        public DataSet GetSeletedData(string sFGITEMCODE, string _SN)
        {
            DataSet ds = new DataSet();
            dlobj = new DL_DDRPartMaster();
            try
            {
                ds = dlobj.GetDDRByID(sFGITEMCODE, _SN);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return ds;
        }
        public string SaveDDR(string sFGITEMCODE, string sBOMPARTCODE, string sDDRPARTCODE, string sDESC, string sUSERID,
                              string sSITECODE, string sLINECODE, string sID)
        {
            string sResult = string.Empty;
            dlobj = new DL_DDRPartMaster();
            try
            {
                DataTable dt = dlobj.SaveDDR(sFGITEMCODE, sBOMPARTCODE, sDDRPARTCODE, sDESC, sUSERID, sSITECODE, sLINECODE,
                                               "SAVEDDRDETAILS", sID);
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

        public string DeleteDDR(string sID)
        {
            string sResult = string.Empty;
            dlobj = new DL_DDRPartMaster();
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
        public string UpdateDDR(string sFGITEMCODE, string sBOMPARTCODE, string sDDRPARTCODE, string sDESC, string sUSERID,
                              string sSITECODE, string sLINECODE, string sID)
        {
            string sResult = string.Empty;
            dlobj = new DL_DDRPartMaster();
            try
            {
                DataTable dt = dlobj.SaveDDR(sFGITEMCODE, sBOMPARTCODE, sDDRPARTCODE, sDESC, sUSERID, sSITECODE, sLINECODE,
                                               "UPDATEDDRDETAILS", sID);
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
