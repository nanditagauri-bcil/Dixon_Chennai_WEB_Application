using BusinessLayer.WIP;
using Common;
using System;
using System.Data;
using System.Reflection;

namespace DIXON.INE.WIP
{
    public partial class WIPSplitReelQuality : System.Web.UI.Page
    {
        private const int QUALITY_OK = 1;
        private const int QUALITY_REJECT = 2;

        BL_SplitReelQuality blobj = new BL_SplitReelQuality();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Signin/v1/Login.aspx?Session=Null");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");

            if (!IsPostBack)
            {
                try
                {
                    ValidateUserRights();
                    BindReelID();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        private void ValidateUserRights()
        {
            if (Session["usertype"] == null || Session["usertype"].ToString() != "ADMIN")
            {
                string _strRights = CommonHelper.GetRights("WIP SPLIT REEL QUALITY", (DataTable)Session["USER_RIGHTS"]);
                CommonHelper._strRights = _strRights.Split('^');
                if (CommonHelper._strRights[0] == "0")  
                {
                    Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                }
            }
        }

        public void BindReelID()
        {
            try
            {
                blobj = new BL_SplitReelQuality();

                string siteCode = Session["SiteCode"] != null ? Session["SiteCode"].ToString() : "";

                DataTable dtPcode = blobj.BindReelBarcode(siteCode);

                if (dtPcode != null && dtPcode.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(drpReelID, dtPcode, true);
                    drpReelID.Focus();
                }
                else
                {
                    drpReelID.Items.Clear();
                    CommonHelper.ShowMessage("Part Barcode not available", msgerror, CommonHelper.MessageType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            UpdateQualityStatus(QUALITY_OK);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            UpdateQualityStatus(QUALITY_REJECT);
        }

        private void UpdateQualityStatus(int qualityType)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);

                if (drpReelID.SelectedIndex <= 0)
                {
                    CommonHelper.ShowMessage("Please select part barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpReelID.Focus();
                    return;
                }

                string sLineCode = Session["LINECODE"]?.ToString() ?? "";
                string sUserID = Session["UserID"]?.ToString() ?? "";
                string sSiteCode = Session["SiteCode"]?.ToString() ?? "";

                blobj = new BL_SplitReelQuality();

                DataTable _Result = blobj.SaveQuality(drpReelID.SelectedItem.Text, qualityType, sUserID, sSiteCode, sLineCode);

                string dbMessage = string.Empty;

                if (_Result != null && _Result.Rows.Count > 0)
                {
                    dbMessage = _Result.Rows[0][0].ToString();
                }
                else
                {
                    dbMessage = "N~No response from database.";
                }

                ProcessDbResponse(dbMessage);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ProcessDbResponse(string message)
        {
            string[] parts = message.Split('~');

            if (parts.Length < 2)
            {
                CommonHelper.ShowMessage(message, msgerror, CommonHelper.MessageType.Error.ToString());
                return;
            }

            string text = parts[1];

            if (message.StartsWith("N~") || message.StartsWith("ERROR~"))
            {
                CommonHelper.ShowMessage(text, msgerror, CommonHelper.MessageType.Error.ToString());
            }
            else if (message.StartsWith("SUCCESS~"))
            {
                CommonHelper.ShowMessage(text, msgsuccess, CommonHelper.MessageType.Success.ToString());

                drpReelID.SelectedIndex = 0;
                BindReelID();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
            BindReelID();
        }

        private void HandleException(Exception ex)
        {
            CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name,
                ex.Message);
        }
    }
}