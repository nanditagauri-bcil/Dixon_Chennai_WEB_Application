using BusinessLayer;
using Common;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace DIXON.INE.Masters
{
    public partial class ApplicationSetting : System.Web.UI.Page
    {
        string Message = string.Empty;
        bool valid = true;
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
                    string _strRights = CommonHelper.GetRights("APP SETTING MASTER", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    ShowGridData();
                    //TEXTBOXENABLEDISABLE();
                    GET_FGITEMCODE();
                    ddlFgItemCode.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        public void GET_FGITEMCODE()
        {

            try
            {
                BL_ApplicationSetting dlobj = new BL_ApplicationSetting();
                DataTable dt = dlobj.BindFGITEMCOE();
                if (dt.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(ddlFgItemCode, dt, true);
                    ddlFgItemCode.SelectedIndex = 0;
                    ddlFgItemCode.Focus();
                    return;
                }
                else
                {
                    CommonHelper.ShowMessage("No Fg item code found", msgerror, CommonHelper.MessageType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }

        //private void TEXTBOXENABLEDISABLE()
        //{
        //    try
        //    { 
        //        txtmachinetestcount.Enabled = false;
        //        txtreworkinoutmaxcount.Enabled = false;
        //        txtreworkoutmaxtime.Enabled = false;
        //        txtreworkinmintime.Enabled = false;
        //        if (lblNumberofRecords.Text.Trim() != "1")
        //        {
        //            txtmachinetestcount.Enabled = true;
        //            txtreworkinoutmaxcount.Enabled = true;
        //            txtreworkoutmaxtime.Enabled = true;
        //            txtreworkinmintime.Enabled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //        CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
        //    }
        //}
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                _ResetField();
                ShowGridData();
                //TEXTBOXENABLEDISABLE();
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
                string fgitemcode = string.Empty;
                if (btnSave.Text == "Save")
                {
                    if (ddlFgItemCode.SelectedIndex > 0)
                    {
                        fgitemcode = ddlFgItemCode.SelectedValue.ToString().Trim();
                    }
                    CHECKVALIDATION();
                    if (valid == false)
                    {
                        return;
                    }
                    BL_ApplicationSetting dlobj = new BL_ApplicationSetting();
                    string sResult = dlobj.SaveAppSetting(txtmachinetestcount.Text.Trim(),
                        txtreworkinoutmaxcount.Text.Trim(), txtreworkoutmaxtime.Text.Trim(),
                        txtreworkinmintime.Text.Trim(), Session["UserID"].ToString(), fgitemcode);
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            ShowGridData();
                            _ResetField();
                            //TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        else
                        {
                            ShowGridData();
                            _ResetField();
                            //TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        }
                    }
                    else
                    {
                        ShowGridData();
                        _ResetField();
                        //TEXTBOXENABLEDISABLE();
                        CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
                else
                {
                    CHECKVALIDATION();
                    if (valid == false)
                    {
                        return;
                    }
                    BL_ApplicationSetting dlobj = new BL_ApplicationSetting();
                    string sResult = dlobj.UpdateAppSetting(txtmachinetestcount.Text.Trim(),
                        txtreworkinoutmaxcount.Text.Trim(), txtreworkoutmaxtime.Text.Trim(),
                        txtreworkinmintime.Text.Trim(), Session["UserID"].ToString(), hidUID.Value);
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            ShowGridData();
                            _ResetField();
                            //TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        else
                        {
                            ShowGridData();
                            _ResetField();
                            //TEXTBOXENABLEDISABLE();
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        }
                    }
                    else
                    {
                        ShowGridData();
                        _ResetField();
                        //TEXTBOXENABLEDISABLE();
                        CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }

        }

        private void CHECKVALIDATION()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtmachinetestcount.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter machine test Count", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtmachinetestcount.Focus();
                    valid = false;
                    return;
                }
                if (!IsValidOnlyNumericNumber(txtmachinetestcount.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter only numeric machine test Count", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtmachinetestcount.Focus();
                    valid = false;
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtreworkinoutmaxcount.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter reworkinout Maxlimit", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtreworkinoutmaxcount.Focus();
                    valid = false;
                    return;
                }
                if (!IsValidOnlyNumericNumber(txtreworkinoutmaxcount.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter only numeric reworkinout Maxlimit", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtreworkinoutmaxcount.Focus();
                    valid = false;
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtreworkoutmaxtime.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter reworkout Maxtime", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtreworkoutmaxtime.Focus();
                    valid = false;
                    return;
                }
                if (!IsValidOnlyNumericNumber(txtreworkoutmaxtime.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter only numeric reworkout Maxtime", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtreworkoutmaxtime.Focus();
                    valid = false;
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtreworkinmintime.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter reworkin Mintime", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtreworkinmintime.Focus();
                    valid = false;
                    return;
                }
                if (!IsValidOnlyNumericNumber(txtreworkinmintime.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please enter only numeric reworkin Mintime", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtreworkinmintime.Focus();
                    valid = false;
                    return;
                }

            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        static bool IsValidOnlyNumericNumber(string Number)
        {
            string numberPattern = @"^\d+$";
            Regex regex = new Regex(numberPattern);
            return regex.IsMatch(Number);
        }


        private void _ResetField()
        {
            ddlFgItemCode.SelectedIndex = 0;
            ddlFgItemCode.Focus();
            txtmachinetestcount.Text = string.Empty;
            txtreworkinoutmaxcount.Text = string.Empty;
            txtreworkoutmaxtime.Text = string.Empty;
            txtreworkinmintime.Text = string.Empty;
            btnSave.Text = "Save";
        }
        private void ShowGridData()
        {
            try
            {
                DataTable dt = new DataTable();
                BL_ApplicationSetting dlobj = new BL_ApplicationSetting();
                dt = dlobj.GetDATA();
                if (dt.Rows.Count > 0)
                {
                    gvAppMaster.DataSource = dt;
                    gvAppMaster.DataBind();
                    lblNumberofRecords.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    gvAppMaster.DataSource = null;
                    gvAppMaster.DataBind();
                    lblNumberofRecords.Text = "0";
                }
                ddlFgItemCode.Enabled = true;
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAppMaster.PageIndex = e.NewPageIndex;
                ShowGridData();
                //TEXTBOXENABLEDISABLE();
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
                BL_ApplicationSetting dlobj = new BL_ApplicationSetting();
                DataTable dt = dlobj.GetAppSettingDataByID(_SN);
                if (dt.Rows.Count > 0)
                {
                    txtmachinetestcount.Text = dt.Rows[0]["MACHINE_TEST_COUNT"].ToString();
                    txtreworkinoutmaxcount.Text = dt.Rows[0]["REWORKINOUT_MAX_LIMIT"].ToString();
                    txtreworkoutmaxtime.Text = dt.Rows[0]["REWORKOUT_MAXTIME"].ToString();
                    txtreworkinmintime.Text = dt.Rows[0]["REWORKIN_MINTIME"].ToString();
                    if (dt.Rows[0]["FG_ITEM_CODE"].ToString().Length > 0)
                    {
                        ddlFgItemCode.SelectedValue = dt.Rows[0]["FG_ITEM_CODE"].ToString();
                    }
                    ddlFgItemCode.Enabled = false;
                    hidUpdate.Value = "Update";
                    hidUID.Value = _SN;
                }
                else
                {
                    CommonHelper.ShowMessage("No details found.", msgerror, CommonHelper.MessageType.Error.ToString());
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
                BL_ApplicationSetting blobj = new BL_ApplicationSetting();
                string sResult = blobj.Deleteid(_SN);
                if (sResult.Length > 0)
                {
                    if (sResult.StartsWith("ERROR~"))
                    {
                        if (sResult.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                        {
                            CommonHelper.ShowMessage("Wave Pallet already in transaction, you can not delete it", msgerror, CommonHelper.MessageType.Error.ToString());
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
                        //TEXTBOXENABLEDISABLE();
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
                    CommonHelper.ShowMessage("Wave Pallet already in transaction, you can not delete it", msgerror, CommonHelper.MessageType.Error.ToString());
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
                //TEXTBOXENABLEDISABLE();
            }
        }

    }
}