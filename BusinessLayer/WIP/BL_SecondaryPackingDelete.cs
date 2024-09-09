using Common;
using DataLayer.WIP;
using System;
using System.Data;
namespace BusinessLayer.WIP
{
    public class BL_SecondaryPackingDelete
    {
        DL_SecondaryPackingDelete dlboj = null;

        public DataTable BindFGITEMCOE()
        {
            DataTable dtFgItemCode = new DataTable();
            dlboj = new DL_SecondaryPackingDelete();
            try
            {
                dtFgItemCode = dlboj.BindFGItemCode();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFgItemCode;
        }

        public string DeleteSecondaryPacking(string sFGItemCode, string sInvoiceNo, string sUserid)
        {
            string sResult = string.Empty;
            dlboj = new DL_SecondaryPackingDelete();
            try
            {
                DataTable dt = new DataTable();
                dt = dlboj.DeleteSecondaryPacking(sFGItemCode, sInvoiceNo, sUserid);
                if (dt.Rows.Count > 0)
                {
                    sResult = dt.Rows[0][0].ToString();
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

