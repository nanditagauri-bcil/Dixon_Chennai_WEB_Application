using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Web.UI;

namespace DIXON.INE.WIP
{
    public partial class SecondaryPackingDelete : System.Web.UI.Page
    {
        BL_SecondaryPackingDelete blobj = new BL_SecondaryPackingDelete();
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
                    string _strRights = CommonHelper.GetRights("WIP Secondary Packing Delete", (DataTable)Session["USER_RIGHTS"]);
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
                blobj = new BL_SecondaryPackingDelete();
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

        private void Reset()
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                GET_FGITEMCODE();
                ddlFgItemCode.Focus();
                ddlFgItemCode.SelectedIndex = 0;
                txtinvoiceno.Text = string.Empty;

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
                    txtinvoiceno.Text = string.Empty;
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtinvoiceno.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Enter Invoice No", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtinvoiceno.Focus();
                    return;
                }
                string sResult = blobj.DeleteSecondaryPacking(ddlFgItemCode.SelectedValue.Trim(),
                    txtinvoiceno.Text.Trim(), Session["UserID"].ToString());
                if (sResult.ToUpper().StartsWith("SUCCESS"))
                {
                    CommonHelper.ShowMessage(sResult, msgsuccess, CommonHelper.MessageType.Success.ToString());
                    ddlFgItemCode.Focus();
                    GET_FGITEMCODE();
                    ddlFgItemCode.SelectedIndex = 0;
                    txtinvoiceno.Text = string.Empty;
                    return;
                }
                if (sResult.StartsWith("ERROR~"))
                {
                    CommonHelper.ShowMessage(sResult, msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlFgItemCode.Focus();
                    GET_FGITEMCODE();
                    ddlFgItemCode.SelectedIndex = 0;
                    txtinvoiceno.Text = string.Empty;
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