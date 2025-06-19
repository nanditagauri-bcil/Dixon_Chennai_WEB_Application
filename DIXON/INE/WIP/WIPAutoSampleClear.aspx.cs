using BusinessLayer.WIP;
using Common;
using System;
using System.Data;

namespace DIXON.INE.WIP
{
    public partial class WIPAutoSampleClear : System.Web.UI.Page
    {
        BL_WIPAutoSampleClear blobj = new BL_WIPAutoSampleClear();
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
                    _strRights = CommonHelper.GetRights("WIP Auto Sample Clear", (DataTable)Session["USER_RIGHTS"]);
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

        protected void txtScanMachineID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtScanMachineID.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan MachineID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanMachineID.Focus();
                    return;
                }

                blobj = new BL_WIPAutoSampleClear();
                DataTable dt = blobj.ValidateMachine(txtScanMachineID.Text.Trim(), Session["SiteCode"].ToString(), Session["LINECODE"].ToString());
                if (dt.Rows.Count > 0)
                {
                    CommonHelper.ShowMessage("Valid machine", msgsuccess, CommonHelper.MessageType.Success.ToString());
                    lblMachineName.Text = dt.Rows[0][0].ToString();
                    lblModelNo.Text = dt.Rows[0][2].ToString();
                    lblmachinetype.Text = dt.Rows[0][3].ToString();
                    BindFGItemCode();
                    ddlModel_Name.Focus();
                    ddlModel_Name.SelectedIndex = 0;
                    lblModelName.Text = string.Empty;
                    txtpcbBarcode.Text = string.Empty;
                    txtremark.Text = string.Empty;
                }
                else
                {
                    CommonHelper.ShowMessage("Invalid machine", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanMachineID.Focus();
                    txtScanMachineID.Text = string.Empty;
                    lblMachineName.Text = "";
                    lblModelNo.Text = "";
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void BindFGItemCode()
        {
            try
            {
                ddlModel_Name.Items.Clear();
                blobj = new BL_WIPAutoSampleClear();
                string sResult = string.Empty;
                DataTable dtFGItemCode = blobj.BindFGItemCode(txtScanMachineID.Text.Trim(), Session["SiteCode"].ToString(), Session["LINECODE"].ToString());
                if (dtFGItemCode.Rows.Count > 0)
                {
                    clsCommon.FillMultiColumnsCombo(ddlModel_Name, dtFGItemCode, true);
                    ddlModel_Name.SelectedIndex = 0;
                    ddlModel_Name.Focus();
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
               System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void ddlModel_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    lblModelName.Text = ddlModel_Name.SelectedValue.ToString();
                    txtpcbBarcode.Text = string.Empty;
                    txtremark.Text = string.Empty;
                    txtpcbBarcode.Focus();
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
                ddlModel_Name.SelectedIndex = 0;
                lblModelName.Text = string.Empty;
                txtremark.Text = string.Empty;
                txtpcbBarcode.Text = string.Empty;
                txtScanMachineID.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void txtpcbBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtScanMachineID.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please MachineID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanMachineID.Focus();
                    ddlModel_Name.SelectedIndex = 0;
                    txtpcbBarcode.Text = string.Empty;
                    return;
                }
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtpcbBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan PCB barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtpcbBarcode.Focus();
                    txtpcbBarcode.Text = string.Empty;
                    return;
                }
                blobj = new BL_WIPAutoSampleClear();
                DataSet ds = blobj.ValidatePCB(txtpcbBarcode.Text.Trim(), txtScanMachineID.Text.Trim(),
                   ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(),
                   Session["LINECODE"].ToString());
                if (ds.Tables.Count > 0)
                {
                    string sResult = ds.Tables[0].Rows[0][0].ToString();
                    Message = sResult.Split('~')[1];
                    if (sResult.StartsWith("SUCCESS"))
                    {
                        CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                        return;
                    }
                    else if (sResult.StartsWith("N~"))
                    {
                        CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                        txtpcbBarcode.Text = string.Empty;
                        txtpcbBarcode.Focus();
                        return;
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found for scanned barcode Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbBarcode.Text = string.Empty;
                    txtpcbBarcode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtScanMachineID.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please MachineID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanMachineID.Focus();
                    ddlModel_Name.SelectedIndex = 0;
                    txtpcbBarcode.Text = string.Empty;
                    return;
                }
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtpcbBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan PCB barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtpcbBarcode.Focus();
                    txtpcbBarcode.Text = string.Empty;
                    return;
                }
                blobj = new BL_WIPAutoSampleClear();
                DataSet ds = blobj.SaveResult(txtpcbBarcode.Text.Trim(), txtScanMachineID.Text.Trim(),
                   ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(),
                   Session["LINECODE"].ToString(), Session["UserID"].ToString(), "OKPCBSAMPLE");
                if (ds.Tables.Count > 0)
                {
                    string sResult = ds.Tables[0].Rows[0][0].ToString();
                    Message = sResult.Split('~')[1];
                    if (sResult.StartsWith("SUCCESS"))
                    {
                        CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtpcbBarcode.Text = string.Empty;
                        txtpcbBarcode.Focus();
                        return;
                    }
                    else if (sResult.StartsWith("ERROR~"))
                    {
                        CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                        txtpcbBarcode.Text = string.Empty;
                        txtpcbBarcode.Focus();
                        return;
                    }
                }
                else
                {
                    CommonHelper.ShowMessage(" scanned barcode NOT found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbBarcode.Text = string.Empty;
                    txtpcbBarcode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtScanMachineID.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please MachineID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanMachineID.Focus();
                    ddlModel_Name.SelectedIndex = 0;
                    txtpcbBarcode.Text = string.Empty;
                    return;
                }
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtpcbBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan PCB barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtpcbBarcode.Focus();
                    txtpcbBarcode.Text = string.Empty;
                    return;
                }
                blobj = new BL_WIPAutoSampleClear();
                DataSet ds = blobj.SaveResult(txtpcbBarcode.Text.Trim(), txtScanMachineID.Text.Trim(),
                   ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(),
                   Session["LINECODE"].ToString(), Session["UserID"].ToString(), "REJECTPCBSAMPLE");
                if (ds.Tables.Count > 0)
                {
                    string sResult = ds.Tables[0].Rows[0][0].ToString();
                    Message = sResult.Split('~')[1];
                    if (sResult.StartsWith("SUCCESS"))
                    {
                        CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtpcbBarcode.Text = string.Empty;
                        txtpcbBarcode.Focus();
                        return;
                    }
                    else if (sResult.StartsWith("ERROR~"))
                    {
                        CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                        txtpcbBarcode.Text = string.Empty;
                        txtpcbBarcode.Focus();
                        return;
                    }
                }
                else
                {
                    CommonHelper.ShowMessage(" scanned barcode NOT found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbBarcode.Text = string.Empty;
                    txtpcbBarcode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}