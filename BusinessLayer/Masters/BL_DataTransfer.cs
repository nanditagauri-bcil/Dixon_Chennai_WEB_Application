using Common;
using DataLayer.Masters;
using System;
using System.Data;

namespace BusinessLayer.Masters
{
    public class BL_DataTransfer
    {
        DL_DataTransfer dlboj = null;

        public DataTable BindWorkOrderNo(string siteCode)
        {
            DataTable dtBins = new DataTable();
            dlboj = new DL_DataTransfer();
            try
            {
                dtBins = dlboj.BindWorkOrderNo(siteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtBins;
        }

        public DataTable DataTranfer(string sType, string issueSlipNo, string workOrderNo, string sUserID)
        {
            DataTable dtBins = new DataTable();
            dlboj = new DL_DataTransfer();
            try
            {
                dtBins = dlboj.DataTransfer(sType, issueSlipNo, workOrderNo, sUserID);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtBins;
        }
    }
}
