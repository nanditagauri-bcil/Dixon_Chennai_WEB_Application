using BusinessLayer;
using Common;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DIXON.INE.Masters
{
    public partial class DDRPartMaster : System.Web.UI.Page
    {
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
                Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");
                if (Session["usertype"].ToString() != "ADMIN")
                {
                    string _strRights = CommonHelper.GetRights("DDR PART MASTER", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    ShowGridData();
                    BindFGItemCode();
                    ddlModel_Name.Focus();
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
                BL_DDRPartMaster blobj = new BL_DDRPartMaster();
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
                    txtBomPart.Text = string.Empty;
                    txtDDRPart.Text = string.Empty;
                    txtDDRDesc.Text = string.Empty;
                    txtBomPart.Focus();
                }
                else
                {
                    gvDDRMaster.Visible = false;
                    txtBomPart.Text = string.Empty;
                    txtDDRPart.Text = string.Empty;
                    txtDDRDesc.Text = string.Empty;
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
                _ResetField();
                ShowGridData();
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
                BL_DDRPartMaster dlobj = new BL_DDRPartMaster();
                dt = dlobj.GetDDR();
                if (dt.Rows.Count > 0)
                {
                    gvDDRMaster.DataSource = dt;
                    gvDDRMaster.DataBind();
                    lblNumberofRecords.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    gvDDRMaster.DataSource = null; ;
                    gvDDRMaster.DataBind();
                    lblNumberofRecords.Text = "0";
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            try
            {
                if (btnSave.Text == "Save")
                {
                    if (ddlModel_Name.SelectedIndex == 0)
                    {
                        CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtBomPart.Text = string.Empty;
                        txtDDRPart.Text = string.Empty;
                        txtDDRDesc.Text = string.Empty;
                        lblModelName.Text = string.Empty;
                        ddlModel_Name.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtBomPart.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please Enter BomPartCode", msginfo, CommonHelper.MessageType.Info.ToString());
                        txtBomPart.Focus();
                        txtBomPart.Text = string.Empty;
                        txtDDRPart.Text = string.Empty;
                        txtDDRDesc.Text = string.Empty;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtDDRPart.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please Enter DDRPartCode", msginfo, CommonHelper.MessageType.Info.ToString());
                        txtDDRPart.Focus();
                        txtDDRPart.Text = string.Empty;
                        txtDDRDesc.Text = string.Empty;
                        return;
                    }
                    BL_DDRPartMaster dlobj = new BL_DDRPartMaster();
                    string sResult = dlobj.SaveDDR(ddlModel_Name.SelectedItem.Text.Trim(), txtBomPart.Text.Trim(),
                                                   txtDDRPart.Text.Trim(), txtDDRDesc.Text.Trim(), Session["UserID"].ToString(),
                                                   Session["SiteCode"].ToString(), Session["LINECODE"].ToString(), hidUID.Value.Trim());
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            ShowGridData();
                            _ResetField();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        else
                        {
                            ShowGridData();
                            _ResetField();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        }
                    }
                    else
                    {
                        txtBomPart.Text = "";
                        txtDDRPart.Text = "";
                        txtDDRDesc.Text = "";
                        CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                        return;
                    }
                }
                else
                {
                    //if (ddlModel_Name.SelectedIndex == 0)
                    //{
                    //    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    //    txtBomPart.Text = string.Empty;
                    //    txtDDRPart.Text = string.Empty;
                    //    txtDDRDesc.Text = string.Empty;
                    //    lblModelName.Text = string.Empty;
                    //    ddlModel_Name.Focus();
                    //    return;
                    //}
                    if (string.IsNullOrWhiteSpace(txtBomPart.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please Enter BomPartCode", msginfo, CommonHelper.MessageType.Info.ToString());
                        txtBomPart.Focus();
                        txtBomPart.Text = string.Empty;
                        txtDDRPart.Text = string.Empty;
                        txtDDRDesc.Text = string.Empty;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtDDRPart.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please Enter DDRPartCode", msginfo, CommonHelper.MessageType.Info.ToString());
                        txtDDRPart.Focus();
                        txtDDRPart.Text = string.Empty;
                        txtDDRDesc.Text = string.Empty;
                        return;
                    }
                    BL_DDRPartMaster dlobj = new BL_DDRPartMaster();
                    string sResult = dlobj.UpdateDDR(ddlModel_Name.SelectedItem.Text.Trim(), txtBomPart.Text.Trim(),
                                                   txtDDRPart.Text.Trim(), txtDDRDesc.Text.Trim(), Session["UserID"].ToString(),
                                                   Session["SiteCode"].ToString(), Session["LINECODE"].ToString(), hidUID.Value.Trim());
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            ShowGridData();
                            _ResetField();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        else
                        {
                            ShowGridData();
                            _ResetField();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        }
                    }
                    else
                    {
                        ShowGridData();
                        _ResetField();
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
            ddlModel_Name.Focus();
            ddlModel_Name.SelectedIndex = 0;
            ddlModel_Name.Enabled = true;
            ddlModel_Name.SelectedItem.Text = string.Empty;
            txtBomPart.Text = string.Empty;
            txtDDRPart.Text = string.Empty;
            txtDDRDesc.Text = string.Empty;
            lblModelName.Text = string.Empty;
            btnSave.Text = "Save";
            BindFGItemCode();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDDRMaster.PageIndex = e.NewPageIndex;
                ShowGridData();
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
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    int rowIndex = gvr.RowIndex;
                    string sFGItemCode = gvr.Cells[1].Text;
                    EditRecords(sFGItemCode, _SN);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        private void EditRecords(string sFGITEMCODE, string _SN)
        {
            try
            {
                BL_DDRPartMaster dlobj = new BL_DDRPartMaster();
                DataSet ds = dlobj.GetSeletedData(sFGITEMCODE, _SN);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlModel_Name.SelectedItem.Text = ds.Tables[0].Rows[0]["FG_ITEM_CODE"].ToString();
                        lblModelName.Text = ds.Tables[0].Rows[0]["MODEL_CODE"].ToString();
                        ddlModel_Name.Enabled = false;
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No FGITEMCODE found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        txtBomPart.Text = ds.Tables[1].Rows[0]["BOM_PART_CODE"].ToString();
                        txtDDRPart.Text = ds.Tables[1].Rows[0]["DDR_PART_CODE"].ToString();
                        txtDDRDesc.Text = ds.Tables[1].Rows[0]["DESCRIPTIONS"].ToString();
                        hidUpdate.Value = "Update";
                        hidUID.Value = _SN;
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No DETAILS found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No DDR details found.", msgerror, CommonHelper.MessageType.Error.ToString());
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
                BL_DDRPartMaster blobj = new BL_DDRPartMaster();
                string sResult = blobj.DeleteDDR(_SN);
                if (sResult.Length > 0)
                {
                    if (sResult.StartsWith("ERROR~"))
                    {
                        if (sResult.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                        {
                            CommonHelper.ShowMessage("DDR already in transaction, you can not delete it", msgerror, CommonHelper.MessageType.Error.ToString());
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
                    CommonHelper.ShowMessage("DDR already in transaction, you can not delete it", msgerror, CommonHelper.MessageType.Error.ToString());
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
            }
        }
    }
}