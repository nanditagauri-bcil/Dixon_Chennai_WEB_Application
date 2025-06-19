using BusinessLayer.MES.PRINTING;
using Common;
using System;
using System.Data;
using System.Threading;

namespace DIXON.INE.MOB
{
    public partial class BindingWallMountKit : System.Web.UI.Page
    {
        BL_Binding_Wall_Mount_Kit blobj = new BL_Binding_Wall_Mount_Kit();
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
                    string _strRights = CommonHelper.GetRights("BINDING WALL MOUNT KIT", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    BindModelName();
                    lblWT.Text = "0";
                    lblTP.Text = "0";
                    lblTM.Text = "0";
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        private void BindModelName()
        {
            try
            {
                ddlModel_Name.Items.Clear();
                lbllastscanned.Text = string.Empty;
                blobj = new BL_Binding_Wall_Mount_Kit();
                DataTable dt = blobj.BindFGItemCode(Session["SiteCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    clsCommon.FillMultiColumnsCombo(ddlModel_Name, dt, true);
                    ddlModel_Name.SelectedIndex = 0;
                    ddlModel_Name.Focus();
                }
                else
                {
                    CommonHelper.ShowMessage("No data found.", msginfo, CommonHelper.MessageType.Info.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void ddlModel_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
        public void GetData()
        {
            try
            {
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    lbllastscanned.Text = string.Empty;
                    txtScanHere.Text = string.Empty;
                    DataTable dt = new DataTable();
                    blobj = new BL_Binding_Wall_Mount_Kit();
                    dt = blobj.DisplayedData(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        lblWT.Text = dt.Rows[0]["GrossWt"].ToString();
                        lblTP.Text = dt.Rows[0]["TolPlus"].ToString();
                        lblTM.Text = dt.Rows[0]["TolMinus"].ToString();
                        lblModelName.Text = ddlModel_Name.SelectedValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }
        protected void txtScanHere_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select FG ITEM CODE", msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtScanHere.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan Here", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanHere.Focus();
                    return;
                }
                blobj = new BL_Binding_Wall_Mount_Kit();
                DataTable dtResult = blobj.Scan_Barcode(txtScanHere.Text.Trim(), ddlModel_Name.SelectedItem.Text.Trim(),
                                     Session["SiteCode"].ToString(), Session["UserID"].ToString(),
                                     Session["LineCode"].ToString());
                Message = dtResult.Rows[0][0].ToString();
                if (Message.StartsWith("SUCCESS~"))
                {
                    CommonHelper.ShowMessage(Message.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    dvMessage.Visible = true;
                    GetData();
                }
                else
                {
                    CommonHelper.ShowMessage(Message.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    lbllastscanned.Text = txtScanHere.Text.Trim();
                    txtScanHere.Text = "";
                    txtScanHere.Focus();
                    dvMessage.Visible = false;
                    GetData();
                }
            }
            catch (Exception ex)
            {
                txtScanHere.Text = "";
                txtScanHere.Focus();
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                BindModelName();
                lbllastscanned.Text = string.Empty;
                txtScanHere.Text = string.Empty;
                lbllastscanned.Text = string.Empty;
                lblWT.Text = "0";
                lblTP.Text = "0";
                lblTM.Text = "0";
                lblModelName.Text = "";
                txtCapWT.Text = string.Empty;
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        protected void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select model", msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlModel_Name.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtScanHere.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please Scan Here", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanHere.Focus();
                    return;
                }
                blobj = new BL_Binding_Wall_Mount_Kit();
                DataTable dtResult = blobj.GetCaptureWeight(txtScanHere.Text.Trim(), ddlModel_Name.SelectedItem.Text.Trim(),
                                     Session["SiteCode"].ToString(), Session["UserID"].ToString(),
                                     Session["LineCode"].ToString());
                if (dtResult.Rows.Count == 0)
                {
                    CommonHelper.ShowMessage("Weight not found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    btnValidate.Focus();
                    return;
                }
                else
                {
                    Message = dtResult.Rows[0][0].ToString();
                    CommonHelper.ShowMessage(Message.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                    txtCapWT.Text = Message.Split('~')[1].ToString().Trim();
                }
                if (string.IsNullOrWhiteSpace(txtCapWT.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Wt not found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanHere.Focus();
                    return;
                }
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, "Weight result found, Capture weight :" + txtCapWT.Text.Trim()
                    + ", Tol + " + lblTM.Text + ", Tol -" + lblTP.Text + ", WALL MOUNT KIT Wt : " + lblWT.Text
                    + ", Barcode : " + txtScanHere.Text.Trim());
                double capWt = Convert.ToDouble(txtCapWT.Text.Trim());
                double tMinus = Convert.ToDouble(lblTM.Text);
                double tPlus = Convert.ToDouble(lblTP.Text);
                double WMwt = Convert.ToDouble(lblWT.Text);
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (capWt >= WMwt - tMinus && capWt <= WMwt + tPlus)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    string sStageCode = "WALLMOUNT_WEIGHT_MACHINE";
                    blobj = new BL_Binding_Wall_Mount_Kit();
                    DataTable dt = blobj.SaveDATA(txtScanHere.Text.Trim(), ddlModel_Name.SelectedItem.Text.Trim(),
                                         Session["SiteCode"].ToString(), Session["UserID"].ToString(), sStageCode,
                                         Session["LineCode"].ToString(), txtCapWT.Text.Trim());
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
                  System.Reflection.MethodBase.GetCurrentMethod().Name, "Weight result found, Save data in DB, Capture weight :" + txtCapWT.Text.Trim()
                  + ", Tol + " + lblTM.Text + ", Tol -" + lblTP.Text + ", WALL MOUNT KIT Wt : " + lblWT.Text
                  + ", Barcode : " + txtScanHere.Text.Trim()
                  );
                    if (dtResult.Rows.Count > 0)
                    {
                        Message = dtResult.Rows[0][0].ToString();
                        if (Message.StartsWith("SUCCESS~"))
                        {
                            CommonHelper.ShowMessage(Message.Split('~')[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                            dvMessage.Visible = false;
                            BL_LabelPrinting blobj = new BL_LabelPrinting();
                            PL.PL_Printing plob = new PL.PL_Printing();
                            plob.sBOMCode = ddlModel_Name.SelectedItem.Text.Trim();
                            plob.sColorCode = "";
                            plob.sCustomerCode = "";
                            plob.sPrinterIP = "";
                            plob.sModelName = lblModelName.Text;
                            plob.sSNBarcode = txtScanHere.Text.Trim();
                            plob.sSiteCode = Session["SiteCode"].ToString();
                            plob.sLineCode = Session["LINECODE"].ToString();
                            plob.sPrintedBy = Session["UserID"].ToString();
                            plob.sUserID = Session["UserID"].ToString();
                            plob.sStageCode = "9";
                            plob.sStageName = sStageCode;
                            string _Result = blobj.blLabelPrinting(plob);
                            Message = _Result;
                            if (Message.StartsWith("SUCCESS~"))
                            {
                                CommonHelper.ShowMessage("Scanned barcode successfully passed this stage", msgsuccess, CommonHelper.MessageType.Success.ToString());
                                lbllastscanned.Text = txtScanHere.Text.Trim();
                                txtScanHere.Text = "";
                                txtScanHere.Focus();
                            }
                            else
                            {
                                CommonHelper.ShowMessage(Message.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                                lbllastscanned.Text = txtScanHere.Text.Trim();
                                txtScanHere.Text = ""; txtScanHere.Focus();
                            }
                            txtScanHere.Enabled = false;
                            Thread.Sleep(2000);
                            txtScanHere.Enabled = true;
                        }
                        else
                        {
                            CommonHelper.ShowMessage(Message.Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                            lbllastscanned.Text = txtScanHere.Text.Trim();
                            txtScanHere.Text = "";
                            txtScanHere.Focus();
                        }
                    }
                    else
                    {
                        CommonHelper.ShowMessage("No result found, Please put weight again", msgerror, CommonHelper.MessageType.Error.ToString());
                        return;
                    }
                }
                else
                {
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtData,
                     System.Reflection.MethodBase.GetCurrentMethod().Name, "WT is not acureate, Capture weight :" + txtCapWT.Text.Trim()
                     + ", Tol + " + lblTM.Text + ", Tol -" + lblTP.Text + ", WALL MOUINT KIT Wt : " + lblWT.Text
                     + ", Barcode : " + txtScanHere.Text.Trim()
                     );
                    CommonHelper.ShowMessage("Wt is not accureate, Please put weight again", msgerror, CommonHelper.MessageType.Error.ToString());
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }
    }
}