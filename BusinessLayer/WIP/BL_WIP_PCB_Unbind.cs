using DataLayer.WIP;
using System.Data;

namespace BusinessLayer.WIP
{
    public class BL_WIP_PCB_Unbind
    {
        DL_WIP_PCB_Unbind dlobj;

        public DataTable VALIDATEPCBID(string spcbid, string sSiteCode)
        {
            dlobj = new DL_WIP_PCB_Unbind();
            return dlobj.VALIDATEPCBID(spcbid, sSiteCode);
        }

        public DataTable VALIDATESUBPCBID(string spcbid, string sSUBpcbid, string sSiteCode)
        {
            dlobj = new DL_WIP_PCB_Unbind();
            return dlobj.VALIDATESUBPCBID(spcbid, sSUBpcbid, sSiteCode);
        }

        public DataTable UNBINDSUBPCBID(string spcbid, string sSUBpcbid, string sSiteCode, string sLineCode, string sUserID)
        {
            dlobj = new DL_WIP_PCB_Unbind();
            return dlobj.UNBINDSUBPCBID(spcbid, sSUBpcbid, sSiteCode, sLineCode, sUserID);
        }




    }
}
