using DataLayer.MES.PRINTING;
using System.Data;

namespace BusinessLayer.MES.PRINTING
{
    public class BL_Binding_Wall_Mount_Kit
    {
        DL_Binding_Wall_Mount_Kit dlobj;
        DataTable dt;

        public DataTable BindFGItemCode(string sSiteCode)
        {
            dlobj = new DL_Binding_Wall_Mount_Kit();
            return dlobj.BindFGItemCode(sSiteCode);
        }
        public DataTable DisplayedData(string sFgItemCode, string sSiteCode)
        {
            dlobj = new DL_Binding_Wall_Mount_Kit();
            return dlobj.DisplayedData(sFgItemCode, sSiteCode);
        }
        public DataTable Scan_Barcode(string sBarcode, string sFgItemCode, string sSiteCode, string sUserID, string sLineCode)
        {
            dlobj = new DL_Binding_Wall_Mount_Kit();
            return dlobj.Scan_Barcode(sBarcode, sFgItemCode, sSiteCode, sUserID, sLineCode);
        }
        public DataTable GetCaptureWeight(string sBarcode, string sFgItemCode, string sSiteCode, string sUserID, string sLineCode)
        {
            dlobj = new DL_Binding_Wall_Mount_Kit();
            return dlobj.GetCaptureWeight(sBarcode, sFgItemCode, sSiteCode, sUserID, sLineCode);
        }
        public DataTable SaveDATA(string sBarcode, string sFgItemCode, string sSiteCode, string sUserID, string sStageCode,
            string sLineCode, string WMWT)
        {
            dlobj = new DL_Binding_Wall_Mount_Kit();
            return dlobj.SaveDATA(sBarcode, sFgItemCode, sSiteCode, sUserID, sStageCode, sLineCode, WMWT);
        }

    }
}
