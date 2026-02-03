using Common;
using DataLayer;
using System;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_SplitReelQuality
    {
        DL_SplitReelQuality dlobj = null;
        public DataTable BindINELPartNo(string sSiteCode)
        {
            DataTable dtINELPartNo = new DataTable();
            dlobj = new DL_SplitReelQuality();
            try
            {
                dtINELPartNo = dlobj.BINDINEL_PARTNO(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtINELPartNo;
        }

        public DataTable BindReelBarcode(string sItemCode, string sSiteCode)
        {
            DataTable dtReelBarcode = new DataTable();
            dlobj = new DL_SplitReelQuality();
            try
            {
                dtReelBarcode = dlobj.BindReelBarcode(sItemCode, sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReelBarcode;
        }

        public DataTable SaveQuality(string sPartCode, string sPartBarcode, int qualityType, string qualityBy, string sSiteCode, string sLineCode)
        {
            DataTable dt = new DataTable();
            dlobj = new DL_SplitReelQuality();
            try
            {
                dt = dlobj.SaveQuality(sPartCode, sPartBarcode, qualityType, qualityBy, sSiteCode, sLineCode);
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
