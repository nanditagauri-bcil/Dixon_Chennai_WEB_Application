using DataLayer.WIP;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIP_IMEIandEID_Unbind
    {
        DL_WIP_IMEIandEID_Unbind dlobj;

        public DataTable UNBINDIDs(string sPcbid, string sMacid, string sImeiid, string sEidid,
                            string sSiteCode, string sUserID, string sChipid, string sRadiolistType)
        {
            dlobj = new DL_WIP_IMEIandEID_Unbind();
            return dlobj.UNBINDIDs(sPcbid, sMacid, sImeiid, sEidid, sSiteCode, sUserID, sChipid, sRadiolistType);
        }
    }
}
