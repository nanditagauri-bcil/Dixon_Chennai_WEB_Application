using DataLayer.WIP;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIPCastingLabelMapping
    {
        DL_WIPCastingLabelMapping dlobj;

        public DataTable ValidateMachine(string sMachineID, string sSiteCode, string sLineCode)
        {
            dlobj = new DL_WIPCastingLabelMapping();
            return dlobj.ValidateMachine(sMachineID, sSiteCode, sLineCode);
        }
        public DataTable BindFGItemCode(string sMachineID, string sSiteCode, string sLineCode)
        {
            dlobj = new DL_WIPCastingLabelMapping();
            return dlobj.BindFGItemCode(sMachineID, sSiteCode, sLineCode);
        }

        public DataTable BindSubPCBID(string sFGitemCode, string sSiteCode)
        {
            dlobj = new DL_WIPCastingLabelMapping();
            return dlobj.BindSubPCBID(sFGitemCode, sSiteCode);
        }

        public DataSet ValidatePCB(string sPartBarcode, string sMachineID, string FGItemCode
             , string sSiteCode, string sLineCode)
        {

            dlobj = new DL_WIPCastingLabelMapping();
            return dlobj.VaildateBarcode(sPartBarcode, sMachineID, FGItemCode
                    , sSiteCode, sLineCode);
        }

        public DataTable ValidateScanSubPCBID(string sMachineID, string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sSubPCBID)
        {

            dlobj = new DL_WIPCastingLabelMapping();
            return dlobj.ValidateScanSubPCBID(sMachineID, sModelCode, sSiteCode, sPartBarcode, sLineCode, sFGItemCode
                    , sSubPCBID);


        }

        public DataTable SaveScanSubPCBID(string sMachineID, string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sUserID, DataTable _dtSubPCBIDs)
        {

            dlobj = new DL_WIPCastingLabelMapping();
            return dlobj.SaveScanSubPCBID(sMachineID, sModelCode, sSiteCode, sPartBarcode, sLineCode, sFGItemCode
                    , sUserID, _dtSubPCBIDs);


        }

        public DataTable GetData(string sModelCode, string sSiteCode, string sPartBarcode
             , string sLineCode, string sFGItemCode, string sSubPCBID)
        {

            dlobj = new DL_WIPCastingLabelMapping();
            return dlobj.GetData(sModelCode, sSiteCode, sPartBarcode, sLineCode, sFGItemCode
                    , sSubPCBID);


        }
    }
}
