using BusinessLayer;
using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace DIXON.INE.WIP
{
    public partial class WipDeviceVerification : System.Web.UI.Page
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
                    _strRights = CommonHelper.GetRights("Device vs GB Comparison", (DataTable)Session["USER_RIGHTS"]);
                    CommonHelper._strRights = _strRights.Split('^');
                    if (CommonHelper._strRights[0] == "0")  //Check view rights
                    {
                        Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                    }
                }
                if (!IsPostBack)
                {
                    BindFGItemCode();
                    lblDevice2QR.Visible = false;
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
                if (string.IsNullOrEmpty(txtScanHere.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan PCB barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDeviceQR.Text = ""; 
                    txtScanHere.Focus();
                    return;
                }
                if (txtDeviceQR.Text.Trim().ToUpper().Contains("<MRP>"))
                {
                    CommonHelper.ShowMessage("Please scan correct barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDeviceQR.Text = "";
                    txtDeviceQR.Focus();
                    return;
                }
                string sSrNo = string.Empty;
                string sEanNo = string.Empty;
                string sMacID = string.Empty;
                string sIMEI = string.Empty;
                string sEID = string.Empty;
                string sBTMAC = string.Empty;
                string sMODELCODE = string.Empty;
                string sAccBarcode = txtDeviceQR.Text;
                if (sAccBarcode.ToUpper().Contains("<SRNO>"))
                {
                    sSrNo = Regex.Split(sAccBarcode.ToUpper(), "<SRNO>").Last().Split('<')[0];
                    sMacID = Regex.Split(sAccBarcode.ToUpper(), "<MACID>").Last().Split('<')[0];
                    
                    //ADDED BY SHIVAM (20/03/2024)
                    sIMEI = Regex.Split(sAccBarcode.ToUpper(), "<IMEI>").Last().Split('<')[0];
                    sEID = Regex.Split(sAccBarcode.ToUpper(), "<EID>").Last().Split('<')[0];
                    sBTMAC = Regex.Split(sAccBarcode.ToUpper(), "<BT MAC>").Last().Split('<')[0];
                    //FINISH
                }
                else
                {
                    CommonHelper.ShowMessage("There is no root element for keyword <SRNO> in the scanned barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDeviceQR.Text = string.Empty;
                    txtDeviceQR.Focus();
                    return;
                }
                //ADDED BY SHIVAM (05/04/2024)
                sMODELCODE = ddlModel_Name.SelectedValue.ToString();
                if(sMODELCODE.StartsWith("JODU"))
                {
                    if (string.IsNullOrEmpty(sSrNo) || string.IsNullOrEmpty(sIMEI) || string.IsNullOrEmpty(sEID) || string.IsNullOrEmpty(sBTMAC))
                    {
                        CommonHelper.ShowMessage("Please scan correct device barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtDeviceQR.Text = "";
                        txtDeviceQR.Focus();
                        return;
                    }
                    if (txtDeviceQR.Text.Trim().ToUpper().Contains("<MACID>"))
                    {
                        CommonHelper.ShowMessage("Please scan correct barcode not required the MACID", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtDeviceQR.Text = "";
                        txtDeviceQR.Focus();
                        return;
                    }
                }
                //FINISH
                blobj = new BL_WIPAccessoriesVerification();
                DataTable dt = new DataTable();
                dt = blobj.blPcbScanDeviceVerify(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), sSrNo
                    , Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(), Session["UserID"].ToString(), sMacID, "VALIDATEDEVICE",
                    txtScanHere.Text.Trim(),sIMEI,sEID,sBTMAC
                    );
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().StartsWith("0~"))
                    {
                        CommonHelper.ShowMessage("PCB scanned successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtScanHere.Enabled = false;
                        txtDeviceQR.Enabled = false;
                        if (lblDevice2QR.Visible == true)
                        {
                            txtDevice2QR.Focus();
                        }
                        else
                        {
                            txtGBQRCode.Focus();
                        }
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
        protected void txtScanGBQR_TextChanged(object sender, EventArgs e)
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
                    txtGBQRCode.Text = "";
                    txtScanHere.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtDeviceQR.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan device barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDeviceQR.Text = "";
                    txtGBQRCode.Text = "";
                    txtDeviceQR.Focus();
                    return;
                }

                //ADDED BY SHIVAM (03012023)
                if (lblDevice2QR.Visible == true)
                {
                    if (string.IsNullOrEmpty(txtDevice2QR.Text.Trim()))
                    {
                        CommonHelper.ShowMessage("Please scan Additional QR barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtDevice2QR.Text = "";
                        txtGBQRCode.Text = "";
                        txtDevice2QR.Focus();
                        return;
                    }
                }
                //FINISH

                if (string.IsNullOrEmpty(txtGBQRCode.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan GB barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtGBQRCode.Text = "";
                    txtGBQRCode.Focus();
                    return;
                }
                if (txtGBQRCode.Text.Trim().Contains("<MRP>"))
                {
                    string sSrNo = string.Empty;
                    string sDeviceSrNo = string.Empty;
                    string sEanNo = string.Empty;
                    string sMacID = string.Empty;
                    string sIMEI = string.Empty;
                    string sEID = string.Empty;
                    string sDeviceMacID = string.Empty;
                    string sDeviceIMEI = string.Empty;
                    string sDeviceEID = string.Empty;
                    string sDeviceBarcode = txtDeviceQR.Text;
                    string sAccBarcode = txtGBQRCode.Text;
                     
                    sDeviceSrNo = Regex.Split(sDeviceBarcode.ToUpper(), "<SRNO>").Last().Split('<')[0].Trim();
                    sDeviceMacID = Regex.Split(sDeviceBarcode.ToUpper(), "<MACID>").Last().Split('<')[0].Trim();
                     
                    //ADDED BY SHIVAM (20/03/2024)
                    sDeviceIMEI = Regex.Split(sDeviceBarcode.ToUpper(), "<IMEI>").Last().Split('<')[0].Trim();
                    sDeviceEID = Regex.Split(sDeviceBarcode.ToUpper(), "<EID>").Last().Split('<')[0].Trim();
                    //FINISH

                    if (sAccBarcode.ToUpper().Contains("<SRNO>"))
                    {
                        sSrNo = Regex.Split(sAccBarcode.ToUpper(), "<SRNO>").Last().Split('<')[0].Trim();
                        sMacID = Regex.Split(sAccBarcode.ToUpper(), "<MACID>").Last().Split('<')[0].Trim();
                        //ADDED BY SHIVAM (20/03/2024)
                        sIMEI = Regex.Split(sAccBarcode.ToUpper(), "<IMEI>").Last().Split('<')[0].Trim();
                        sEID = Regex.Split(sAccBarcode.ToUpper(), "<EID>").Last().Split('<')[0].Trim();
                        //FINISH
                    }
                    else
                    {
                        CommonHelper.ShowMessage("There is no root element for keyword <SRNO> in the scanned barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtDeviceQR.Text = string.Empty;
                        txtDeviceQR.Focus();
                        return;
                    } 
                    if (sSrNo != sDeviceSrNo)
                    {
                        CommonHelper.ShowMessage("Scanned GB RSN not matched with Device RSN, Please scan the correct Gift Box", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtGBQRCode.Text = string.Empty;
                        txtGBQRCode.Focus();
                        return;
                    }
                    if (sMacID != sDeviceMacID)
                    {
                        CommonHelper.ShowMessage("Scanned GB MACID not matched with Device MACID, Please scan the correct Gift Box", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtGBQRCode.Text = string.Empty;
                        txtGBQRCode.Focus();
                        return;
                    } 
                    //ADDED BY SHIVAM(20/03/2024)
                    if (sIMEI != sDeviceIMEI)
                    {
                        CommonHelper.ShowMessage("Scanned GB IMEI not matched with Device RSN, Please scan the correct Gift Box", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtGBQRCode.Text = string.Empty;
                        txtGBQRCode.Focus();
                        return;
                    }
                    if (sEID != sDeviceEID)
                    {
                        CommonHelper.ShowMessage("Scanned GB EID not matched with Device MACID, Please scan the correct Gift Box", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtGBQRCode.Text = string.Empty;
                        txtGBQRCode.Focus();
                        return;
                    }
                    //FINISH 
                    string sDevice2Barcode = string.Empty;
                    if (lblDevice2QR.Visible == true)
                    {
                        sDevice2Barcode = txtDevice2QR.Text.Trim();
                    } 
                    blobj = new BL_WIPAccessoriesVerification();
                    DataTable dt = new DataTable();
                    dt = blobj.blPcbScanGBVerify(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), sSrNo
                        , Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(),
                        Session["UserID"].ToString(), sMacID, "VALIDATEGB",
                        txtDeviceQR.Text.Trim(), txtGBQRCode.Text.Trim(), txtScanHere.Text.Trim()
                        , sDevice2Barcode);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString().StartsWith("0~"))
                        {
                            CommonHelper.ShowMessage("GB verified successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());
                            Reset();
                            txtScanHere.Focus();
                        }
                        else
                        {
                            CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                            //txtDeviceQR.Enabled = false;

                            txtGBQRCode.Text = string.Empty;
                            txtGBQRCode.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("Please scan correct barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtGBQRCode.Text = string.Empty;
                    txtGBQRCode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
               System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtGBQRCode.Text = string.Empty;
                txtGBQRCode.Focus();
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
                dt = blobj.blPcbScanDeviceVerify(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), txtScanHere.Text.Trim()
                    , Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(), Session["UserID"].ToString(), "", "PCBSCANDEVICEVERIFY", ""
                    ,"","","");
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
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            txtScanHere.Text = "";
            txtDeviceQR.Text = "";
            txtDevice2QR.Text = "";
            txtGBQRCode.Text = "";
            txtScanHere.Enabled = true;
            txtDeviceQR.Enabled = true;
            txtDevice2QR.Enabled = true;
            txtGBQRCode.Enabled = true;
        }

        protected void txtScanDevice2QR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                string text = txtDevice2QR.Text.Replace("\n", "").Replace("\r", "");
                text = text + "\n";
                if (ddlModel_Name.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select fg item code", msgerror, CommonHelper.MessageType.Error.ToString());
                    ddlModel_Name.Focus();
                    txtDevice2QR.Text = "";
                    return;
                }
                if (string.IsNullOrEmpty(txtScanHere.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan PCB barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtScanHere.Text = "";
                    txtGBQRCode.Text = "";
                    txtDevice2QR.Text = "";
                    txtScanHere.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtDeviceQR.Text.Trim()))
                {
                    CommonHelper.ShowMessage("Please scan device barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDeviceQR.Text = "";
                    txtGBQRCode.Text = "";
                    txtDevice2QR.Text = "";
                    txtDeviceQR.Focus();
                    return;
                }
                if (txtDevice2QR.Text.Trim().ToUpper().Contains("<MRP>"))
                {
                    CommonHelper.ShowMessage("Please scan correct barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDevice2QR.Text = "";
                    txtDevice2QR.Focus();
                    return;
                }
                string sSrNo = string.Empty;
                string sEanNo = string.Empty;
                string sMacID = string.Empty;
                string sIMEI = string.Empty;
                string sEID = string.Empty;
                string sBTMAC = string.Empty;
                string sMODELCODE = string.Empty;
                string sAccBarcode = txtDevice2QR.Text;
                if (sAccBarcode.ToUpper().Contains("<SRNO>"))
                {
                    sSrNo = Regex.Split(sAccBarcode.ToUpper(), "<SRNO>").Last().Split('<')[0];
                    sMacID = Regex.Split(sAccBarcode.ToUpper(), "<MACID>").Last().Split('<')[0];
                    //ADDED BY SHIVAM (20/03/2024)
                    sIMEI = Regex.Split(sAccBarcode.ToUpper(), "<IMEI>").Last().Split('<')[0];
                    sEID = Regex.Split(sAccBarcode.ToUpper(), "<EID>").Last().Split('<')[0];
                    sBTMAC = Regex.Split(sAccBarcode.ToUpper(), "<BT MAC>").Last().Split('<')[0];
                    //FINISH
                }
                else
                {
                    CommonHelper.ShowMessage("There is no root element for keyword <SRNO> in the scanned barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtDevice2QR.Text = string.Empty;
                    txtDevice2QR.Focus();
                    return;
                }
                //ADDED BY SHIVAM (05/04/2024)
                sMODELCODE = ddlModel_Name.SelectedValue.ToString();
                if (sMODELCODE.StartsWith("JODU"))
                {
                    if (string.IsNullOrEmpty(sSrNo) || string.IsNullOrEmpty(sMacID) || string.IsNullOrEmpty(sIMEI) || string.IsNullOrEmpty(sEID) || string.IsNullOrEmpty(sBTMAC))
                    {
                        CommonHelper.ShowMessage("Please scan correct Additional barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                        txtDevice2QR.Text = "";
                        txtDevice2QR.Focus();
                        return;
                    }
                }
                //FINISH
                blobj = new BL_WIPAccessoriesVerification();
                DataTable dt = new DataTable();
                dt = blobj.blPcbScanDeviceVerify(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString(), sSrNo
                    , Session["LineCode"].ToString(), ddlModel_Name.SelectedValue.ToString(), Session["UserID"].ToString(), sMacID, "VALIDATEDEVICE2",
                    txtScanHere.Text.Trim(),sIMEI,sEID,sBTMAC);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().StartsWith("0~"))
                    {
                        CommonHelper.ShowMessage("PCB scanned successfully", msgsuccess, CommonHelper.MessageType.Success.ToString());
                        txtScanHere.Enabled = false;
                        txtDevice2QR.Enabled = false;
                        txtGBQRCode.Focus();
                    }
                    else
                    {
                        CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        txtDevice2QR.Text = "";
                        txtDevice2QR.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
               System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                txtDevice2QR.Text = "";
                txtDevice2QR.Focus();
            }
        }

        protected void ddlModel_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlModel_Name.SelectedIndex > 0)
                {
                    CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                    blobj = new BL_WIPAccessoriesVerification();
                    DataTable dt = blobj.GetDevice2Req(ddlModel_Name.SelectedItem.Text.Trim(), Session["SiteCode"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString().ToUpper() == "TRUE")
                        {
                            lblDevice2QR.Visible = true;
                        }
                        else
                        {
                            lblDevice2QR.Visible = false;
                        }
                    }
                }
                else
                {
                    ddlModel_Name.Focus();
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }


}