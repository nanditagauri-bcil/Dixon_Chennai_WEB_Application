using BusinessLayer.WIP;
using Common;
using System;
using System.Data;

namespace DIXON.INE.WIP
{
    public partial class WIPIMEIandEIDUnbind : System.Web.UI.Page
    {
        BL_WIP_IMEIandEID_Unbind blobj = new BL_WIP_IMEIandEID_Unbind();
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
                    _strRights = CommonHelper.GetRights("WIP IMEIandEID Unbind", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    dvchipid.Visible = false;
                    dvimeieid.Visible = true;
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
                txtpcbid.Text = string.Empty;
                txtmacid.Text = string.Empty;
                txtimei.Text = string.Empty;
                txteid.Text = string.Empty;
                txtchipid.Text = string.Empty;
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
                if (string.IsNullOrWhiteSpace(txtpcbid.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan PCBID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbid.Text = string.Empty;
                    txtmacid.Text = string.Empty;
                    txtimei.Text = string.Empty;
                    txteid.Text = string.Empty;
                    txtpcbid.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtmacid.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan MACID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtmacid.Text = string.Empty;
                    txtimei.Text = string.Empty;
                    txteid.Text = string.Empty;
                    txtmacid.Focus();
                    return;
                }
                if (rdIsImeiEid.SelectedValue == "CHIPID")
                {
                    if (string.IsNullOrWhiteSpace(txtchipid.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please Scan CHIPID", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtchipid.Text = string.Empty;
                        txtchipid.Focus();
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtimei.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please Scan IMEI", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtimei.Text = string.Empty;
                        txteid.Text = string.Empty;
                        txtimei.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txteid.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please Scan EID", msgerror, CommonHelper.MessageType.Error.ToString());
                        txteid.Text = string.Empty;
                        txteid.Focus();
                        return;
                    }
                } 
                blobj = new BL_WIP_IMEIandEID_Unbind();
                DataTable dt = new DataTable();
                dt = blobj.UNBINDIDs(txtpcbid.Text.Trim(),txtmacid.Text.Trim(),txtimei.Text.Trim(),txteid.Text.Trim(),
                    Session["SiteCode"].ToString(), Session["UserID"].ToString(),txtchipid.Text.Trim(),
                    rdIsImeiEid.SelectedValue.ToString().Trim());
                if (dt.Rows[0][0].ToString().StartsWith("SUCCESS"))
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtpcbid.Focus();
                    txtpcbid.Text = string.Empty;
                    txtmacid.Text = string.Empty;
                    txtimei.Text = string.Empty;
                    txteid.Text = string.Empty;
                    txtchipid.Text = string.Empty;
                    return;
                }
                else
                {
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbid.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void rdIsImeiEid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (rdIsImeiEid.SelectedValue == "CHIPID")
                {
                    dvchipid.Visible = true;
                    dvimeieid.Visible = false;
                }
                else
                {
                    dvchipid.Visible = false;
                    dvimeieid.Visible = true;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
    }
}