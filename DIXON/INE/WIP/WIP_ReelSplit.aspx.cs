using BusinessLayer;
using Common;
using System;
using System.Data;
using System.Web.UI;

namespace DIXON.INE.WIP
{
    public partial class WIP_ReelSplit : Page
    {
        BL_ReelSplitPrinting blobj = new BL_ReelSplitPrinting();
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
            if (Session["usertype"].ToString() != "ADMIN")
            {
                string _strRights = CommonHelper.GetRights("WIP REEL SPLIT", (DataTable)Session["USER_RIGHTS"]);
                CommonHelper._strRights = _strRights.Split('^');
                if (CommonHelper._strRights[0] == "0")  //Check view rights
                {
                    Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                }
            }

            Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            if (!IsPostBack)
            {
                try
                {
                    BindReelID();
                }
                catch (Exception ex)
                {
                    CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
        }

        public void BindReelID()
        {
            try
            {
                txtQuantity.Text = string.Empty;
                txtQty.Text = string.Empty;
                blobj = new BL_ReelSplitPrinting();
                DataTable dtPcode = blobj.BindReelBarcode(Session["SiteCode"].ToString());
                if (dtPcode.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(drpReelID, dtPcode, true);
                    drpReelID.Focus();
                }
                else
                {
                    CommonHelper.ShowMessage("Part Barcode not available", msgerror, CommonHelper.MessageType.Error.ToString());
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void drpReelID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtQuantity.Text = string.Empty;
                txtQty.Text = string.Empty;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (drpReelID.Text == "--Select Reel ID--" || drpReelID.Text == "0")
                {
                    txtQty.Text = string.Empty;
                    txtQuantity.Text = string.Empty;
                    drpReelID.Focus();
                    return;
                }
                blobj = new BL_ReelSplitPrinting();
                DataTable dt = new DataTable();
                dt = blobj.ValidateReelBarcode(drpReelID.Text, Session["SiteCode"].ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().StartsWith("N~") || dt.Rows[0][0].ToString().StartsWith("ERROR~"))
                    {
                        CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
                        return;
                    }
                    else
                    {
                        Message = dt.Rows[0][0].ToString();
                        txtQty.Text = Convert.ToString(Message.Split('~')[1]);
                    }
                }
                else
                {
                    CommonHelper.ShowMessage("No result found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                if (drpReelID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select part barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    return;
                }
                if (txtQuantity.Text.Trim() == "")
                {
                    CommonHelper.ShowMessage("Please enter quantity", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtQuantity.Focus();
                    return;
                }
                if (Convert.ToDecimal(txtQuantity.Text) == 0)
                {
                    CommonHelper.ShowMessage("Please enter valid quantity", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtQuantity.Text = string.Empty;
                    txtQuantity.Focus();
                    return;
                }
                if (Convert.ToDecimal(txtQuantity.Text) > Convert.ToDecimal(txtQty.Text))
                {
                    CommonHelper.ShowMessage("Enter quantity can not be greater than displayed quantity", msgerror, CommonHelper.MessageType.Error.ToString());
                    txtQuantity.Text = string.Empty;
                    txtQuantity.Focus();
                    return;
                }

                string sLineCode = Session["LINECODE"].ToString();
                string sUserID = Session["UserID"].ToString();
                blobj = new BL_ReelSplitPrinting();
                DataTable dt = new DataTable();
                string _Result = blobj.ChildLabelPrint(drpReelID.Text,
                         Session["UserID"].ToString(),
                         Convert.ToDecimal(txtQuantity.Text),
                         sLineCode, Session["SiteCode"].ToString()
                    );

                Message = _Result;
                string[] msg = Message.Split('~');
                string Msgs = msg[0];

                if (Message.Length > 0)
                {
                    if (msg[0].StartsWith("N~") || msg[0].StartsWith("ERROR~"))
                    {
                        CommonHelper.ShowMessage(msg[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                    else
                    {
                        CommonHelper.ShowMessage(msg[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        drpReelID.Items.Clear();
                        txtQty.Text = "";
                        txtQuantity.Text = "";
                        BindReelID();

                        string script = @"
                            var iframe = document.createElement('iframe');
                            iframe.src = '../PrintLabel.aspx';
                            iframe.style.display = 'none';
                            document.body.appendChild(iframe);
                        ";

                        ScriptManager.RegisterStartupScript(this, GetType(), "DownloadFile", script, true);
                    }
                }
                else
                {
                    CommonHelper.ShowMessage(msg[1], msgerror, CommonHelper.MessageType.Error.ToString());
                }

            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            if (drpReelID.SelectedIndex > 0)
            {
                drpReelID.Items.Clear();
                txtQty.Text = "";
                txtQuantity.Text = "";
            }
            BindReelID();
        }
    }
}