using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DIXON.INE.WIP
{
    public partial class MSNvsSecondaryPackingComparison : System.Web.UI.Page
    {
        BL_MSNvsSecondaryPackingComparison blobj = new BL_MSNvsSecondaryPackingComparison();
        string Message = string.Empty;
        DataTable MSNdt = new DataTable();

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
                    _strRights = CommonHelper.GetRights("MSN vs Secondary Packing Comparison", (DataTable)Session["USER_RIGHTS"]);
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
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        private void BindFGItemCode()
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                ddlModel_Name.Items.Clear();
                blobj = new BL_MSNvsSecondaryPackingComparison();
                DataTable dtFGItemCode = blobj.BindFGItemCode(Session["SiteCode"].ToString());
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
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    lblModelName.Text = ddlModel_Name.SelectedValue.ToString();
                    blobj = new BL_MSNvsSecondaryPackingComparison();
                    DataTable dtPO = blobj.BindPO(ddlModel_Name.SelectedItem.Text.Trim().ToString()
                                                                    , Session["SiteCode"].ToString());
                    if (dtPO.Rows.Count > 0)
                    {
                        clsCommon.FillMultiColumnsCombo(ddlPOnumber, dtPO, true);
                        ddlPOnumber.SelectedIndex = 0;
                        ddlPOnumber.Focus();
                    }
                    txtMsnBarcode.Text = string.Empty;
                }
                else
                {
                    gvModel.Visible = false;
                    txtMsnBarcode.Text = string.Empty;
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

        protected void ddlPOnumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlPOnumber.SelectedIndex > 0)
                {
                    blobj = new BL_MSNvsSecondaryPackingComparison();
                    DataTable dtInvoiceNo = blobj.BindInvoiceNo(ddlPOnumber.SelectedItem.Text.Trim().ToString(),
                        ddlModel_Name.SelectedItem.Text.Trim().ToString(), Session["SiteCode"].ToString());
                    if (dtInvoiceNo.Rows.Count > 0)
                    {
                        clsCommon.FillMultiColumnsCombo(ddlInvoiceNumber, dtInvoiceNo, true);
                        ddlInvoiceNumber.SelectedIndex = 0;
                        ddlInvoiceNumber.Focus();
                    }
                    txtMsnBarcode.Text = string.Empty;
                }
                else
                {
                    gvModel.Visible = false;
                    txtMsnBarcode.Text = string.Empty;
                    ddlPOnumber.SelectedIndex = 0;
                    ddlPOnumber.Focus();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void ddlInvoiceNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlInvoiceNumber.SelectedIndex > 0)
                {
                    blobj = new BL_MSNvsSecondaryPackingComparison();
                    DataTable dtSecBOXID = blobj.BindSecBOXID(ddlInvoiceNumber.SelectedItem.Text.Trim().ToString(),
                        ddlPOnumber.SelectedItem.Text.Trim().ToString(),
                        ddlModel_Name.SelectedItem.Text.Trim().ToString(), Session["SiteCode"].ToString());
                    if (dtSecBOXID.Rows.Count > 0)
                    {
                        clsCommon.FillMultiColumnsCombo(ddlSecondaryBoxID, dtSecBOXID, true);
                        ddlSecondaryBoxID.SelectedIndex = 0;
                        ddlSecondaryBoxID.Focus();
                    }
                    txtMsnBarcode.Text = string.Empty;
                }
                else
                {
                    gvModel.Visible = false;
                    txtMsnBarcode.Text = string.Empty;
                    ddlInvoiceNumber.SelectedIndex = 0;
                    ddlInvoiceNumber.Focus();
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
                ddlModel_Name.Focus();
                txtMsnBarcode.Text = string.Empty;
                lblModelName.Text = string.Empty;
                ddlModel_Name.SelectedIndex = 0;
                gvModel.DataSource = null;
                gvModel.Visible = false;
                txtMsnBarcode.ReadOnly = false;
                txtRemarks.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void ddlSecondaryBoxID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlSecondaryBoxID.SelectedIndex > 0)
                {
                    blobj = new BL_MSNvsSecondaryPackingComparison();
                    DataSet ds = blobj.BindMsnBOXID(ddlSecondaryBoxID.SelectedItem.Text.Trim().ToString(),
                    ddlInvoiceNumber.SelectedItem.Text.Trim().ToString(), ddlPOnumber.SelectedItem.Text.Trim().ToString(),
                    ddlModel_Name.SelectedItem.Text.Trim().ToString(), Session["SiteCode"].ToString(), Session["LineCode"].ToString(),
                    Session["UserID"].ToString());
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MSNdt = ds.Tables[0];
                            gvModel.DataSource = MSNdt;
                            gvModel.DataBind();
                            gvModel.Visible = true;
                            BindMSNBARCODECOLOR();
                        }
                        else
                        {
                            CommonHelper.ShowMessage("No MSN BARCODE found for scanned barcode Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                            ddlSecondaryBoxID.SelectedIndex = 0;
                            ddlSecondaryBoxID.Focus();
                            return;
                        }
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No result found for scanned barcode Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                        ddlSecondaryBoxID.SelectedIndex = 0;
                        ddlSecondaryBoxID.Focus();
                        return;
                    }
                }
                else
                {
                    gvModel.Visible = false;
                    txtMsnBarcode.Text = string.Empty;
                    ddlSecondaryBoxID.SelectedIndex = 0;
                    ddlSecondaryBoxID.Focus();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void BindMSNBARCODECOLOR()
        {
            try
            {
                blobj = new BL_MSNvsSecondaryPackingComparison();
                DataTable dt = new DataTable();
                dt = blobj.GetData(ddlModel_Name.SelectedItem.Text.Trim(),
                            Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                            ddlModel_Name.SelectedItem.Text.Trim(), ddlSecondaryBoxID.SelectedItem.Text.Trim().ToString());
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string MSNpcbid = dt.Rows[i]["MSN_BARCODE"].ToString();
                        string RSNpcbid = dt.Rows[i]["RSN_BARCODE"].ToString();
                        foreach (GridViewRow row in gvModel.Rows)
                        {
                            int MSNindex = 0, RSNindex = 0;
                            Label lblBOXID = row.FindControl("lblPRIBOXID") as Label;
                            Label lblRSN = row.FindControl("lblRSN") as Label;
                            TableCell cellBOXID = lblBOXID != null ? lblBOXID.Parent as TableCell : null;
                            TableCell cellRSN = lblRSN != null ? lblRSN.Parent as TableCell : null;
                            if (MSNpcbid == lblBOXID.Text.Trim().ToString())
                            {
                                MSNindex = 1;
                            }
                            if (MSNindex == 1)
                            {
                                cellBOXID.BackColor = System.Drawing.Color.Green;
                                cellBOXID.ForeColor = System.Drawing.Color.White;
                            }
                            if (RSNpcbid == lblRSN.Text.Trim().ToString())
                            {
                                RSNindex = 1;
                            }
                            if (RSNindex == 1)
                            {
                                cellRSN.BackColor = System.Drawing.Color.Violet;
                                cellRSN.ForeColor = System.Drawing.Color.White;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        protected void txtMsnBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (ddlPOnumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select PO Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlPOnumber.Focus();
                    return;
                }
                if (ddlInvoiceNumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Invoice Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlInvoiceNumber.Focus();
                    return;
                }
                if (ddlSecondaryBoxID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Secondary BoxID", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlSecondaryBoxID.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtMsnBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan MSN barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    txtMsnBarcode.Focus();
                    return;
                }
                if (gvModel.Rows.Count == 0)
                {
                    CommonHelper.ShowMessage("No grid data found, please select sec boxid", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlSecondaryBoxID.Focus();
                    return;
                }
                foreach (GridViewRow row in gvModel.Rows)
                {
                    Label lblBOXID = row.FindControl("lblPRIBOXID") as Label;
                    if (txtMsnBarcode.Text.Trim() == lblBOXID.Text.ToString())
                    {
                        if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                        {
                            index = 1;
                        }
                        else
                        {
                            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                            CommonHelper.ShowMessage("MSN BARCODE : " + txtMsnBarcode.Text.Trim() + " already scanned ,Please scanned another MSN BARCODE",
                                msginfo, CommonHelper.MessageType.Success.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            txtMsnBarcode.Focus();
                            return;
                        }
                    }
                    if (index == 1)
                    {
                        blobj = new BL_MSNvsSecondaryPackingComparison();
                        DataTable dt = blobj.ValidateScanMSNBarcode(ddlModel_Name.SelectedValue.ToString(),
                        ddlSecondaryBoxID.SelectedItem.Text.Trim().ToString(), ddlInvoiceNumber.SelectedItem.Text.Trim().ToString(),
                        ddlPOnumber.SelectedItem.Text.Trim().ToString(), ddlModel_Name.SelectedItem.Text.Trim().ToString(),
                        Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                        Session["UserID"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            string sResult = dt.Rows[0][0].ToString();
                            Message = sResult.Split('~')[1];
                            if (sResult.StartsWith("SUCCESS"))
                            {
                                if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                                {
                                    BindMSNBARCODECOLOR();
                                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                    CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                                    index = 2;
                                    txtMsnBarcode.Text = string.Empty;
                                    txtMsnBarcode.Focus();
                                    break;
                                }
                                else
                                {
                                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                    CommonHelper.ShowMessage("MSN BARCODE : " + row.Cells[2].Text + " already scanned ,Please scanned another ,MSN BARCODE",
                                        msgerror, CommonHelper.MessageType.Success.ToString());
                                    txtMsnBarcode.Text = string.Empty;
                                    txtMsnBarcode.Focus();
                                    BindMSNBARCODECOLOR();
                                    return;
                                }
                            }
                            else if (sResult.StartsWith("N~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtMsnBarcode.Text = string.Empty;
                                txtMsnBarcode.Focus();
                                return;
                            }
                            else if (sResult.StartsWith("ERROR~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtMsnBarcode.Text = string.Empty;
                                txtMsnBarcode.Focus();
                            }
                        }
                        else
                        {
                            CommonHelper.ShowMessage("No result found for scanned MSN BARCODE Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            txtMsnBarcode.Focus();
                        }
                    }
                }
                if (index == 0)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage("MSN BARCODE : " + txtMsnBarcode.Text.Trim() +
                        " not FOUND in grid", msgerror, CommonHelper.MessageType.Success.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    txtMsnBarcode.Focus();
                    BindMSNBARCODECOLOR();
                    return;
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtMsnBarcode.Text = string.Empty;
                txtMsnBarcode.Focus();
                BindMSNBARCODECOLOR();
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                int iVerifiedKeyCount = 0;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (ddlPOnumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select PO Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlPOnumber.Focus();
                    return;
                }
                if (ddlInvoiceNumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Invoice Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlInvoiceNumber.Focus();
                    return;
                }
                if (ddlSecondaryBoxID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Secondary BoxID", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlSecondaryBoxID.Focus();
                    return;
                }
                // Create DataTable with necessary columns
                DataTable dtREJECTData = new DataTable();
                dtREJECTData.Columns.Add("SECBOXID");
                dtREJECTData.Columns.Add("PRIBOXID");
                dtREJECTData.Columns.Add("RSN");
                dtREJECTData.Columns.Add("SOFTVER");

                // Iterate through each row of the GridView
                foreach (GridViewRow row in gvModel.Rows)
                {
                    // Find the controls within the GridViewRow
                    Label lblSECBOXID = row.FindControl("lblSECBOXID") as Label;
                    Label lblPRIBOXID = row.FindControl("lblPRIBOXID") as Label;
                    Label lblRSN = row.FindControl("lblRSN") as Label;
                    Label lblSOFTVER = row.FindControl("lblSOFTVER") as Label;

                    // Find the cells containing the labels
                    TableCell cellPRIBOXID = lblPRIBOXID != null ? lblPRIBOXID.Parent as TableCell : null;
                    TableCell cellRSN = lblRSN != null ? lblRSN.Parent as TableCell : null;

                    // Check if the cellPRIBOXID is green
                    if (cellPRIBOXID != null && cellPRIBOXID.BackColor == System.Drawing.Color.Green)
                    {
                        string cell1Text = lblSECBOXID != null ? lblSECBOXID.Text.Trim() : string.Empty;
                        string cell2Text = lblPRIBOXID != null ? lblPRIBOXID.Text.Trim() : string.Empty;
                        string cell3Text = lblRSN != null ? lblRSN.Text.Trim() : string.Empty;
                        string cell4Text = lblSOFTVER != null ? lblSOFTVER.Text.Trim() : string.Empty;

                        // Add the row to the DataTable
                        dtREJECTData.Rows.Add(cell1Text, cell2Text, cell3Text, cell4Text);
                        iVerifiedKeyCount++;
                    }

                    // Check if the cellRSN is violet
                    if (cellRSN != null && cellRSN.BackColor == System.Drawing.Color.Violet)
                    {
                        string cell1Text = lblSECBOXID != null ? lblSECBOXID.Text.Trim() : string.Empty;
                        string cell2Text = lblPRIBOXID != null ? lblPRIBOXID.Text.Trim() : string.Empty;
                        string cell3Text = lblRSN != null ? lblRSN.Text.Trim() : string.Empty;
                        string cell4Text = lblSOFTVER != null ? lblSOFTVER.Text.Trim() : string.Empty;

                        // Add the row to the DataTable
                        dtREJECTData.Rows.Add(cell1Text, cell2Text, cell3Text, cell4Text);
                        iVerifiedKeyCount++;
                    }
                }

                if (iVerifiedKeyCount > 0)
                {
                    blobj = new BL_MSNvsSecondaryPackingComparison();
                    DataTable dt = blobj.REJECTSAVED(ddlModel_Name.SelectedValue.ToString(),
                    ddlSecondaryBoxID.SelectedItem.Text.Trim().ToString(), ddlInvoiceNumber.SelectedItem.Text.Trim().ToString(),
                    ddlPOnumber.SelectedItem.Text.Trim().ToString(), ddlModel_Name.SelectedItem.Text.Trim().ToString(),
                    Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                    Session["UserID"].ToString(), dtREJECTData, txtRemarks.Text.Trim());

                    if (dt.Rows.Count > 0)
                    {
                        string sResult = dt.Rows[0][0].ToString();
                        Message = sResult.Split('~')[1];
                        if (sResult.StartsWith("SUCCESS"))
                        {
                            CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            ddlInvoiceNumber.SelectedIndex = 0;
                            ddlSecondaryBoxID.SelectedIndex = 0;
                            ddlInvoiceNumber.Focus();
                            gvModel.DataSource = null;
                            gvModel.Visible = false;
                            txtRemarks.Text = string.Empty;
                            return;
                        }
                        else if (sResult.StartsWith("ERROR~"))
                        {
                            CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            txtMsnBarcode.Focus();
                            gvModel.DataSource = MSNdt;
                            gvModel.DataBind();
                            BindMSNBARCODECOLOR();
                            return;
                        }
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No result found for scanned MSN Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtMsnBarcode.Text = string.Empty;
                        txtMsnBarcode.Focus();
                        gvModel.DataSource = MSNdt;
                        gvModel.DataBind();
                        return;
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("please scan MSN/RSN barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    txtMsnBarcode.Focus();
                    gvModel.DataSource = MSNdt;
                    gvModel.DataBind();
                    return;
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                BindMSNBARCODECOLOR();
                txtMsnBarcode.Text = string.Empty;
                txtMsnBarcode.Focus();
            }
        }

        protected void txtrsn_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (ddlPOnumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select PO Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlPOnumber.Focus();
                    return;
                }
                if (ddlInvoiceNumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Invoice Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlInvoiceNumber.Focus();
                    return;
                }
                if (ddlSecondaryBoxID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Secondary BoxID", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlSecondaryBoxID.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtrsn.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan RSN", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtrsn.Text = string.Empty;
                    txtrsn.Focus();
                    return;
                }
                if (gvModel.Rows.Count == 0)
                {
                    CommonHelper.ShowMessage("No grid data found, please select sec boxid", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtrsn.Text = string.Empty;
                    ddlSecondaryBoxID.Focus();
                    return;
                }
                foreach (GridViewRow row in gvModel.Rows)
                {
                    Label lblPCBID = row.FindControl("lblRSN") as Label;
                    Label lblBOXID = row.FindControl("lblPRIBOXID") as Label;
                    if (txtrsn.Text.Trim() == lblPCBID.Text.ToString())
                    {
                        if (row.ControlStyle.BackColor != System.Drawing.Color.Violet)
                        {
                            index = 1;
                        }
                        else
                        {
                            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                            CommonHelper.ShowMessage("RSN : " + txtrsn.Text.Trim() + " already scanned ,Please scanned another RSN",
                                msginfo, CommonHelper.MessageType.Success.ToString());
                            txtrsn.Text = string.Empty;
                            txtrsn.Focus();
                            return;
                        }
                    }
                    if (index == 1)
                    {
                        blobj = new BL_MSNvsSecondaryPackingComparison();
                        DataTable dt = blobj.ValidateScanRSN(ddlModel_Name.SelectedValue.ToString(),
                        ddlSecondaryBoxID.SelectedItem.Text.Trim().ToString(), ddlInvoiceNumber.SelectedItem.Text.Trim().ToString(),
                        ddlPOnumber.SelectedItem.Text.Trim().ToString(), ddlModel_Name.SelectedItem.Text.Trim().ToString(),
                        Session["SiteCode"].ToString(), lblBOXID.Text.ToString().Trim(), Session["LineCode"].ToString(),
                        Session["UserID"].ToString(), txtrsn.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            string sResult = dt.Rows[0][0].ToString();
                            Message = sResult.Split('~')[1];
                            if (sResult.StartsWith("SUCCESS"))
                            {
                                if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                                {
                                    BindMSNBARCODECOLOR();
                                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                    CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                                    index = 2;
                                    txtrsn.Text = string.Empty;
                                    txtrsn.Focus();
                                    break;
                                }
                                else
                                {
                                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                    CommonHelper.ShowMessage("RSN BARCODE : " + row.Cells[3].Text + " already scanned ,Please scanned another ,RSN BARCODE",
                                        msgerror, CommonHelper.MessageType.Success.ToString());
                                    txtrsn.Text = string.Empty;
                                    txtrsn.Focus();
                                    BindMSNBARCODECOLOR();
                                    return;
                                }
                            }
                            else if (sResult.StartsWith("N~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtrsn.Text = string.Empty;
                                txtrsn.Focus();
                                BindMSNBARCODECOLOR();
                                return;
                            }
                            else if (sResult.StartsWith("ERROR~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtrsn.Text = string.Empty;
                                txtrsn.Focus();
                                BindMSNBARCODECOLOR();
                                return;
                            }
                        }
                        else
                        {
                            CommonHelper.ShowMessage("No result found for scanned RSN BARCODE Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            txtMsnBarcode.Focus();
                            BindMSNBARCODECOLOR();
                            return;
                        }
                    }
                }
                if (index == 0)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage("RSN BARCODE : " + txtrsn.Text.Trim() +
                        " not FOUND in grid", msgerror, CommonHelper.MessageType.Success.ToString());
                    txtrsn.Text = string.Empty;
                    txtrsn.Focus();
                    BindMSNBARCODECOLOR();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtMsnBarcode.Text = string.Empty;
                txtMsnBarcode.Focus();
                BindMSNBARCODECOLOR();
                return;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                int iVerifiedKeyCount = 0;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (ddlPOnumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select PO Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlPOnumber.Focus();
                    return;
                }
                if (ddlInvoiceNumber.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Invoice Number", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlInvoiceNumber.Focus();
                    return;
                }
                if (ddlSecondaryBoxID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Secondary BoxID", msginfo, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlSecondaryBoxID.Focus();
                    return;
                }
                //DataTable dtRigthsData = new DataTable();
                //dtRigthsData.Columns.Add("SECBOXID");
                //dtRigthsData.Columns.Add("PRIBOXID");
                //dtRigthsData.Columns.Add("RSN");
                //dtRigthsData.Columns.Add("SOFTVER");
                //foreach (GridViewRow row in gvModel.Rows)
                //{
                //    if (row.ControlStyle.BackColor == System.Drawing.Color.Green)
                //    {
                //        string cell1Text = ((Label)row.FindControl("lblSECBOXID")).Text.Trim();
                //        string cell2Text = ((Label)row.FindControl("lblPRIBOXID")).Text.Trim();
                //        string cell3Text = ((Label)row.FindControl("lblRSN")).Text.Trim();
                //        string cell4Text = ((Label)row.FindControl("lblSOFTVER")).Text.Trim();
                //        dtRigthsData.Rows.Add(cell1Text, cell2Text, cell3Text, cell4Text);
                //        iVerifiedKeyCount++;
                //    }
                //}

                // Create DataTable with necessary columns
                DataTable dtRigthsData = new DataTable();
                dtRigthsData.Columns.Add("SECBOXID");
                dtRigthsData.Columns.Add("PRIBOXID");
                dtRigthsData.Columns.Add("RSN");
                dtRigthsData.Columns.Add("SOFTVER");

                // Iterate through each row of the GridView
                foreach (GridViewRow row in gvModel.Rows)
                {
                    // Find the controls within the GridViewRow
                    Label lblSECBOXID = row.FindControl("lblSECBOXID") as Label;
                    Label lblPRIBOXID = row.FindControl("lblPRIBOXID") as Label;
                    Label lblRSN = row.FindControl("lblRSN") as Label;
                    Label lblSOFTVER = row.FindControl("lblSOFTVER") as Label;

                    // Find the cells containing the labels
                    TableCell cellPRIBOXID = lblPRIBOXID != null ? lblPRIBOXID.Parent as TableCell : null;
                    TableCell cellRSN = lblRSN != null ? lblRSN.Parent as TableCell : null;

                    // Check if the cellPRIBOXID is green
                    if (cellPRIBOXID != null && cellPRIBOXID.BackColor == System.Drawing.Color.Green)
                    {
                        string cell1Text = lblSECBOXID != null ? lblSECBOXID.Text.Trim() : string.Empty;
                        string cell2Text = lblPRIBOXID != null ? lblPRIBOXID.Text.Trim() : string.Empty;
                        string cell3Text = lblRSN != null ? lblRSN.Text.Trim() : string.Empty;
                        string cell4Text = lblSOFTVER != null ? lblSOFTVER.Text.Trim() : string.Empty;

                        // Add the row to the DataTable
                        dtRigthsData.Rows.Add(cell1Text, cell2Text, cell3Text, cell4Text);
                        iVerifiedKeyCount++;
                    }

                    // Check if the cellRSN is violet
                    if (cellRSN != null && cellRSN.BackColor == System.Drawing.Color.Violet)
                    {
                        string cell1Text = lblSECBOXID != null ? lblSECBOXID.Text.Trim() : string.Empty;
                        string cell2Text = lblPRIBOXID != null ? lblPRIBOXID.Text.Trim() : string.Empty;
                        string cell3Text = lblRSN != null ? lblRSN.Text.Trim() : string.Empty;
                        string cell4Text = lblSOFTVER != null ? lblSOFTVER.Text.Trim() : string.Empty;

                        // Add the row to the DataTable
                        dtRigthsData.Rows.Add(cell1Text, cell2Text, cell3Text, cell4Text);
                        iVerifiedKeyCount++;
                    }
                }

                if (iVerifiedKeyCount > 0)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    blobj = new BL_MSNvsSecondaryPackingComparison();
                    DataTable dt = blobj.VERIFIEDSAVED(ddlModel_Name.SelectedValue.ToString(),
                        ddlSecondaryBoxID.SelectedItem.Text.Trim().ToString(), ddlInvoiceNumber.SelectedItem.Text.Trim().ToString(),
                        ddlPOnumber.SelectedItem.Text.Trim().ToString(), ddlModel_Name.SelectedItem.Text.Trim().ToString(),
                        Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                        Session["UserID"].ToString(), dtRigthsData, txtRemarks.Text.Trim());

                    if (dt.Rows.Count > 0)
                    {
                        string sResult = dt.Rows[0][0].ToString();
                        Message = sResult.Split('~')[1];
                        if (sResult.StartsWith("SUCCESS"))
                        {
                            CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            ddlSecondaryBoxID.SelectedIndex = 0;
                            ddlSecondaryBoxID.Focus();
                            gvModel.DataSource = null;
                            gvModel.Visible = false;
                            iVerifiedKeyCount = 0;
                            txtRemarks.Text = string.Empty;
                            return;
                        }
                        else if (sResult.StartsWith("ERROR~"))
                        {
                            CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            txtMsnBarcode.Focus();
                            gvModel.DataSource = MSNdt;
                            gvModel.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No result found for scanned MSN Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtMsnBarcode.Text = string.Empty;
                        txtMsnBarcode.Focus();
                        gvModel.DataSource = MSNdt;
                        gvModel.DataBind();
                        return;
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("please scan MSN/RSN barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    txtMsnBarcode.Focus();
                    gvModel.DataSource = MSNdt;
                    gvModel.DataBind();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtMsnBarcode.Text = string.Empty;
                txtMsnBarcode.Focus();
                BindMSNBARCODECOLOR();
                return;
            }
        }
    }
}