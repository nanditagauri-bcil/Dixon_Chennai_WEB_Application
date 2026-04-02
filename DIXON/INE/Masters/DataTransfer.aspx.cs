using BusinessLayer.Masters;
using Common;
using System;
using System.Data;
using System.Web.UI;

namespace DIXON.INE.Masters
{
    public partial class DataTransfer : Page
    {
        BL_DataTransfer blobj = new BL_DataTransfer();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Signin/v1/Login.aspx?Session=Null");
            }
        }

        private void _ResetField()
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            //txtIssueSlipNo.Focus();
            //txtIssueSlipNo.Text = "";
            drpIssueSlipNo.SelectedIndex = 0;
            drpWorkOrderNo.SelectedIndex = 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (Session["usertype"].ToString() != "ADMIN")
                {
                    string _strRights = CommonHelper.GetRights("DATA TRANSFER", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    try
                    {
                        BindIssueSlipNo();
                        BindWorkOrderNo();
                    }
                    catch (Exception ex)
                    {
                        CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        private void BindIssueSlipNo()
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                blobj = new BL_DataTransfer();
                DataTable dt = blobj.BindIssueSlipNo(Session["SiteCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(drpIssueSlipNo, dt, true);
                    drpIssueSlipNo.SelectedIndex = 0;
                    drpIssueSlipNo.Focus();
                }
                else
                {
                    drpIssueSlipNo.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void BindWorkOrderNo()
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                blobj = new BL_DataTransfer();
                DataTable dt = blobj.BindWorkOrderNo(Session["SiteCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(drpWorkOrderNo, dt, true);
                    drpWorkOrderNo.SelectedIndex = 0;
                    drpWorkOrderNo.Focus();
                }
                else
                {
                    drpWorkOrderNo.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                //if (string.IsNullOrWhiteSpace(txtIssueSlipNo.Text))
                //{
                //    CommonHelper.ShowMessage("Please enter transfer value", msgerror, CommonHelper.MessageType.Error.ToString());
                //    txtIssueSlipNo.Focus();
                //    return;
                //}
                if (drpIssueSlipNo.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Issue Slip No", msgerror, CommonHelper.MessageType.Error.ToString());
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
                    drpIssueSlipNo.Focus();
                    return;
                }
                if (drpWorkOrderNo.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Work Order No", msgerror, CommonHelper.MessageType.Error.ToString());
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
                    drpWorkOrderNo.Focus();
                    return;
                }
                blobj = new BL_DataTransfer();
                DataTable dt = blobj.DataTransfer(drpType.Text, drpIssueSlipNo.SelectedValue.Trim(), drpWorkOrderNo.SelectedValue.Trim(), Session["UserID"].ToString());
                if (dt.Rows.Count > 0)
                {
                    string sResult = dt.Rows[0][0].ToString();
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("SUCCESS~"))
                        {
                            _ResetField();
                            CommonHelper.ShowMessage(sResult, msgsuccess, CommonHelper.MessageType.Success.ToString());
                        }
                        else
                        {
                            _ResetField();
                            CommonHelper.ShowMessage(sResult, msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No record found to transfer", msgerror, CommonHelper.MessageType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                     System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                if (ex.Message.Contains("Could not find server"))
                {
                    CommonHelper.ShowMessage("Unable to connect with Base server, Please contact server admin for checking linking server.", msgerror, CommonHelper.MessageType.Error.ToString());
                }
                else
                {
                    CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            _ResetField();
        }
    }
}