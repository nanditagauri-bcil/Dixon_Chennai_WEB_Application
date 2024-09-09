using BusinessLayer;
using BusinessLayer.MES.PRINTING;
using Common;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace DIXON.INE.Masters
{
    public partial class AccessoresScanning : System.Web.UI.Page
    {
        BL_Acc_Scanning blobj = new BL_Acc_Scanning();
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
                    _strRights = CommonHelper.GetRights("Accessories Scanning", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    BindFGItemCode();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        private void BindFGItemCode()
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (gvModel.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvModel.Rows)
                    {
                        row.ControlStyle.BackColor = System.Drawing.Color.White;
                    }
                }
                gvModel.DataSource = null;
                gvModel.DataBind();
                ddlModel_Name.Items.Clear();
                lblModelName.Text = string.Empty;
                txtScanHere.Text = string.Empty;
                txtAccessoriesBarcode.Text = string.Empty;
                BL_MobCommon blobj = new BL_MobCommon();
                string sResult = string.Empty;
                DataTable dtFGItemCode = blobj.BindModel();
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
        private void bindModelKeys()
        {
            try
            {
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    blobj = new BL_Acc_Scanning();
                    DataTable dt = blobj.Bind_Model_Mapping_Accessories(Convert.ToString(ddlModel_Name.SelectedItem.Text), Session["SiteCode"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        gvModel.Visible = true;
                        gvModel.DataSource = dt;
                        gvModel.DataBind();
                        ViewState["DATA"] = dt;
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No key parts are mapped against selected model or color, Please map in model key mapping module."
                            , msgerror, CommonHelper.MessageType.Error.ToString());
                        gvModel.Visible = false;
                        ddlModel_Name.SelectedIndex = 0;
                        ddlModel_Name.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void ddlModel_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    lblModelName.Text = ddlModel_Name.SelectedValue.ToString();
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtScanHere.Text = string.Empty;
                    txtScanHere.Focus();
                    bindModelKeys();
                }
                else
                {
                    gvModel.Visible = false;
                    txtScanHere.Text = string.Empty;
                    txtAccessoriesBarcode.Text = string.Empty;
                    lblModelName.Text = string.Empty;
                    ddlModel_Name.Focus();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void txtScanHere_TextChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvModel.Rows)
                {
                    row.ControlStyle.BackColor = System.Drawing.Color.White;
                }
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtScanHere.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtScanHere.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan Here", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtScanHere.Focus();
                    return;
                }

                blobj = new BL_Acc_Scanning();
                DataTable dt = new DataTable();
                dt = blobj.blScanIMEI(ddlModel_Name.SelectedItem.Text.ToString().Trim(), Session["SiteCode"].ToString(), txtScanHere.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    Message = dt.Rows[0][0].ToString();
                    if (Message.StartsWith("SUCCESS~"))
                    {
                        CommonHelper.ShowMessage("Valid barcode", msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtAccessoriesBarcode.Text = string.Empty;
                        txtAccessoriesBarcode.Focus();
                    }
                    else
                    {
                        if (Message.StartsWith("2~"))
                        {
                            //gvModel.DataSource = (DataTable)ViewState["DATA"];
                            //DataTable dtLoadData = (DataTable)ViewState["DATA"];
                            string[] sArr = dt.Rows[0][0].ToString().Split('~')[1].Split(',');
                            foreach (GridViewRow row in gvModel.Rows)
                            {
                                Label lblAccessoriesName = row.FindControl("lblAccessName") as Label;
                                for (int i = 0; i < sArr.Length; i++)
                                {
                                    if (lblAccessoriesName.Text == sArr[i].ToString().Split('^')[0].Trim())
                                    {
                                        row.ControlStyle.BackColor = System.Drawing.Color.Green;
                                        row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                        row.Cells[5].Text = sArr[i].ToString().Split('^')[1];
                                    }
                                }
                            }
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                        }
                        else
                        {
                            CommonHelper.ShowMessage(Message.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtScanHere.Text = string.Empty;
                            txtScanHere.Focus();
                        }
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found,Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtScanHere.Text = string.Empty;
                    txtScanHere.Focus();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void txtAccessoriesBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int iLoopCount = 0; // Increase the count each time if scanned acc not matched, to show error
                int iScannedCount = 0;
                bool bIsAlreadyVerified = false;
                string sVerifyAccName = string.Empty;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please Select FG item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanHere.Text = string.Empty;
                    txtAccessoriesBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtScanHere.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan Pcb Barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtScanHere.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtAccessoriesBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan Accessory Barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Focus();
                    return;
                }

                string sMacID = null;
                string sAdaptorSN = null; // Keep the variable null to store null in database if no value set
                string sScanPcbBarcode = txtScanHere.Text.Trim().ToUpper();
                string sModelName = lblModelName.Text.Trim().ToUpper();
                string sAccBarcode = txtAccessoriesBarcode.Text.Trim().ToUpper();
                string tempregexString = Regex.Replace(txtAccessoriesBarcode.Text.Trim(), @"[\r\n\t]+^|[ ]", string.Empty);

                foreach (GridViewRow row in gvModel.Rows)
                {
                    Label lblAccessoriesName = row.FindControl("lblAccessName") as Label;
                    Label lblKeyValue = row.FindControl("Value") as Label;
                    Label lblStartDigit = row.FindControl("StartDigit") as Label;
                    Label lblEndDigit = row.FindControl("EndDigit") as Label;

                    string sAccName = lblAccessoriesName.Text.ToString().Trim().ToUpper();
                    string sKeyValue = lblKeyValue.Text.ToString().Trim().ToUpper();

                    if (sAccName != "STAND LABEL" && sAccBarcode.StartsWith("<?XML"))
                    {
                        iLoopCount++;
                        continue;
                    }

                    if (sAccName == "STAND LABEL" && !sAccBarcode.StartsWith("<?XML"))
                    {
                        iLoopCount++;
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(sKeyValue) && sAccName != "STAND LABEL")
                    {
                        if (Regex.IsMatch(sAccBarcode, @"[<>\/?!]"))
                        {
                            if (!sAccBarcode.Contains(sKeyValue))
                            {
                                iLoopCount++;
                                continue;
                            }
                        }
                        else
                        {
                            if (!sAccBarcode.Contains(sKeyValue))
                            {
                                iLoopCount++;
                                continue;
                            }

                            if (sAccName == "DEVICE" && sAccBarcode == sScanPcbBarcode)
                            {
                                iLoopCount++;
                                continue;
                            }
                        }
                    }

                    // STAND LABEL ACCESSORY
                    if (sAccName == "STAND LABEL" && sAccBarcode.StartsWith("<?XML"))
                    {
                        try
                        {
                            if (CheckIfAlreadyVerified(row))
                            {
                                sVerifyAccName = sAccName;
                                bIsAlreadyVerified = true;
                                continue;
                            }

                            if (sAccBarcode.Contains("<SRNO>"))
                            {
                                sMacID = Regex.Split(sAccBarcode, "<MACID>").Last().Split('<')[0];
                                sAccBarcode = Regex.Split(sAccBarcode, "<SRNO>").Last().Split('<')[0];
                            }
                            else
                            {
                                RootKeywordNotFound(sAccName);
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            CommonHelper.ShowMessage("<b>XML</b> design is inaccurate, Please correct the format", msgerror, CommonHelper.MessageType.Error.ToString());
                            return;
                        }
                    }
                    // OTHER THAN STAND LABEL ACCESSORY
                    else if (sAccName != "STAND LABEL" && !sAccBarcode.StartsWith("<?XML"))
                    {
                        int nStartDigit = ConvertToInteger(lblStartDigit);
                        int nEndDigit = ConvertToInteger(lblEndDigit);

                        if (string.IsNullOrEmpty(sKeyValue))
                        {
                            CommonHelper.ShowMessage($"Key <b>Value</b> not found for the scanned accessory : {sAccName}. Please define the Value in mapping", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                            return;
                        }

                        if (nStartDigit == 0 && nEndDigit == 0)
                        {
                            CommonHelper.ShowMessage($"Validation not found for the scanned accessory : {sAccName}. Please define the <b>Start</b> and <b>End Digit</b> in mapping", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                            return;
                        }

                        if (sKeyValue.Length != nEndDigit)
                        {
                            CommonHelper.ShowMessage($"The length of the Key <b>Value</b> and <b>End Digit</b> is not matched for the scanned accessory : <b>{sAccName}</b>. Please change the <b>End Digit</b> value", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                            return;
                        }

                        if (sAccBarcode.Length < nEndDigit)
                        {
                            CommonHelper.ShowMessage($"Scanned accessory {sAccName} barcode length is less than the defined <b>End Digit</b> value.<br> Please change max length of key <b>End Digit</b>", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                            return;
                        }

                        if (sAccName == "ADAPTOR")
                        {
                            if (CheckIfAlreadyVerified(row))
                            {
                                sVerifyAccName = sAccName;
                                bIsAlreadyVerified = true;
                                continue;
                            }

                            if (!sAccBarcode.Contains("<SRNO>"))
                            {
                                RootKeywordNotFound(sAccName);
                                return;
                            }

                            string sEanNo = string.Empty;
                            if (sAccBarcode.StartsWith("<SRNO>") && sAccBarcode.EndsWith("</EAN>"))
                            {
                                sAdaptorSN = Regex.Split(sAccBarcode, "<SRNO>").Last().Split('<')[0];
                                sEanNo = Regex.Split(sAccBarcode, "<EAN>").Last().Split('<')[0];
                            }
                            else
                            {
                                CommonHelper.ShowMessage($"Scanned <b>{sAccName}</b> accessory barcode format is incorrect, It should start with <b>&lt;SRNO&gt;</b> and ends with <b>&lt;/EAN&gt;</b>", msgerror, CommonHelper.MessageType.Error.ToString());
                                txtAccessoriesBarcode.Text = string.Empty;
                                txtAccessoriesBarcode.Focus();
                                return;
                            }

                            try
                            {
                                if (!sEanNo.Substring(nStartDigit, nEndDigit).Contains(sKeyValue))
                                {
                                    KeyValueNotMatched(sAccName);
                                    return;
                                }
                            }
                            catch (Exception)
                            {
                                AccKeyValueLengthNotMatched(sAccName);
                                return;
                            }
                        }
                        else if (sAccName == "DEVICE")
                        {
                            if (CheckIfAlreadyVerified(row))
                            {
                                sVerifyAccName = sAccName;
                                bIsAlreadyVerified = true;
                                continue;
                            }

                            //Model With Stand
                            if (sModelName == "JCOW411 CKD")
                            {
                                if (!sAccBarcode.Contains("<SRNO>"))
                                {
                                    RootKeywordNotFound(sAccName);
                                    return;
                                }

                                if (sAccBarcode.StartsWith("<SRNO>") && sAccBarcode.EndsWith("</SRNO>"))
                                {
                                    sAccBarcode = Regex.Split(sAccBarcode, "<SRNO>").Last().Split('<')[0];
                                }
                                else
                                {
                                    CommonHelper.ShowMessage($"Scanned <b>{sAccName}</b> accessory barcode format is incorrect, It should contains start with <b>&lt;SRNO&gt;</b> and ends with <b>&lt;/SRNO&gt;</b>", msgerror, CommonHelper.MessageType.Error.ToString());
                                    txtAccessoriesBarcode.Text = string.Empty;
                                    txtAccessoriesBarcode.Focus();
                                    return;
                                }
                            }

                            try
                            {
                                if (!sAccBarcode.Substring(nStartDigit, nEndDigit).Contains(sKeyValue))
                                {
                                    KeyValueNotMatched(sAccName);
                                    return;
                                }
                            }
                            catch (Exception)
                            {
                                AccKeyValueLengthNotMatched(sAccName);
                                return;
                            }
                        }
                        else
                        {
                            if (sAccName != "DEVICE")
                            {
                                if (CheckIfAlreadyVerified(row))
                                {
                                    sVerifyAccName = sAccName;
                                    bIsAlreadyVerified = true;
                                    continue;
                                }

                                if (Regex.IsMatch(sAccBarcode, @"[<>\/?!]"))
                                {
                                    CommonHelper.ShowMessage($"Scanned <b>{sAccName}</b> accessory barcode is not valid. It should not contain any XML format", msgerror, CommonHelper.MessageType.Error.ToString());
                                    txtAccessoriesBarcode.Text = string.Empty;
                                    txtAccessoriesBarcode.Focus();
                                    return;
                                }

                                try
                                {
                                    if (!sAccBarcode.Substring(nStartDigit, nEndDigit).Contains(sKeyValue))
                                    {
                                        KeyValueNotMatched(sAccName);
                                        return;
                                    }
                                }
                                catch (Exception)
                                {
                                    AccKeyValueLengthNotMatched(sAccName);
                                    return;
                                }
                            }
                            else
                            {
                                iLoopCount++;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        iLoopCount++;
                        continue;
                    }

                    blobj = new BL_Acc_Scanning();
                    DataTable dt = new DataTable();
                    dt = blobj.blScanAccessories(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), sScanPcbBarcode, Session["LineCode"].ToString(), sModelName, sAccBarcode, sAccName, Session["UserID"].ToString(), txtAccessoriesBarcode.Text.ToString().Trim(), sAdaptorSN, sMacID);

                    if (dt.Rows.Count > 0)
                    {
                        Message = dt.Rows[0][0].ToString();
                        if (Message.StartsWith("SUCCESS~"))
                        {
                            CommonHelper.ShowMessage("Data saved successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());
                            row.ControlStyle.BackColor = System.Drawing.Color.Green;
                            row.ControlStyle.ForeColor = System.Drawing.Color.White;
                            row.Cells[5].Text = txtAccessoriesBarcode.Text.Trim();
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                            foreach (GridViewRow row1 in gvModel.Rows)
                            {
                                if (row1.ControlStyle.BackColor == System.Drawing.Color.Green)
                                {
                                    iScannedCount++;
                                }
                            }
                            if (iScannedCount == gvModel.Rows.Count)
                            {
                                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                CommonHelper.ShowMessage("All the accessories barcode are scanned", msgsuccess, CommonHelper.MessageType.Success.ToString());
                                bindModelKeys();
                                txtAccessoriesBarcode.Text = string.Empty;
                                txtScanHere.Text = string.Empty;
                                txtScanHere.Focus();
                            }
                        }
                        else
                        {
                            CommonHelper.ShowMessage(Message.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                            if (Message.StartsWith("0~"))
                            {
                                row.ControlStyle.BackColor = System.Drawing.Color.Green;
                                row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                row.Cells[5].Text = txtAccessoriesBarcode.Text.Trim();
                                txtAccessoriesBarcode.Text = string.Empty;
                                txtAccessoriesBarcode.Focus();
                            }
                        }
                        return;
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No result found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtAccessoriesBarcode.Text = "";
                        txtAccessoriesBarcode.Focus();
                        return;
                    }
                }

                if (bIsAlreadyVerified)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage($"Scanned <b>{sVerifyAccName}</b> accessory already bind", msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtAccessoriesBarcode.Focus();
                    return;
                }

                if (iLoopCount >= gvModel.Rows.Count)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage("Scanned accessory barcode not matched with any defined key <b>Value</b>", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtAccessoriesBarcode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtAccessoriesBarcode.Text = string.Empty;
                txtAccessoriesBarcode.Focus();
            }
        }
        private void AccKeyValueLengthNotMatched(string sAccName)
        {
            CommonHelper.ShowMessage($"Scanned <b>{sAccName}</b> accessory barcode length is less than the defined <b>End Digit</b> value", msgerror, CommonHelper.MessageType.Error.ToString());
            txtAccessoriesBarcode.Text = string.Empty;
            txtAccessoriesBarcode.Focus();
        }
        private void KeyValueNotMatched(string sAccName)
        {
            CommonHelper.ShowMessage($"Scanned <b>{sAccName}</b> accessory barcode not matched with the defined <b>Value</b>", msgerror, CommonHelper.MessageType.Error.ToString());
            txtAccessoriesBarcode.Text = string.Empty;
            txtAccessoriesBarcode.Focus();
        }
        private void RootKeywordNotFound(string sAccName)
        {
            CommonHelper.ShowMessage($"Scanned <b>{sAccName}</b> accessory barcode do not have a root keyword <b>&lt;SRNO&gt;</b>", msgerror, CommonHelper.MessageType.Error.ToString());
            txtAccessoriesBarcode.Text = string.Empty;
            txtAccessoriesBarcode.Focus();
        }
        private bool CheckIfAlreadyVerified(GridViewRow row)
        {
            if (row.ControlStyle.BackColor == System.Drawing.Color.Green)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private int ConvertToInteger(Label lblKeyDigit)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(lblKeyDigit.Text.ToString().Trim()))
                {
                    return Convert.ToInt32(Regex.Replace(lblKeyDigit.Text.ToString().Trim(), "[^0-9]+", string.Empty));
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                CommonHelper.ShowMessage("Defined Key <b>Start Digit</b> or <b>End Digit</b> is not valid. It should be a numeric value", msgerror, CommonHelper.MessageType.Error.ToString());
                txtAccessoriesBarcode.Text = string.Empty;
                txtAccessoriesBarcode.Focus();
                return 0;
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                BindFGItemCode();
                gvModel.Visible = false;
                txtScanHere.Text = string.Empty;
                txtAccessoriesBarcode.Text = string.Empty;
                lblModelName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}