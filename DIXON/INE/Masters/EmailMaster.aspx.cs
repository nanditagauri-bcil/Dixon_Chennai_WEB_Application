using BusinessLayer;
using Common;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace DIXON.INE.Masters
{
    public partial class EmailMaster : System.Web.UI.Page
    {
        string Message = string.Empty;
        string patternfrom = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
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
                Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");
                if (Session["usertype"].ToString() != "ADMIN")
                {
                    string _strRights = CommonHelper.GetRights("EMAIL MASTER", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    _ResetField();
                    ShowGridData();
                    TEXTBOXENABLEDISABLE();
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
                _ResetField();
                ShowGridData();
                TEXTBOXENABLEDISABLE();
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        private void ShowGridData()
        {
            try
            {
                DataTable dt = new DataTable();
                BL_EmailMaster dlobj = new BL_EmailMaster();
                dt = dlobj.GetEmail();
                if (dt.Rows.Count > 0)
                {
                    gvLineMaster.DataSource = dt;
                    gvLineMaster.DataBind();
                    lblNumberofRecords.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    gvLineMaster.DataSource = null;
                    gvLineMaster.DataBind();
                    lblNumberofRecords.Text = "0";
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        public static Regex email_validation()
        {

            string pattern = (@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)");
            return new Regex(pattern, RegexOptions.IgnoreCase);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (string.IsNullOrWhiteSpace(txtEmailSub.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email subject", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtEmailSub.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtEmailBody.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email body", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtEmailBody.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtToEmail.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email to", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtToEmail.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtFromEmail.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email from", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtFromEmail.Focus();
                        return;
                    }
                    Regex validate_emailaddress = email_validation();

                    if (Regex.IsMatch(txtFromEmail.Text, patternfrom) != true)
                    {
                        CommonHelper.ShowMessage("Please enter valid email from", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtFromEmail.Focus();
                        return;
                    }
                    if (validate_emailaddress.IsMatch(txtToEmail.Text) != true)
                    {
                        CommonHelper.ShowMessage("Please enter valid email to", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtToEmail.Focus();
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(txtCCEmail.Text.Trim()))
                    {
                        if (validate_emailaddress.IsMatch(txtCCEmail.Text) != true)
                        {
                            CommonHelper.ShowMessage("Please enter valid email CC", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtCCEmail.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtBCCEmail.Text.Trim()))
                    {
                        if (validate_emailaddress.IsMatch(txtBCCEmail.Text) != true)
                        {
                            CommonHelper.ShowMessage("Please enter valid email BCC", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtBCCEmail.Focus();
                            return;
                        }
                    }
                    BL_EmailMaster dlobj = new BL_EmailMaster();
                    string sResult = dlobj.SaveEmailDetails(txtEmailSub.Text.Trim(), txtEmailBody.Text.Trim(), Session["SiteCode"].ToString(),
                                     txtFromEmail.Text.Trim(), txtToEmail.Text.Trim(), txtCCEmail.Text.Trim(), Session["LINECODE"].ToString(),
                                     txtBCCEmail.Text.Trim(), txtRemarks.Text.Trim(), Session["UserID"].ToString(), "SAVEEMAILDETAILS", hidUID.Value);
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            ShowGridData();
                            _ResetField();
                            TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        else
                        {
                            ShowGridData();
                            _ResetField();
                            TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        }
                    }
                    else
                    {
                        ShowGridData();
                        _ResetField();
                        TEXTBOXENABLEDISABLE();
                        CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtEmailSub.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email subject", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtEmailSub.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtEmailBody.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email body", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtEmailBody.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtToEmail.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email to", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtToEmail.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtFromEmail.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please enter email from", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtFromEmail.Focus();
                        return;
                    }
                    Regex validate_emailaddress = email_validation();

                    if (Regex.IsMatch(txtFromEmail.Text, patternfrom) != true)
                    {
                        CommonHelper.ShowMessage("Please enter valid email from", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtFromEmail.Focus();
                        return;
                    }
                    if (validate_emailaddress.IsMatch(txtToEmail.Text) != true)
                    {
                        CommonHelper.ShowMessage("Please enter valid email to", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtToEmail.Focus();
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(txtCCEmail.Text.Trim()))
                    {
                        if (validate_emailaddress.IsMatch(txtCCEmail.Text) != true)
                        {
                            CommonHelper.ShowMessage("Please enter valid email CC", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtCCEmail.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtBCCEmail.Text.Trim()))
                    {
                        if (validate_emailaddress.IsMatch(txtBCCEmail.Text) != true)
                        {
                            CommonHelper.ShowMessage("Please enter valid email BCC", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtBCCEmail.Focus();
                            return;
                        }
                    }
                    BL_EmailMaster dlobj = new BL_EmailMaster();
                    string sResult = dlobj.SaveEmailDetails(txtEmailSub.Text.Trim(), txtEmailBody.Text.Trim(), Session["SiteCode"].ToString(),
                                     txtFromEmail.Text.Trim(), txtToEmail.Text.Trim(), txtCCEmail.Text.Trim(), Session["LINECODE"].ToString(),
                                     txtBCCEmail.Text.Trim(), txtRemarks.Text.Trim(), Session["UserID"].ToString(), "UPDATEEMAILDETAILS", hidUID.Value);
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            ShowGridData();
                            _ResetField();
                            TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        else
                        {
                            ShowGridData();
                            _ResetField();
                            TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        }
                    }
                    else
                    {
                        ShowGridData();
                        _ResetField();
                        TEXTBOXENABLEDISABLE();
                        CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" +
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }

        }

        private void _ResetField()
        {
            txtEmailSub.Text = string.Empty;
            txtEmailSub.Focus();
            txtEmailBody.Text = string.Empty;
            txtFromEmail.Text = string.Empty;
            txtToEmail.Text = string.Empty;
            txtCCEmail.Text = string.Empty;
            txtBCCEmail.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            btnSave.Text = "Save";
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLineMaster.PageIndex = e.NewPageIndex;
                ShowGridData();
                TEXTBOXENABLEDISABLE();
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                string _SN = string.Empty;
                string[] strValue = e.CommandArgument.ToString().Split('~');
                _SN = e.CommandArgument.ToString();
                if (e.CommandName == "DeleteRecords")
                {
                    DeleteRecords(_SN);
                }
                if (e.CommandName == "EditRecords")
                {
                    if (btnSave.Text == "Save")
                    { btnSave.Text = "Update"; }
                    EditRecords(_SN);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        private void EditRecords(string _SN)
        {
            try
            {
                BL_EmailMaster dlobj = new BL_EmailMaster();
                DataTable dtUserDetails = dlobj.GetSeletedData(_SN);
                if (dtUserDetails.Rows.Count > 0)
                {
                    txtEmailSub.Text = dtUserDetails.Rows[0]["MessageSubject"].ToString();
                    txtEmailBody.Text = dtUserDetails.Rows[0]["MessageBody"].ToString();
                    txtFromEmail.Text = dtUserDetails.Rows[0]["FromMailId"].ToString();
                    txtToEmail.Text = dtUserDetails.Rows[0]["ToMailId"].ToString();
                    txtCCEmail.Text = dtUserDetails.Rows[0]["CCMailID"].ToString();
                    txtBCCEmail.Text = dtUserDetails.Rows[0]["BCCMailID"].ToString();
                    hidUpdate.Value = "Update";
                    hidUID.Value = _SN;
                    txtEmailSub.Enabled = true;
                    txtEmailBody.Enabled = true;
                    txtFromEmail.Enabled = true;
                    txtToEmail.Enabled = true;
                    txtCCEmail.Enabled = true;
                    txtBCCEmail.Enabled = true;
                    txtRemarks.Enabled = true;
                }
                else
                {
                    CommonHelper.ShowMessage("No Email details found.", msgerror, CommonHelper.MessageType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        private void DeleteRecords(string _SN)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            try
            {
                BL_EmailMaster blobj = new BL_EmailMaster();
                string sResult = blobj.DeleteEmail(_SN);
                if (sResult.Length > 0)
                {
                    if (sResult.StartsWith("ERROR~"))
                    {
                        if (sResult.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                        {
                            CommonHelper.ShowMessage("Email already in transaction, you can not delete it", msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        else
                        {
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                    }
                    else if (sResult.StartsWith("N~"))
                    {
                        CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                    else
                    {
                        _ResetField();
                        ShowGridData();
                        TEXTBOXENABLEDISABLE();
                        CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("voilation"))
                {
                    CommonHelper.ShowMessage("Email already in transaction, you can not delete it", msgerror, CommonHelper.MessageType.Error.ToString());
                }
                else
                {
                    CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                }
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);

            }
            finally
            {
                ShowGridData();
                TEXTBOXENABLEDISABLE();
            }
        }

        private void TEXTBOXENABLEDISABLE()
        {
            try
            {
                txtEmailSub.Enabled = false;
                txtEmailBody.Enabled = false;
                txtFromEmail.Enabled = false;
                txtToEmail.Enabled = false;
                txtCCEmail.Enabled = false;
                txtBCCEmail.Enabled = false;
                txtRemarks.Enabled = false;
                if (lblNumberofRecords.Text.Trim() != "1")
                {
                    txtEmailSub.Enabled = true;
                    txtEmailBody.Enabled = true;
                    txtFromEmail.Enabled = true;
                    txtToEmail.Enabled = true;
                    txtCCEmail.Enabled = true;
                    txtBCCEmail.Enabled = true;
                    txtRemarks.Enabled = true;
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