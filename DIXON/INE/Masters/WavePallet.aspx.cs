using BusinessLayer;
using Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DIXON.INE.Masters
{
    public partial class WavePallet : System.Web.UI.Page
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
                    string _strRights = CommonHelper.GetRights("LINE MASTER", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    ShowGridData();
                    txtWavePalletID.Focus();
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
                    if (txtWavePalletID.Text.Trim() == "")
                    {
                        CommonHelper.ShowMessage("Please enter Wave Pallet code", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtWavePalletID.Focus();
                        return;
                    }
                    if (txtWavePalletName.Text.Trim() == "")
                    {
                        CommonHelper.ShowMessage("Please enter Wave Pallet name", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtWavePalletName.Focus();
                        return;
                    }

                    BL_WavePalletMaster dlobj = new BL_WavePalletMaster();
                    string sResult = dlobj.SaveWavePallet(txtWavePalletID.Text.Trim(), txtWavePalletName.Text.Trim(), txtWavePalletDesc.Text.Trim(), Session["UserID"].ToString());
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            txtWavePalletID.Text = "";
                            txtWavePalletName.Text = "";
                            txtWavePalletDesc.Text = "";

                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        if (sResult.StartsWith("N~"))
                        {
                            txtWavePalletID.Text = "";
                            txtWavePalletName.Text = "";
                            txtWavePalletDesc.Text = "";

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
                        txtWavePalletID.Text = "";
                        txtWavePalletName.Text = "";
                        txtWavePalletDesc.Text = "";

                        CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
                else
                {
                    if (txtWavePalletName.Text == "")
                    {
                        CommonHelper.ShowMessage("Please enter Pallet name.", msgerror, CommonHelper.MessageType.Error.ToString());
                        return;
                    }
                    BL_WavePalletMaster blobj = new BL_WavePalletMaster();
                    string sResult = blobj.UpdateWavePallet(txtWavePalletName.Text.Trim(), txtWavePalletDesc.Text.Trim(), hidUID.Value.Trim());
                    if (sResult.Length > 0)
                    {
                        if (sResult.StartsWith("ERROR~"))
                        {
                            CommonHelper.ShowMessage(sResult.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        }
                        if (sResult.StartsWith("N~"))
                        {
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
                        CommonHelper.ShowMessage("No result found.", msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    CommonHelper.ShowMessage("Wave Pallet id already exists.", msgerror, CommonHelper.MessageType.Error.ToString());
                    return;
                }
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }

        }


        private void _ResetField()
        {
            txtWavePalletID.Focus();
            txtWavePalletID.Text = string.Empty;
            txtWavePalletName.Text = string.Empty;
            txtWavePalletDesc.Text = string.Empty;
            btnSave.Text = "Save";
            txtWavePalletID.ReadOnly = false;
        }
        private void ShowGridData()
        {
            try
            {
                DataTable dt = new DataTable();
                BL_WavePalletMaster dlobj = new BL_WavePalletMaster();
                dt = dlobj.GetWavePallet();
                if (dt.Rows.Count > 0)
                {
                    gvLineMaster.DataSource = dt;
                    gvLineMaster.DataBind();
                    lblNumberofRecords.Text = dt.Rows.Count.ToString();
                }
                else
                {
                    gvLineMaster.DataSource = null; ;
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvLineMaster.PageIndex = e.NewPageIndex;
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
                BL_WavePalletMaster dlobj = new BL_WavePalletMaster();
                DataTable dtUserDetails = dlobj.GetWavePalletByID(_SN);
                if (dtUserDetails.Rows.Count > 0)
                {
                    txtWavePalletID.Text = dtUserDetails.Rows[0]["WAVEPALLETID"].ToString();
                    txtWavePalletDesc.Text = dtUserDetails.Rows[0]["WAVEPALLETDESC"].ToString();
                    txtWavePalletName.Text = dtUserDetails.Rows[0]["WAVEPALLETNAME"].ToString();
                    hidUpdate.Value = "Update";
                    hidUID.Value = _SN;
                    txtWavePalletID.ReadOnly = true;
                }
                else
                {
                    CommonHelper.ShowMessage("No Wave Pallet details found.", msgerror, CommonHelper.MessageType.Error.ToString());
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
                BL_WavePalletMaster blobj = new BL_WavePalletMaster();
                string sResult = blobj.DeleteWavePalletid(_SN);
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
            }
        }

    }
}