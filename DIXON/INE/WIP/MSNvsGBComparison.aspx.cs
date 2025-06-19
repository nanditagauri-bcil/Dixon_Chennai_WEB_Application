using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DIXON.INE.WIP
{
    public partial class MSNvsGBComparison : System.Web.UI.Page
    {
        BL_MSNvsGBComparison blobj = new BL_MSNvsGBComparison();
        string Message = string.Empty;
        DataTable GBdt = new DataTable();

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
                    _strRights = CommonHelper.GetRights("MSN vs GB Comparison", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    BindFGItemCode();
                    txtMsnBarcode.ReadOnly = false;
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
                ddlModel_Name.Items.Clear();
                blobj = new BL_MSNvsGBComparison();
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
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    lblModelName.Text = ddlModel_Name.SelectedValue.ToString();
                    txtMsnBarcode.Text = string.Empty;
                    txtGBBarcode.Text = string.Empty;
                    txtMsnBarcode.Focus();
                }
                else
                {
                    gvModel.Visible = false;
                    txtMsnBarcode.Text = string.Empty;
                    txtGBBarcode.Text = string.Empty;
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
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                ddlModel_Name.Focus();
                txtMsnBarcode.Text = string.Empty;
                txtGBBarcode.Text = string.Empty;
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
        protected void txtMsnBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtMsnBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan MSN barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtMsnBarcode.Focus();
                    txtMsnBarcode.Text = string.Empty;
                    return;
                }
                blobj = new BL_MSNvsGBComparison();
                DataSet ds = blobj.ValidateMSN(txtMsnBarcode.Text.Trim(), ddlModel_Name.SelectedItem.Text.Trim(),
                    Session["SiteCode"].ToString(), Session["LINECODE"].ToString(), Session["UserID"].ToString());
                if (ds.Tables.Count > 0)
                {
                    string sResult = ds.Tables[0].Rows[0][0].ToString();
                    Message = sResult.Split('~')[1];
                    if (sResult.StartsWith("SUCCESS"))
                    {
                        txtMsnBarcode.ReadOnly = true;
                        txtGBBarcode.Focus();
                        CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            GBdt = ds.Tables[1];
                            gvModel.DataSource = GBdt;
                            gvModel.DataBind();
                            gvModel.Visible = true;
                            BindGBBARCODECOLOR();
                        }
                        else
                        {
                            CommonHelper.ShowMessage("No GB BARCODE for scanned barcode Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            txtMsnBarcode.ReadOnly = false;
                            txtMsnBarcode.Focus();
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
                        return;
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found for scanned barcode Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    txtMsnBarcode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        private void BindGBBARCODECOLOR()
        {
            try
            {
                blobj = new BL_MSNvsGBComparison();
                DataTable dt = new DataTable();
                dt = blobj.GetData(ddlModel_Name.SelectedItem.Text.Trim(),
                            Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                            ddlModel_Name.SelectedItem.Text.Trim(), txtGBBarcode.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string gbpcbid = dt.Rows[i]["PCBID"].ToString();
                        foreach (GridViewRow row in gvModel.Rows)
                        {
                            int index = 0;
                            Label lblRSN = row.FindControl("lblRSN") as Label;
                            if (gbpcbid == lblRSN.Text.ToString())
                            {
                                index = 1;
                            }
                            if (index == 1)
                            {
                                row.ControlStyle.BackColor = System.Drawing.Color.Green;
                                row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                break;
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
        protected void txtGBBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                int iVerifiedKeyCount = 0;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtMsnBarcode.Text = string.Empty;
                    txtGBBarcode.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtMsnBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan MSN barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtMsnBarcode.Focus();
                    txtMsnBarcode.Text = string.Empty;
                    txtGBBarcode.Text = string.Empty;
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtGBBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan GB Barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtGBBarcode.Focus();
                    txtGBBarcode.Text = string.Empty;
                    return;
                }
                DataTable dtRigthsData = new DataTable();
                dtRigthsData.Columns.Add("MSNBARCODE");
                dtRigthsData.Columns.Add("GBBARCODE");
                dtRigthsData.Columns.Add("MACID");
                foreach (GridViewRow row in gvModel.Rows)
                {
                    Label lblRSN = row.FindControl("lblRSN") as Label;
                    if (txtGBBarcode.Text.Trim() == lblRSN.Text.ToString())
                    {
                        if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                        {
                            index = 1;
                        }
                        else
                        {
                            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                            CommonHelper.ShowMessage("GB BARCODE : " + txtGBBarcode.Text.Trim() + " already scanned ,Please scanned another GB BARCODE",
                                msgerror, CommonHelper.MessageType.Success.ToString());
                            txtGBBarcode.Text = string.Empty;
                            txtGBBarcode.Focus();
                            return;
                        }
                    }
                    if (index == 1)
                    {
                        blobj = new BL_MSNvsGBComparison();
                        DataTable dt = blobj.ValidateScanGBBarcode(ddlModel_Name.SelectedValue.ToString(),
                            Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                            ddlModel_Name.SelectedItem.Text.Trim(), txtGBBarcode.Text.Trim(), Session["UserID"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            string sResult = dt.Rows[0][0].ToString();
                            Message = sResult.Split('~')[1];
                            if (sResult.StartsWith("SUCCESS"))
                            {
                                if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                                {
                                    BindGBBARCODECOLOR();
                                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                    CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                                    index = 2;
                                    txtGBBarcode.Text = string.Empty;
                                    txtGBBarcode.Focus();
                                    break;
                                }
                                else
                                {
                                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                    CommonHelper.ShowMessage("GB BARCODE : " + row.Cells[2].Text + " already scanned ,Please scanned another GB BARCODE",
                                        msgerror, CommonHelper.MessageType.Success.ToString());
                                    txtGBBarcode.Text = string.Empty;
                                    txtGBBarcode.Focus();
                                    BindGBBARCODECOLOR();
                                    return;
                                }
                            }
                            else if (sResult.StartsWith("N~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtGBBarcode.Text = string.Empty;
                                txtGBBarcode.Focus();
                                return;
                            }
                            else if (sResult.StartsWith("ERROR~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtGBBarcode.Text = string.Empty;
                                txtGBBarcode.Focus();
                            }
                        }
                        else
                        {
                            CommonHelper.ShowMessage("No result found for scanned GB BARCODE Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtGBBarcode.Text = string.Empty;
                            txtGBBarcode.Focus();
                        }
                    }
                }
                if (index == 0)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage("GB BARCODE : " + txtGBBarcode.Text.Trim() +
                        " not FOUND in grid", msgerror, CommonHelper.MessageType.Success.ToString());
                    txtGBBarcode.Text = string.Empty;
                    txtGBBarcode.Focus();
                    BindGBBARCODECOLOR();
                    return;
                }
                foreach (GridViewRow row in gvModel.Rows)
                {
                    if (row.ControlStyle.BackColor == System.Drawing.Color.Green)
                    {
                        string cell1Text = ((Label)row.FindControl("lblBOXID")).Text.Trim();
                        string cell2Text = ((Label)row.FindControl("lblRSN")).Text.Trim();
                        string cell3Text = ((Label)row.FindControl("lblMACID")).Text.Trim();
                        dtRigthsData.Rows.Add(cell1Text, cell2Text, cell3Text);
                        iVerifiedKeyCount++;
                    }
                }
                if (iVerifiedKeyCount == gvModel.Rows.Count)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    blobj = new BL_MSNvsGBComparison();
                    DataTable dt = blobj.VERIFIEDSAVED(ddlModel_Name.SelectedValue.ToString(),
                        Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                        ddlModel_Name.SelectedItem.Text.Trim(), Session["UserID"].ToString(), dtRigthsData);

                    if (dt.Rows.Count > 0)
                    {
                        string sResult = dt.Rows[0][0].ToString();
                        Message = sResult.Split('~')[1];
                        if (sResult.StartsWith("SUCCESS"))
                        {
                            CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                            txtMsnBarcode.Text = string.Empty;
                            txtGBBarcode.Text = string.Empty;
                            txtMsnBarcode.Focus();
                            txtMsnBarcode.ReadOnly = false;
                            gvModel.DataSource = null;
                            gvModel.Visible = false;
                            iVerifiedKeyCount = 0;
                            txtRemarks.Text = string.Empty;
                        }
                        else if (sResult.StartsWith("ERROR~"))
                        {
                            CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                            txtGBBarcode.Text = string.Empty;
                            txtGBBarcode.Focus();
                            gvModel.DataSource = GBdt;
                            gvModel.DataBind();

                        }
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No result found for scanned Wave Pallet Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtGBBarcode.Text = string.Empty;
                        txtGBBarcode.Focus();
                        gvModel.DataSource = GBdt;
                        gvModel.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtGBBarcode.Text = string.Empty;
                txtGBBarcode.Focus();
                BindGBBARCODECOLOR();
            }
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtMsnBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan MSN barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtMsnBarcode.Focus();
                    txtMsnBarcode.Text = string.Empty;
                    return;
                }
                DataTable dtREJECTData = new DataTable();
                dtREJECTData.Columns.Add("MSNBARCODE");
                dtREJECTData.Columns.Add("GBBARCODE");
                dtREJECTData.Columns.Add("MACID");
                foreach (GridViewRow row in gvModel.Rows)
                {
                    if (row.ControlStyle.BackColor == System.Drawing.Color.Green)
                    {
                        string cell1Text = ((Label)row.FindControl("lblBOXID")).Text.Trim();
                        string cell2Text = ((Label)row.FindControl("lblRSN")).Text.Trim();
                        string cell3Text = ((Label)row.FindControl("lblMACID")).Text.Trim();
                        dtREJECTData.Rows.Add(cell1Text, cell2Text, cell3Text);
                    }
                }
                blobj = new BL_MSNvsGBComparison();
                DataTable dt = blobj.REJECTSAVED(ddlModel_Name.SelectedValue.ToString(),
                    Session["SiteCode"].ToString(), txtMsnBarcode.Text.Trim(), Session["LineCode"].ToString(),
                    ddlModel_Name.SelectedItem.Text.Trim(), Session["UserID"].ToString(), dtREJECTData, txtRemarks.Text.Trim());

                if (dt.Rows.Count > 0)
                {
                    string sResult = dt.Rows[0][0].ToString();
                    Message = sResult.Split('~')[1];
                    if (sResult.StartsWith("SUCCESS"))
                    {
                        CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtMsnBarcode.Text = string.Empty;
                        txtGBBarcode.Text = string.Empty;
                        txtMsnBarcode.Focus();
                        txtMsnBarcode.ReadOnly = false;
                        gvModel.DataSource = null;
                        gvModel.Visible = false;
                        txtRemarks.Text = string.Empty;
                    }
                    else if (sResult.StartsWith("ERROR~"))
                    {
                        CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                        txtGBBarcode.Text = string.Empty;
                        txtGBBarcode.Focus();
                        gvModel.DataSource = GBdt;
                        gvModel.DataBind();
                        BindGBBARCODECOLOR();

                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found for scanned Wave Pallet Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtGBBarcode.Text = string.Empty;
                    txtGBBarcode.Focus();
                    gvModel.DataSource = GBdt;
                    gvModel.DataBind();
                    BindGBBARCODECOLOR();
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                BindGBBARCODECOLOR();
                txtGBBarcode.Text = string.Empty;
                txtGBBarcode.Focus();
            }
        }
    }
}