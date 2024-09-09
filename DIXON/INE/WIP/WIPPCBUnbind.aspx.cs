using BusinessLayer.WIP;
using Common;
using System;
using System.Data;

namespace DIXON.INE.WIP
{
    public partial class WIPPCBUnbind : System.Web.UI.Page
    {
        BL_WIP_PCB_Unbind blobj = new BL_WIP_PCB_Unbind();
        string Message = string.Empty;
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
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");
                string sHeaderName = string.Empty;
                if (Session["usertype"].ToString() != "ADMIN")
                {
                    string _strRights = string.Empty;
                    _strRights = CommonHelper.GetRights("WIP PCB Unbind", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                txtpcb.Text = string.Empty;
                txtsubpcbid.Text = string.Empty;

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void txtpcb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtpcb.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan PCBID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcb.Text = string.Empty;
                    txtpcb.Focus();
                    return;
                }
                blobj = new BL_WIP_PCB_Unbind();
                DataTable dt = new DataTable();
                dt = blobj.VALIDATEPCBID(txtpcb.Text.Trim(), Session["SiteCode"].ToString());
                if (dt.Rows[0][0].ToString().StartsWith("SUCCESS"))
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtsubpcbid.Focus();
                    return;
                }
                else
                {
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcb.Text = string.Empty;
                    txtpcb.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void txtsubpcbid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtpcb.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan PCBID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcb.Text = string.Empty;
                    txtsubpcbid.Text = string.Empty;
                    txtpcb.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtsubpcbid.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan SUB PCBID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtsubpcbid.Text = string.Empty;
                    txtsubpcbid.Focus();
                    return;
                }
                blobj = new BL_WIP_PCB_Unbind();
                DataTable dt = new DataTable();
                dt = blobj.VALIDATESUBPCBID(txtpcb.Text.Trim(), txtsubpcbid.Text.Trim(), Session["SiteCode"].ToString());
                if (dt.Rows[0][0].ToString().StartsWith("SUCCESS"))
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    return;
                }
                else
                {
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    txtsubpcbid.Text = string.Empty;
                    txtsubpcbid.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnUnbind_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtpcb.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan PCBID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcb.Text = string.Empty;
                    txtsubpcbid.Text = string.Empty;
                    txtpcb.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtsubpcbid.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan SUB PCBID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtsubpcbid.Text = string.Empty;
                    txtsubpcbid.Focus();
                    return;
                }
                blobj = new BL_WIP_PCB_Unbind();
                DataTable dt = new DataTable();
                dt = blobj.UNBINDSUBPCBID(txtpcb.Text.Trim(), txtsubpcbid.Text.Trim(),
                    Session["SiteCode"].ToString(), Session["UserID"].ToString(), Session["LineCode"].ToString());
                if (dt.Rows[0][0].ToString().StartsWith("SUCCESS"))
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtpcb.Focus();
                    txtsubpcbid.Text = string.Empty;
                    txtpcb.Text = string.Empty;
                    return;
                }
                else
                {
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcb.Text = string.Empty;
                    txtsubpcbid.Text = string.Empty;
                    txtpcb.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}