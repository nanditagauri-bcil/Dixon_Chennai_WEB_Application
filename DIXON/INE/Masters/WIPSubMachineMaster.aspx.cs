using BusinessLayer;
using Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DIXON.INE.FG
{
    /// <summary>
    /// 
    /// </summary>
    public partial class WIPSubMachineMaster : System.Web.UI.Page
    {
        BL_SubMachineMaster blobj = new BL_SubMachineMaster();
        string Message = "";
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
                    string _strRights = CommonHelper.GetRights("SUB MACHINE MASTER", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    ShowGridData();
                    bindMachine();
                    drpMachineID.Focus();
                    //dvPrintergrup.Visible = true;
                    //if (PCommon.sUseNetworkPrinter == "1")
                    //{
                    //    bindPRINTER();
                    //}
                    //else
                    //{
                    //    dvPrintergrup.Visible = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return;
            }
        }

        private void bindMachine()
        {
            try
            {
                blobj = new BL_SubMachineMaster();
                DataTable dt = blobj.GetMachineID(Session["SiteCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(drpMachineID, dt, true);
                    drpMachineID.SelectedIndex = 0;
                    drpMachineID.Focus();
                }
                else
                {
                    drpMachineID.DataSource = null;
                    drpMachineID.DataBind();
                }
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
                blobj = new BL_SubMachineMaster();
                DataTable dt = blobj.GetSubMachineRecord(Session["SiteCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    gvMachinemst.DataSource = dt;
                    gvMachinemst.DataBind();
                    lblNumberofRecords.Text = dt.Rows.Count.ToString();

                    dt.TableName = "Table1";
                    dt.AcceptChanges();
                    System.Data.DataView view = new System.Data.DataView(dt);
                    System.Data.DataTable selected = view.ToTable("Table1", false, "MACHINEID", "SUBMACHINENAME");
                    clsCommon.FillComboBox(drpMachineFilter, selected, true);
                    ViewState["Data"] = dt;
                }
                else
                {
                    gvMachinemst.DataSource = null;
                    gvMachinemst.DataBind();
                    lblNumberofRecords.Text = "0";
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        public void save()
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            try
            {
                if (drpMachineID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select Machine ID.", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpMachineID.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSubMachineID.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter Sub Machine ID", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtSubMachineID.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSubMachinename.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter Sub Machine name.", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtSubMachinename.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtSubMachineDesc.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter Sub Machine Desc.", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtSubMachineDesc.Focus();
                    return;
                }
                else
                {
                    if (btnSave.Text == "Save")
                    {
                        blobj = new BL_SubMachineMaster();

                        //string Submachineid = chkAddChildMachine.Checked ? txtSubMachineID.Text.Trim() : string.Empty;
                        //string Submachinename = chkAddChildMachine.Checked ? txtSubMachineName.Text.Trim() : string.Empty;


                        string sResult = blobj.SaveSubMachineMstData(drpMachineID.SelectedItem.Value,
                            txtSubMachineID.Text.Trim(), txtSubMachinename.Text.Trim(), txtSubMachineDesc.Text.Trim(),
                            Session["UserID"].ToString(), Session["SiteCode"].ToString());
                        if (sResult.ToUpper().StartsWith("SUCCESS~"))
                        {
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                            txtSubMachinename.Text = "";
                            txtSubMachineDesc.Text = "";
                            txtSubMachineID.Text = "";
                            drpMachineID.SelectedIndex = 0;
                            drpMachineID.Focus();
                        }
                        else if (sResult.ToUpper().StartsWith("N~"))
                        {
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                            txtSubMachinename.Text = "";
                            txtSubMachineDesc.Text = "";
                            txtSubMachineID.Text = "";
                            drpMachineID.SelectedIndex = 0;
                            drpMachineID.Focus();
                        }
                        //else if (sResult.ToUpper().StartsWith("SQ~"))
                        //{
                        //    CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        //    drpMachineSequence.Focus();
                        //}
                    }
                    else
                    {
                        blobj = new BL_SubMachineMaster();
                        string sResult = blobj.UpdateSubMachineRecords(drpMachineID.SelectedItem.Value,
                            txtSubMachineID.Text.Trim(), txtSubMachinename.Text.Trim(), txtSubMachineDesc.Text.Trim(),
                            Session["UserID"].ToString(), Session["SiteCode"].ToString());
                        if (sResult.ToUpper().StartsWith("SUCCESS~"))
                        {
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                            txtSubMachinename.Text = "";
                            txtSubMachineDesc.Text = "";
                            txtSubMachineID.Text = "";
                            drpMachineID.SelectedIndex = 0;
                            drpMachineID.Focus();
                        }
                        else
                        {
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                            txtSubMachinename.Text = "";
                            txtSubMachineDesc.Text = "";
                            txtSubMachineID.Text = "";
                            drpMachineID.SelectedIndex = 0;
                            drpMachineID.Focus();
                        }
                        txtSubMachineID.ReadOnly = false;
                        btnSave.Text = "Save";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return;
            }
            finally
            {
                ShowGridData();

            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSubMachineID.Text = "";
            txtSubMachinename.Text = "";
            txtSubMachineDesc.Text = "";
            drpMachineID.Enabled = true;
            drpMachineID.SelectedIndex = 0;
            drpMachineID.Focus();
            btnSave.Text = "Save";
            txtSubMachineID.ReadOnly = false;
            if (drpMachineFilter.Items.Count > 0)
            {
                gvMachinemst.DataSource = null;
                gvMachinemst.DataBind();
                drpMachineFilter.SelectedIndex = 0;
                DataTable dt = (DataTable)ViewState["Data"];
                gvMachinemst.DataSource = dt;
                gvMachinemst.DataBind();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            save();
        }

        protected void txtMachineID_TextChanged(object sender, EventArgs e)
        {
            txtSubMachinename.Focus();
        }

        protected void txtMachineName_TextChanged(object sender, EventArgs e)
        {
            txtSubMachineDesc.Focus();
        }

        protected void txtMachineDesc_TextChanged(object sender, EventArgs e)
        {
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvMachinemst.PageIndex = e.NewPageIndex;
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
                string _sUserID = string.Empty;
                string _SiteCode = string.Empty;
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
                blobj = new BL_SubMachineMaster();
                DataTable dt = blobj.GetSeletedData(_SN, Session["SiteCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    drpMachineID.CssClass = "form-control select2";
                    drpMachineID.Enabled = false;
                    txtSubMachineID.ReadOnly = true;
                    string sMachineType = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        drpMachineID.Text = dr.ItemArray[0].ToString();
                        txtSubMachineID.Text = dr.ItemArray[1].ToString();
                        txtSubMachinename.Text = dr.ItemArray[2].ToString();
                        txtSubMachineDesc.Text = dr.ItemArray[3].ToString();

                    }
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
                blobj = new BL_SubMachineMaster();
                string sResult = blobj.DeleteSubMachine(_SN, Session["SiteCode"].ToString());
                drpMachineID.Enabled = false;
                if (sResult.StartsWith("SUCCESS~"))
                {
                    CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtSubMachinename.Text = "";
                    txtSubMachineDesc.Text = "";
                    txtSubMachineID.Text = "";
                    drpMachineID.SelectedIndex = 0;
                    drpMachineID.Focus();
                }
                else
                {
                    if (sResult.Contains("The DELETE statement conflicted with the REFERENCE"))
                    {
                        CommonHelper.ShowMessage("Machine already used in transaction, It can not delete", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                    else
                    {
                        CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                    txtSubMachinename.Text = "";
                    txtSubMachineDesc.Text = "";
                    txtSubMachineID.Text = "";
                    drpMachineID.SelectedIndex = 0;
                    drpMachineID.Focus();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
            finally
            {
                ShowGridData();
            }
        }

        protected void drpMachineFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpMachineFilter.SelectedIndex > 0)
                {
                    DataTable dt = (DataTable)ViewState["Data"];
                    DataView dataView = dt.DefaultView;
                    dataView.RowFilter = "MACHINEID = '" + drpMachineFilter.SelectedValue + "'";
                    gvMachinemst.DataSource = dataView;
                    gvMachinemst.DataBind();
                }
                else
                {
                    DataTable dt = (DataTable)ViewState["Data"];
                    gvMachinemst.DataSource = dt;
                    gvMachinemst.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        string sProgramID = string.Empty;
        //        if (Path.GetExtension(FileUpload1.FileName).Equals(".csv"))
        //        {
        //            DirectoryInfo _dir = null;
        //            string CSVFilePath = Path.GetFileName(FileUpload1.FileName);    //getting full file path of Uploaded file  
        //            string sUploadFilePath = ConfigurationManager.AppSettings["Upload_File_Path"].ToString();
        //            string sPath = Server.MapPath("~/" + sUploadFilePath + "//");
        //            _dir = new DirectoryInfo(sPath);
        //            if (_dir.Exists == false)
        //            {
        //                _dir.Create();
        //                Directory.CreateDirectory(_dir.ToString());
        //            }
        //            FileUpload1.SaveAs(Server.MapPath("~/Upload_File//") + CSVFilePath);
        //            string sFileName = Server.MapPath("~/Upload_File//") + CSVFilePath;
        //            dt = PCommon.ConvertCSVtoDataTable(sFileName);
        //            if (dt.Columns.Count != 8)
        //            {
        //                CommonHelper.ShowMessage("Invalid file,No of columns should be 8", msgerror, CommonHelper.MessageType.Error.ToString());
        //                return;
        //            }
        //            BL_MachineMaster blobj = new BL_MachineMaster();
        //            string Message = string.Empty;
        //            DataTable dtResult = blobj.UploadMachine(dt, Session["UserID"].ToString());
        //            string sResult = string.Empty;
        //            if (dtResult.Rows.Count > 0)
        //            {
        //                sResult = dtResult.Rows[0][0].ToString();
        //            }
        //            if (sResult.ToUpper().StartsWith("SUCCESS"))
        //            {
        //                Message = sResult.Split('~')[1];
        //                CommonHelper.ShowMessage(Message, msgsuccess, CommonHelper.MessageType.Success.ToString());
        //                return;
        //            }
        //            else if (sResult.StartsWith("N~"))
        //            {
        //                Message = sResult.Split('~')[1];
        //                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
        //                return;
        //            }
        //            else if (sResult.StartsWith("SQ~"))
        //            {
        //                Message = sResult.Split('~')[1];
        //                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
        //                return;
        //            }
        //            else if (sResult.StartsWith("ERROR~"))
        //            {
        //                if (sResult.ToUpper().Contains("PRIMARY"))
        //                {
        //                    Message = "Data in selected file already uploaded in database, Please upload fresh record only";
        //                }
        //                else if (sResult.ToUpper().Contains("FOREIGN KEY"))
        //                {
        //                    Message = "Data in selected file not found in master tables, Please check master data before upload";
        //                }
        //                else
        //                {
        //                    Message = sResult.Split('~')[1];
        //                }
        //                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
        //            }
        //            else if (sResult.StartsWith("NOTFOUND~"))
        //            {
        //                Message = sResult.Split('~')[1];
        //                CommonHelper.ShowMessage(Message, msgerror, CommonHelper.MessageType.Error.ToString());
        //            }
        //        }
        //        else
        //        {
        //            CommonHelper.ShowMessage("Please select valid(.csv) file only", msgerror, CommonHelper.MessageType.Error.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
        //        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //    }
        //}

        //protected void chkAddChildMachine_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if(chkAddChildMachine.Checked)
        //        {
        //            SubMachineIDBox.Visible = true;
        //        }
        //        else
        //        {
        //            SubMachineIDBox.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
        //        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //    }
        //}
    }
}