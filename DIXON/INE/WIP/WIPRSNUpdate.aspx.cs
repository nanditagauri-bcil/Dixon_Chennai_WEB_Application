using BusinessLayer.WIP;
using Common;
using System;
using System.Data;

namespace DIXON.INE.WIP
{
    public partial class WIPRSNUpdate : System.Web.UI.Page
    {
        BL_WIP_RSN_Update blobj = new BL_WIP_RSN_Update();
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
                    _strRights = CommonHelper.GetRights("RSN Update", (DataTable)Session["USER_RIGHTS"]);
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



        protected void txtrsn_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtrsn.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan RSN", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtrsn.Text = string.Empty;
                    txtrsn.Focus();
                    return;
                }
                blobj = new BL_WIP_RSN_Update();
                DataTable dt = new DataTable();
                dt = blobj.blRSN(txtrsn.Text.Trim(), Session["SiteCode"].ToString());
                if (dt.Rows[0][0].ToString().StartsWith("SUCCESS"))
                {
                    txtmacid.Text = dt.Rows[0][0].ToString().Split('~')[1];
                    txtcuurentRSNmonth.Text = dt.Rows[0][0].ToString().Split('~')[2];
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage("Valid RSN", msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtnewrsn.Focus();
                    return;
                }
                else
                {
                    CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    txtrsn.Text = string.Empty;
                    txtrsn.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                txtrsn.Text = string.Empty;
                txtmacid.Text = string.Empty;
                txtcuurentRSNmonth.Text = string.Empty;
                txtnewrsn.Text = string.Empty;
                txtwifikey.Text = string.Empty;
                txtssid.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            try
            {
                if (string.IsNullOrWhiteSpace(txtrsn.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter RSN", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtrsn.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtmacid.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Mac ID not found", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtmacid.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtcuurentRSNmonth.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Current RSN Month not found", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtcuurentRSNmonth.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtnewrsn.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter New RSN", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtnewrsn.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtwifikey.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter Wifi Key", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtrsn.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtssid.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter SSID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtrsn.Focus();
                    return;
                }
                blobj = new BL_WIP_RSN_Update();
                string sResult = blobj.SaveUpdateRsnMonth(txtrsn.Text.Trim(),
                            txtmacid.Text.Trim(), txtcuurentRSNmonth.Text.Trim(), txtnewrsn.Text.Trim(),
                            Session["UserID"].ToString(), Session["SiteCode"].ToString(),
                            Session["LINECODE"].ToString(), txtwifikey.Text.Trim(), txtssid.Text.Trim());
                if (sResult.ToUpper().StartsWith("SUCCESS~"))
                {
                    CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtrsn.Text = "";
                    txtmacid.Text = "";
                    txtcuurentRSNmonth.Text = "";
                    txtnewrsn.Text = "";
                    txtwifikey.Text = "";
                    txtssid.Text = "";
                    txtrsn.Focus();
                }
                else
                {
                    CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    txtrsn.Text = "";
                    txtmacid.Text = "";
                    txtcuurentRSNmonth.Text = "";
                    txtnewrsn.Text = "";
                    txtwifikey.Text = "";
                    txtssid.Text = "";
                    txtrsn.Focus();
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