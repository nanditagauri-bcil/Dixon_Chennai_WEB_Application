using Common;
using DataLayer;
using System;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_SplitReelQuality
    {
        DL_SplitReelQuality dlobj = null;

        public DataTable BindReelBarcode(string sSiteCode)
        {
            DataTable dtReelBarcode = new DataTable();
            dlobj = new DL_SplitReelQuality();
            try
            {
                dtReelBarcode = dlobj.BindReelBarcode(sSiteCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtReelBarcode;
        }

        public DataTable SaveQuality(string sPartBarcode, int qualityType, string qualityBy, string sSiteCode, string sLineCode)
        {
            DataTable dt = new DataTable();
            dlobj = new DL_SplitReelQuality();
            try
            {
                dt = dlobj.SaveQuality(sPartBarcode, qualityType, qualityBy, sSiteCode, sLineCode);
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
