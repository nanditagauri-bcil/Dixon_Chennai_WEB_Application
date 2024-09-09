using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace DIXON.INE.WIP
{
    public partial class WIPCastingLabelMapping : System.Web.UI.Page
    {
        BL_WIPCastingLabelMapping blobj = new BL_WIPCastingLabelMapping();
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
                    _strRights = CommonHelper.GetRights("WIP Casting Label Mapping", (DataTable)Session["USER_RIGHTS"]);
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

                blobj = new BL_WIPCastingLabelMapping();
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
                    txtSubPCBID.Text = string.Empty;
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
                gvModel.Visible = false;
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
                blobj = new BL_WIPCastingLabelMapping();
                if (txtScanMachineID.Text.Length > 0)
                {
                    string sResult = string.Empty;
                    DataTable dtFGItemCode = blobj.BindFGItemCode(txtScanMachineID.Text.Trim()
                         , Session["SiteCode"].ToString(), Session["LINECODE"].ToString());
                    if (dtFGItemCode.Rows.Count > 0)
                    {
                        clsCommon.FillMultiColumnsCombo(ddlModel_Name, dtFGItemCode, true);
                        ddlModel_Name.SelectedIndex = 0;
                        ddlModel_Name.Focus();
                    }
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
                    txtSubPCBID.Text = string.Empty;
                    txtpcbBarcode.Focus();
                    bindSubPCBID();
                }
                else
                {
                    gvModel.Visible = false;
                    txtpcbBarcode.Text = string.Empty;
                    txtSubPCBID.Text = string.Empty;
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

        private void bindSubPCBID()
        {
            try
            {
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    blobj = new BL_WIPCastingLabelMapping();
                    DataTable dt = blobj.BindSubPCBID(Convert.ToString(ddlModel_Name.SelectedItem.Text),
                        Session["SiteCode"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        string keyVal1 = dt.Rows[0]["KEY_VAL1"].ToString();
                        string[] PCBIDsArray = keyVal1.Split(',').Select(s => s.Trim()).ToArray();

                        DataTable newDt = new DataTable();
                        newDt.Columns.Add("ACCNAME", typeof(string)); // Assuming PCBID is the column name
                        newDt.Columns.Add("KEY_VAL1", typeof(string));
                        newDt.Columns.Add("SCANBARCODE", typeof(string));
                        int counter = 1;
                        // Add rows to the new DataTable
                        foreach (string PCBID in PCBIDsArray)
                        {
                            DataRow newRow = newDt.NewRow();
                            newRow["ACCNAME"] = $"SUB_PCB{counter}";
                            newRow["KEY_VAL1"] = PCBID;
                            newDt.Rows.Add(newRow);
                            counter++;
                        }

                        // Bind the new DataTable to gvModel
                        gvModel.Visible = true;
                        gvModel.DataSource = newDt;
                        gvModel.DataBind();
                        ViewState["DATA"] = newDt;
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
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                txtScanMachineID.Focus();
                txtScanMachineID.Text = string.Empty;
                lblMachineName.Text = string.Empty;
                lblModelNo.Text = string.Empty;
                ddlModel_Name.SelectedIndex = 0;
                lblModelName.Text = string.Empty;
                lblmachinetype.Text = string.Empty;
                txtpcbBarcode.Text = string.Empty;
                txtSubPCBID.Text = string.Empty;
                gvModel.Visible = false;
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
                    CommonHelper.ShowMessage("Please Scan Casting Label MachineID", msgerror, CommonHelper.MessageType.Error.ToString());
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
                blobj = new BL_WIPCastingLabelMapping();
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

                        if (ds.Tables[1].Rows.Count > 0)
                        {

                            for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                            {
                                string subpcbid = ds.Tables[1].Rows[i]["SUB_PCBID"].ToString();
                                foreach (GridViewRow row in gvModel.Rows)
                                {
                                    int index = 0;
                                    Label lblKeyValue = row.FindControl("lblKeyValue") as Label;
                                    if (subpcbid.Trim().StartsWith(lblKeyValue.Text.Trim().ToUpper()))
                                    {
                                        index = 1;
                                    }
                                    if (index == 1)
                                    {
                                        if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                                        {
                                            row.ControlStyle.BackColor = System.Drawing.Color.Green;
                                            row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                            row.Cells[3].Text = subpcbid;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        return;
                    }
                    else if (sResult.StartsWith("N~"))
                    {
                        CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
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

        private void BindSubPCBIDGrid()
        {
            try
            {
                blobj = new BL_WIPCastingLabelMapping();
                DataTable dt = new DataTable();
                dt = blobj.GetData(ddlModel_Name.SelectedItem.Text.Trim(),
                            Session["SiteCode"].ToString(), txtpcbBarcode.Text.Trim(), Session["LineCode"].ToString(),
                            ddlModel_Name.SelectedItem.Text.Trim(), txtSubPCBID.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string subpcbid = dt.Rows[i]["SUBPCBID"].ToString();
                        foreach (GridViewRow row in gvModel.Rows)
                        {
                            int index = 0;
                            Label lblKeyValue = row.FindControl("lblKeyValue") as Label;
                            if (subpcbid.Trim().StartsWith(lblKeyValue.Text.Trim().ToUpper()))
                            {
                                index = 1;
                            }
                            if (index == 1)
                            {
                                row.ControlStyle.BackColor = System.Drawing.Color.Green;
                                row.ControlStyle.ForeColor = System.Drawing.Color.White;
                                row.Cells[3].Text = subpcbid;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    gvModel.DataSource = null;
                    gvModel.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        protected void txtSubPCBID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                int iVerifiedKeyCount = 0;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (string.IsNullOrWhiteSpace(txtScanMachineID.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan Casting Label MachineID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanMachineID.Focus();
                    ddlModel_Name.SelectedIndex = 0;
                    txtpcbBarcode.Text = string.Empty;
                    txtSubPCBID.Text = string.Empty;
                    return;
                }
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtpcbBarcode.Text = string.Empty;
                    txtSubPCBID.Text = string.Empty;
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtpcbBarcode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan PCB barcode", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtpcbBarcode.Focus();
                    txtSubPCBID.Text = string.Empty;
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSubPCBID.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan Sub PCBID", msginfo, CommonHelper.MessageType.Info.ToString());
                    txtSubPCBID.Focus();
                    return;
                }
                DataTable dtRigthsData = new DataTable();
                dtRigthsData.Columns.Add("ACCNAME");
                dtRigthsData.Columns.Add("KEYVALUE");
                dtRigthsData.Columns.Add("SUBPCBID");

                foreach (GridViewRow row in gvModel.Rows)
                {
                    Label lblKeyValue = row.FindControl("lblKeyValue") as Label;
                    if (txtSubPCBID.Text.Trim().StartsWith(lblKeyValue.Text.Trim().ToUpper()))
                    {
                        if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                        {
                            index = 1;
                        }
                    }
                    if (index == 1)
                    {
                        blobj = new BL_WIPCastingLabelMapping();
                        DataTable dt = blobj.ValidateScanSubPCBID(txtScanMachineID.Text.Trim(), ddlModel_Name.SelectedValue.ToString(),
                            Session["SiteCode"].ToString(), txtpcbBarcode.Text.Trim(), Session["LineCode"].ToString(),
                            ddlModel_Name.SelectedItem.Text.Trim(), txtSubPCBID.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            string sResult = dt.Rows[0][0].ToString();
                            Message = sResult.Split('~')[1];
                            if (sResult.StartsWith("SUCCESS"))
                            {
                                if (row.ControlStyle.BackColor != System.Drawing.Color.Green)
                                {
                                    BindSubPCBIDGrid();
                                    index = 2;
                                    txtSubPCBID.Text = string.Empty;
                                    txtSubPCBID.Focus();
                                    break;
                                }
                                else
                                {
                                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                                    CommonHelper.ShowMessage("SUB PCBID : " + row.Cells[3].Text + " Scanned against the keyvalue : "
                                        + row.Cells[2].Text + " ,Please scanned another Sub PCBID", msgerror, CommonHelper.MessageType.Success.ToString());
                                    txtSubPCBID.Text = string.Empty;
                                    txtSubPCBID.Focus();
                                    bindSubPCBID();
                                    BindSubPCBIDGrid();
                                    return;
                                }
                            }
                            else if (sResult.StartsWith("N~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtSubPCBID.Text = string.Empty;
                                txtSubPCBID.Focus();
                                return;
                            }
                            else if (sResult.StartsWith("ERROR~"))
                            {
                                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                                txtSubPCBID.Text = string.Empty;
                                txtSubPCBID.Focus();
                            }
                        }
                        else
                        {
                            CommonHelper.ShowMessage("No result found for scanned sub PCBID Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                            txtSubPCBID.Text = string.Empty;
                            txtSubPCBID.Focus();
                        }
                    }
                }
                if (index == 0)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    CommonHelper.ShowMessage("Sub PCBID : " + txtSubPCBID.Text.Trim() + " not start with any key value/mapped in grid", msgerror, CommonHelper.MessageType.Success.ToString());
                    txtSubPCBID.Text = string.Empty;
                    txtSubPCBID.Focus();
                    bindSubPCBID();
                    return;
                }
                foreach (GridViewRow row in gvModel.Rows)
                {
                    if (row.ControlStyle.BackColor == System.Drawing.Color.Green)
                    {
                        string cell1Text = ((Label)row.FindControl("lblAccessName")).Text.Trim();
                        string cell2Text = ((Label)row.FindControl("lblKeyValue")).Text.Trim();
                        dtRigthsData.Rows.Add(cell1Text, cell2Text, row.Cells[3].Text.Trim());
                        iVerifiedKeyCount++;
                    }
                }

                if (iVerifiedKeyCount == gvModel.Rows.Count)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    blobj = new BL_WIPCastingLabelMapping();
                    DataTable dt = blobj.SaveScanSubPCBID(txtScanMachineID.Text.Trim(), ddlModel_Name.SelectedValue.ToString(),
                        Session["SiteCode"].ToString(), txtpcbBarcode.Text.Trim(), Session["LineCode"].ToString(),
                        ddlModel_Name.SelectedItem.Text.Trim(), Session["UserID"].ToString()
                        , dtRigthsData);

                    if (dt.Rows.Count > 0)
                    {
                        string sResult = dt.Rows[0][0].ToString();
                        Message = sResult.Split('~')[1];
                        if (sResult.StartsWith("SUCCESS"))
                        {
                            bindSubPCBID();
                            CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
                            txtpcbBarcode.Focus();
                            txtpcbBarcode.Text = string.Empty;
                            txtSubPCBID.Text = string.Empty;
                            iVerifiedKeyCount = 0;

                        }
                        else if (sResult.StartsWith("N~"))
                        {
                            CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                            txtSubPCBID.Text = string.Empty;
                            txtSubPCBID.Focus();
                            bindSubPCBID();

                        }
                        else if (sResult.StartsWith("ERROR~"))
                        {
                            CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
                            txtSubPCBID.Text = string.Empty;
                            txtSubPCBID.Focus();
                            bindSubPCBID();

                        }
                        foreach (GridViewRow row in gvModel.Rows)
                        {
                            row.Cells[3].Text = string.Empty;

                        }
                        return;
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No result found for scanned Wave Pallet Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtSubPCBID.Text = string.Empty;
                        txtSubPCBID.Focus();
                        bindSubPCBID();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtSubPCBID.Text = string.Empty;
                txtSubPCBID.Focus();
                bindSubPCBID();
            }
        }
    }
}