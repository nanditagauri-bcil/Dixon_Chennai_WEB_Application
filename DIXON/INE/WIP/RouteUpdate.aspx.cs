using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Web.UI;

namespace DIXON.INE.WIP
{
    public partial class RouteUpdate : System.Web.UI.Page
    {
        BL_RouteUpdate blobj = new BL_RouteUpdate();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Signin/v1/Login.aspx?Session=Null");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usertype"].ToString() != "ADMIN")
                {
                    string _strRights = CommonHelper.GetRights("WIP Route Update", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");
                if (!Page.IsPostBack)
                {
                    GET_FGITEMCODE();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        public void GET_FGITEMCODE()
        {

            try
            {
                blobj = new BL_RouteUpdate();
                string sResult = string.Empty;
                DataTable dt = blobj.BindFGITEMCOE();
                if (dt.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(ddlFgItemCode, dt, true);
                    ddlFgItemCode.SelectedIndex = 0;
                    ddlFgItemCode.Focus();
                    return;
                }
                else
                {
                    CommonHelper.ShowMessage("No Fg item code found", msgerror, CommonHelper.MessageType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }

        protected void ddlFgItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            try
            {
                ddlRoute.DataSource = null;
                ddlRoute.Items.Clear();
                if (ddlFgItemCode.SelectedIndex > 0)
                {
                    blobj = new BL_RouteUpdate();
                    string sResult = string.Empty;
                    DataTable dt = blobj.GetRouteName(ddlFgItemCode.SelectedValue.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        clsCommon.FillComboBox(ddlRoute, dt, true);
                        return;
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No route details found", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        private void Reset()
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                GET_FGITEMCODE();
                ddlFgItemCode.Focus();
                ddlFgItemCode.SelectedIndex = 0;
                ddlRoute.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            Reset();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            try
            {
                if (ddlFgItemCode.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select FG Item Code", msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlFgItemCode.Focus();
                    return;
                }
                if (ddlRoute.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select route name", msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlRoute.Focus();
                    return;
                }
                string sResult = blobj.SaveRouteUpdate(ddlFgItemCode.SelectedValue.Trim(),
                    ddlRoute.SelectedValue.Trim(), Session["UserID"].ToString());

                if (sResult.ToUpper().StartsWith("SUCCESS"))
                {
                    CommonHelper.ShowMessage(sResult, msgsuccess, CommonHelper.MessageType.Success.ToString());
                    ddlFgItemCode.Focus();
                    GET_FGITEMCODE();
                    ddlFgItemCode.SelectedIndex = 0;
                    ddlRoute.SelectedIndex = 0;
                    return;
                }
                if (sResult.StartsWith("ERROR~"))
                {
                    CommonHelper.ShowMessage(sResult, msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlFgItemCode.Focus();
                    GET_FGITEMCODE();
                    ddlFgItemCode.SelectedIndex = 0;
                    ddlRoute.SelectedIndex = 0;
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return;
            }

        }

    }
}