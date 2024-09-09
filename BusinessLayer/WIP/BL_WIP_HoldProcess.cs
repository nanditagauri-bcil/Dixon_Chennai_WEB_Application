using Common;
using DataLayer.WIP;
using System;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIP_HoldProcess
    {
        DL_WIP_Hold_Process dlobj = new DL_WIP_Hold_Process();
        public DataTable BindFGItemCode(string sSiteCode)
        {
            DataTable dt = new DataTable();
            try
            {
                dlobj = new DL_WIP_Hold_Process();
                dt = dlobj.BindFGItemCode(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }

        public DataTable SavePartHoldData(string sPCBBarcode, string sType, string sFGItemCode
            , string sSiteCode, string sUserID, string sLineCode, string sReason
            )
        {
            DataTable dt = new DataTable();
            try
            {
                dlobj = new DL_WIP_Hold_Process();
                dt = dlobj.PartHoldProcess(sPCBBarcode, sType, sFGItemCode
                    , sSiteCode, sUserID, sLineCode, sReason
                    );
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dt;
        }
    }
}
