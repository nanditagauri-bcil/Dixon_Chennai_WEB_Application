using DataLayer.WIP;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIPAutoSampleClear
    {
        DL_WIPAutoSampleClear dlobj;

        public DataTable ValidateMachine(string sMachineID, string sSiteCode, string sLineCode)
        {
            dlobj = new DL_WIPAutoSampleClear();
            return dlobj.ValidateMachine(sMachineID, sSiteCode, sLineCode);
        }
        public DataTable BindFGItemCode(string sMachineID, string sSiteCode, string sLineCode)
        {
            dlobj = new DL_WIPAutoSampleClear();
            return dlobj.BindFGItemCode(sMachineID, sSiteCode, sLineCode);
        }


        public DataSet ValidatePCB(string sPartBarcode, string sMachineID, string FGItemCode
             , string sSiteCode, string sLineCode)
        {

            dlobj = new DL_WIPAutoSampleClear();
            return dlobj.VaildateBarcode(sPartBarcode, sMachineID, FGItemCode
                    , sSiteCode, sLineCode);


        }

        public DataSet SaveResult(string sPartBarcode, string sMachineID, string sFGItemCode, string sSiteCode,
              string sLineCode, string sUserID, string sResultType)
        {
            dlobj = new DL_WIPAutoSampleClear();
            return dlobj.SaveResult(sPartBarcode, sMachineID, sFGItemCode, sSiteCode,
                sLineCode, sUserID, sResultType);
        }

    }
}
