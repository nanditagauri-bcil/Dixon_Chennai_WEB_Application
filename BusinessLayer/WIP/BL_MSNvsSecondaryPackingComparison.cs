using DataLayer.WIP;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_MSNvsSecondaryPackingComparison
    {
        DL_MSNvsSecondaryPackingComparison dlobj;

        public DataTable BindFGItemCode(string sSiteCode)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.BindFGItemCode(sSiteCode);
        }
        public DataTable BindPO(string sFGITEMCODE, string sSiteCode)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.BindPO(sFGITEMCODE, sSiteCode);
        }
        public DataTable BindInvoiceNo(string sPONumber, string sFGITEMCODE, string sSiteCode)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.BindInvoiceNo(sPONumber, sFGITEMCODE, sSiteCode);
        }
        public DataTable BindSecBOXID(string sInvoiceNo, string sPONumber, string sFGITEMCODE, string sSiteCode)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.BindSecBOXID(sInvoiceNo, sPONumber, sFGITEMCODE, sSiteCode);
        }
        public DataSet BindMsnBOXID(string sSecBoxID, string sInvoiceNo, string sPONumber, string sFGITEMCODE,
                                                                string sSiteCode, string sLineCode, string sUserID)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.BindMsnBOXID(sSecBoxID, sInvoiceNo, sPONumber, sFGITEMCODE, sSiteCode, sLineCode, sUserID);
        }
        public DataTable ValidateScanMSNBarcode(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
             string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.ValidateScanMSNBarcode(sModelCode, sSecBoxID, sInvoiceNo, sPONumber, sFGItemCode,
                sSiteCode, sMSNBarcode, sLineCode, sUserID);
        }
        public DataTable ValidateScanRSN(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
             string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID, string sRSN)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.ValidateScanRSN(sModelCode, sSecBoxID, sInvoiceNo, sPONumber, sFGItemCode,
                sSiteCode, sMSNBarcode, sLineCode, sUserID, sRSN);
        }

        public DataTable VERIFIEDSAVED(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
             string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID, DataTable _dt, string sRemarks)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.VERIFIEDSAVED(sModelCode, sSecBoxID, sInvoiceNo, sPONumber, sFGItemCode,
                sSiteCode, sMSNBarcode, sLineCode, sUserID, _dt, sRemarks);
        }

        public DataTable REJECTSAVED(string sModelCode, string sSecBoxID, string sInvoiceNo, string sPONumber,
            string sFGItemCode, string sSiteCode, string sMSNBarcode, string sLineCode, string sUserID, DataTable _dt, string sRemarks)
        {
            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.REJECTSAVED(sModelCode, sSecBoxID, sInvoiceNo, sPONumber, sFGItemCode,
                                        sSiteCode, sMSNBarcode, sLineCode, sUserID, _dt, sRemarks);
        }

        public DataTable GetData(string sModelCode, string sSiteCode, string sMSNBarcode
             , string sLineCode, string sFGItemCode, string sSecBoxID)
        {

            dlobj = new DL_MSNvsSecondaryPackingComparison();
            return dlobj.GetData(sModelCode, sSiteCode, sMSNBarcode, sLineCode, sFGItemCode
                    , sSecBoxID);


        }
    }
}
