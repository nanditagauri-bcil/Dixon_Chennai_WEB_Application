using DataLayer.WIP;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIPWavePalletMapping
    {
        DL_WIPWavePalletMapping dlobj;

        public DataTable ValidateMachine(string sMachineID, string sSiteCode, string sLineCode)
        {
            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.ValidateMachine(sMachineID, sSiteCode, sLineCode);
        }
        public DataTable BindFGItemCode(string sMachineID, string sSiteCode, string sLineCode)
        {
            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.BindFGItemCode(sMachineID, sSiteCode, sLineCode);
        }

        public DataTable BindSubPCBID(string sFGitemCode, string sSiteCode)
        {
            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.BindSubPCBID(sFGitemCode, sSiteCode);
        }

        public DataSet ValidatePCB(string sPartBarcode, string sMachineID, string FGItemCode
             , string sSiteCode, string sLineCode)
        {

            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.VaildateBarcode(sPartBarcode, sMachineID, FGItemCode
                    , sSiteCode, sLineCode);


        }

        public DataTable ValidateWavePalletID(string sMachineID, string sPartBarcode, string sFGItemCode,
            string sWavePalletID, string sSiteCode, string sUserID, string sLineCode)
        {

            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.ValidateWavePalletID(sMachineID, sPartBarcode, sFGItemCode, sWavePalletID, sSiteCode,
                sUserID, sLineCode);


        }
        public DataTable ValidateScanSubPCBID(string sMachineID, string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sSubPCBID)
        {

            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.ValidateScanSubPCBID(sMachineID, sModelCode, sSiteCode, sPartBarcode, sLineCode, sFGItemCode
                    , sSubPCBID);


        }

        public DataTable SaveScanSubPCBID(string sMachineID, string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sUserID, string sWavePalletID, DataTable _dtSubPCBIDs)
        {

            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.SaveScanSubPCBID(sMachineID, sModelCode, sSiteCode, sPartBarcode, sLineCode, sFGItemCode
                    , sUserID, sWavePalletID, _dtSubPCBIDs);


        }

        public DataTable GetData(string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sSubPCBID)
        {

            dlobj = new DL_WIPWavePalletMapping();
            return dlobj.GetData(sModelCode, sSiteCode, sPartBarcode, sLineCode, sFGItemCode
                    , sSubPCBID);


        }
    }
}
