using DataLayer.WIP;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_MSNvsGBComparison
    {
        DL_MSNvsGBComparison dlobj;

        public DataTable BindFGItemCode(string sSiteCode)
        {
            dlobj = new DL_MSNvsGBComparison();
            return dlobj.BindFGItemCode(sSiteCode);
        }
         

        public DataSet ValidateMSN(string sMSNBarcode,string FGItemCode, string sSiteCode, string sLineCode, string sUserID)
        { 
            dlobj = new DL_MSNvsGBComparison();
            return dlobj.VaildateBarcode(sMSNBarcode,FGItemCode, sSiteCode, sLineCode, sUserID); 
        }
         
        public DataTable ValidateScanGBBarcode(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sGBBarcode, string sUserID)
        { 
            dlobj = new DL_MSNvsGBComparison();
            return dlobj.ValidateScanGBBarcode(sModelCode, sSiteCode, sMSNBarcode, sLineCode, sFGItemCode
                    , sGBBarcode,sUserID); 
        }

        public DataTable VERIFIEDSAVED(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sUserID, DataTable _dt)
        {
            dlobj = new DL_MSNvsGBComparison();
            return dlobj.VERIFIEDSAVED(sModelCode, sSiteCode, sMSNBarcode, sLineCode, sFGItemCode, sUserID, _dt);
        }

        public DataTable REJECTSAVED(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sUserID, DataTable _dt, string sRemarks)
        {
            dlobj = new DL_MSNvsGBComparison();
            return dlobj.REJECTSAVED(sModelCode, sSiteCode, sMSNBarcode, sLineCode, sFGItemCode, sUserID, _dt, sRemarks);
        }

        public DataTable GetData(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sGBBarcode)
        {

            dlobj = new DL_MSNvsGBComparison();
            return dlobj.GetData(sModelCode, sSiteCode, sMSNBarcode, sLineCode, sFGItemCode
                    , sGBBarcode);


        }
    }
}
