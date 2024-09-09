using Common;
using DataLayer.WIP;
using System;
using System.Data;
namespace BusinessLayer.WIP
{
    public class BL_RouteUpdate
    {
        DL_RouteUpdate dlboj = null;

        public DataTable BindFGITEMCOE()
        {
            DataTable dtFgItemCode = new DataTable();
            dlboj = new DL_RouteUpdate();
            try
            {
                dtFgItemCode = dlboj.BindFGItemCode();
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFgItemCode;
        }

        public DataTable GetRouteName(string sFGItemCode)
        {
            DataTable dtFgItemCode = new DataTable();
            dlboj = new DL_RouteUpdate();
            try
            {
                dtFgItemCode = dlboj.GetRouteName(sFGItemCode);
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return dtFgItemCode;
        }

        public string SaveRouteUpdate(string sFGItemCode, string sRoutename, string sUserid)
        {
            string sResult = string.Empty;
            dlboj = new DL_RouteUpdate();
            try
            {
                DataTable dt = new DataTable();
                dt = dlboj.SaveRouteUpdate(sFGItemCode, sRoutename, sUserid);
                if (dt.Rows.Count > 0)
                {
                    sResult = dt.Rows[0][0].ToString();
                }
                else
                {
                    sResult = "N~Data updation failed";
                }
            }
            catch (Exception ex)
            {
                PCommon.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
            return sResult;
        }
    }
}

