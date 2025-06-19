using BusinessLayer;
using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DIXON.INE.WIP
{
    public partial class WIPAccessoriesVerification : System.Web.UI.Page
    {
        BL_WIPAccessoriesVerification blobj = new BL_WIPAccessoriesVerification();
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
                    _strRights = CommonHelper.GetRights("Accessories Verification", (DataTable)Session["USER_RIGHTS"]);
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
                txtScanHere.Text = string.Empty;
                txtAccessoriesBarcode.Text = string.Empty;
                BL_MobCommon blobj = new BL_MobCommon();
                string sResult = string.Empty;
                DataTable dtFGItemCode = blobj.BindModel();
                if (dtFGItemCode.Rows.Count > 0)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
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
                    blobj = new BL_WIPAccessoriesVerification();
                    DataTable dt = blobj.Bind_Model_Mapping_Accessories(
                        Convert.ToString(ddlModel_Name.SelectedItem.Text), Session["SiteCode"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        gvModel.Visible = true;
                        gvModel.DataSource = dt;
                        gvModel.DataBind();
                        ViewState["DATA"] = dt;
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No key parts are mapped against selected model or color, Please map in model key mapping module.", msgerror, CommonHelper.MessageType.Error.ToString());
                        gvModel.Visible = false;
                        lblModelName.Text = string.Empty;
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
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                foreach (GridViewRow row in gvModel.Rows)
                {
                    row.ControlStyle.BackColor = System.Drawing.Color.White;
                }

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

                blobj = new BL_WIPAccessoriesVerification();
                DataTable dt = new DataTable();
                dt = blobj.blScanIMEI(ddlModel_Name.SelectedItem.Text.ToString().Trim(), Session["SiteCode"].ToString(), txtScanHere.Text.Trim());

                if (dt.Rows.Count > 0)
                {
                    Message = dt.Rows[0][0].ToString();
                    if (Message.StartsWith("SUCCESS~"))
                    {
                        CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                        CommonHelper.ShowMessage("Valid Barcode", msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtAccessoriesBarcode.Text = string.Empty;
                        txtAccessoriesBarcode.Focus();
                        return;
                    }
                    else
                    {
                        if (Message.StartsWith("2~"))
                        {
                            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                            string[] sArr = dt.Rows[0][0].ToString().Split('~')[1].Split(',');

                            for (int i = 0; i < sArr.Length; i++)
                            {
                                string sAccName = sArr[i].ToString().Split('^')[0].Trim().ToUpper();
                                string sAccBarcode = sArr[i].ToString().Split('^')[1].Trim();
                                int isAccVerified = Convert.ToInt32(sArr[i].ToString().Split('^')[2].Trim());

                                foreach (GridViewRow row in gvModel.Rows)
                                {
                                    Label lblAccessoriesName = row.FindControl("lblAccessName") as Label;
                                    string sLblAccName = lblAccessoriesName.Text.Trim().ToUpper();

                                    if (sLblAccName != sAccName)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        row.Cells[5].Text = sAccBarcode;

                                        if (isAccVerified == 1)
                                        {
                                            row.ControlStyle.BackColor = System.Drawing.Color.Green;
                                            row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                        }
                                    }
                                }
                            }

                            foreach (GridViewRow row in gvModel.Rows)
                            {
                                if (string.IsNullOrWhiteSpace(row.Cells[5].Text.ToString().Trim()) || row.Cells[5].Text.Contains("Not Bind"))
                                {
                                    row.Cells[5].Text = "Not Bind";
                                    row.ControlStyle.Font.Bold = true;
                                    row.ControlStyle.BackColor = System.Drawing.Color.MediumVioletRed;
                                    row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                }
                            }

                            if (sArr.Length != gvModel.Rows.Count)
                            {
                                CommonHelper.ShowMessage($"All accessories are not bind. Please scan all the accessories in Accessory Scanning module", msgerror, CommonHelper.MessageType.Error.ToString());
                                txtAccessoriesBarcode.ReadOnly = true;
                                txtAccessoriesBarcode.Text = string.Empty;
                                txtScanHere.Text = string.Empty;
                                txtScanHere.Focus();
                                return;
                            }
                            else
                            {
                                txtAccessoriesBarcode.ReadOnly = false;
                                txtAccessoriesBarcode.Text = string.Empty;
                                txtAccessoriesBarcode.Focus();
                            }
                        }
                        else
                        {
                            CommonHelper.ShowMessage(Message.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtScanHere.Text = string.Empty;
                            txtScanHere.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtScanHere.Text = string.Empty;
                    txtScanHere.Focus();
                    return;
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
                int iVerifiedAccCount = 0;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
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

                string sAccBarcode = txtAccessoriesBarcode.Text.Trim();

                blobj = new BL_WIPAccessoriesVerification();
                DataTable dt = new DataTable();
                dt = blobj.blScanAccessories(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(),
                    txtScanHere.Text.Trim(), Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(),
                    sAccBarcode, Session["UserID"].ToString());

                if (dt.Rows.Count > 0)
                {
                    Message = dt.Rows[0][0].ToString();
                    if (Message.StartsWith("SUCCESS~"))
                    {
                        CommonHelper.ShowMessage("Accessory verified successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());

                        string sAccName = Message.Split('~')[1].Trim().ToUpper();
                        foreach (GridViewRow row in gvModel.Rows)
                        {
                            Label lblAccessoriesName = row.FindControl("lblAccessName") as Label;
                            if (lblAccessoriesName.Text.Trim().ToUpper() == sAccName)
                            {
                                row.ControlStyle.BackColor = System.Drawing.Color.Green;
                                row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                row.Cells[5].Text = txtAccessoriesBarcode.Text.Trim();
                            }

                            if (row.ControlStyle.BackColor == System.Drawing.Color.Green)
                            {
                                iVerifiedAccCount++;
                            }
                        }

                        if (iVerifiedAccCount == gvModel.Rows.Count)
                        {
                            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                            CommonHelper.ShowMessage("All the Accessories barcode are verified", msgsuccess, CommonHelper.MessageType.Success.ToString());
                            bindModelKeys();
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtScanHere.Text = string.Empty;
                            txtScanHere.Focus();
                            return;
                        }
                        else
                        {
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                        }
                    }
                    else
                    {
                        CommonHelper.ShowMessage(Message.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        txtAccessoriesBarcode.Text = "";
                        txtAccessoriesBarcode.Focus();
                        if (Message.StartsWith("0~"))
                        {
                            txtAccessoriesBarcode.Text = string.Empty;
                            txtAccessoriesBarcode.Focus();
                        }
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtAccessoriesBarcode.Text = string.Empty;
                    txtAccessoriesBarcode.Focus();
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