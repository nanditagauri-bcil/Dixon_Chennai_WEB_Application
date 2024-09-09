using BusinessLayer;
using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DIXON.INE.WIP
{
    public partial class WIPDeviceVsStandVerification : System.Web.UI.Page
    {
        BL_WIPAccessoriesVerification blobj;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");
                string sHeaderName = string.Empty;
                if (Session["usertype"].ToString() != "ADMIN")
                {
                    string _strRights = string.Empty;
                    _strRights = CommonHelper.GetRights("Device vs Stand Comparison", (DataTable)Session["USER_RIGHTS"]);
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

                //gvModel.DataSource = null;
                //gvModel.DataBind();
                ddlModel_Name.Items.Clear();
                //txtScanHere.Text = string.Empty;
                //txtAccessoriesBarcode.Text = string.Empty;
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
        protected void txtScanHere_TextChanged(object sender, EventArgs e)
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
                blobj = new BL_WIPAccessoriesVerification();
                DataTable dt = new DataTable();
                dt = blobj.blPcbScanDeviceVsStandVerify(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), txtScanHere.Text.Trim()
                    , Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(), Session["UserID"].ToString(), "", "PCBSCANSTANDVERIFY", ""
                    );
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().StartsWith("0~"))
                    {
                        CommonHelper.ShowMessage("PCB scanned successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtScanHere.Enabled = false;
                        txtDeviceQR.Focus();
                    }
                    else
                    {
                        CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        //txtDeviceQR.Enabled = false;
                        txtScanHere.Enabled = true;
                        txtScanHere.Text = "";
                        txtScanHere.Focus();
                        return;
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
        protected void txtScanDeviceQR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                string text = txtDeviceQR.Text.Replace("\n", "").Replace("\r", "");
                text = text + "\n";
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlModel_Name.Focus();
                    return;
                }
                string sSrNo = string.Empty;
                string sMacID = string.Empty;
                string sAccBarcode = txtDeviceQR.Text;
                // ADDED BY VIVEK 3 APR,2023
                if (!sAccBarcode.ToUpper().Trim().Contains("<SRNO>"))
                {
                    CommonHelper.ShowMessage("There is no root element for keyword <SRNO> in the scanned barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtStandQRCode.Text = string.Empty;
                    txtStandQRCode.Focus();
                    return;
                }

                if (sAccBarcode.ToUpper().Trim().StartsWith("<SRNO>") && sAccBarcode.ToUpper().Trim().EndsWith("</SRNO>"))
                {
                    sSrNo = Regex.Split(sAccBarcode.ToUpper(), "<SRNO>").Last().Split('<')[0];
                }
                else
                {
                    CommonHelper.ShowMessage("Please Scan Device QR", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtStandQRCode.Text = string.Empty;
                    txtStandQRCode.Focus();
                    return;
                }
                //FINISH

                blobj = new BL_WIPAccessoriesVerification();
                DataTable dt = new DataTable();
                dt = blobj.blPcbScanDeviceStandVerify(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), sSrNo
                    , Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(), Session["UserID"].ToString(), sMacID, "VALIDATEDEVICEVSStand",
                    txtScanHere.Text.Trim()
                    );
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().StartsWith("0~"))
                    {
                        CommonHelper.ShowMessage("PCB scanned successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtScanHere.Enabled = false;
                        txtDeviceQR.Enabled = false;
                        txtStandQRCode.Focus();
                    }
                    else
                    {
                        CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        //txtDeviceQR.Enabled = false;
                        txtDeviceQR.Text = "";
                        txtDeviceQR.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
               System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtDeviceQR.Text = "";
                txtDeviceQR.Focus();
            }
        }
        protected void txtScanStandQR_TextChanged(object sender, EventArgs e)
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

                if (string.IsNullOrEmpty(txtScanHere.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan PCB barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanHere.Text = "";
                    txtStandQRCode.Text = "";
                    txtScanHere.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtDeviceQR.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan device barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDeviceQR.Text = "";
                    txtStandQRCode.Text = "";
                    txtDeviceQR.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtStandQRCode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan Stand barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtStandQRCode.Text = "";
                    txtStandQRCode.Focus();
                    return;
                }

                string sSrNo = string.Empty;
                string sEanNo = string.Empty;
                string sMacID = string.Empty;
                string sAccBarcode = txtStandQRCode.Text;
                if (sAccBarcode.ToUpper().Contains("<SRNO>"))
                {
                    sSrNo = Regex.Split(sAccBarcode.ToUpper(), "<SRNO>").Last().Split('<')[0];
                }
                else
                {
                    CommonHelper.ShowMessage("There is no root element for keyword <SRNO> in the scanned barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDeviceQR.Text = string.Empty;
                    txtDeviceQR.Focus();
                    return;
                }

                blobj = new BL_WIPAccessoriesVerification();
                DataTable dt = new DataTable();
                dt = blobj.blPcbScanStandVerify(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), sSrNo

                    , Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(), Session["UserID"].ToString(), sMacID, "VALIDATEStand",
                    txtDeviceQR.Text.Trim(), txtStandQRCode.Text.Trim(), txtScanHere.Text.Trim()
                    );
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().StartsWith("0~"))
                    {
                        CommonHelper.ShowMessage("Stand verified successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());
                        Reset();
                        txtScanHere.Focus();
                    }
                    else
                    {
                        CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        //txtDeviceQR.Enabled = false;
                        txtStandQRCode.Text = string.Empty;
                        txtStandQRCode.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
               System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtStandQRCode.Text = string.Empty;
                txtStandQRCode.Focus();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            txtScanHere.Text = "";
            txtDeviceQR.Text = "";
            txtStandQRCode.Text = "";
            txtScanHere.Enabled = true;
            txtDeviceQR.Enabled = true;
            txtStandQRCode.Enabled = true;
        }
    }
}