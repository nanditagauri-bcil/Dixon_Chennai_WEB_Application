using BusinessLayer.WIP;
using Common;
using System;
using System.Data;

namespace DIXON.INE.WIP
{
    public partial class WIPSplitReelQuality : System.Web.UI.Page
    {
        string Message = "";
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
            if (Session["usertype"].ToString() != "ADMIN")
            {
                string _strRights = CommonHelper.GetRights("SPLITREELQUALITY", (DataTable)Session["USER_RIGHTS"]);
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
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                blobj = new BL_SplitReelQuality();
                DataTable dtPcode = blobj.BindReelBarcode(Session["SiteCode"].ToString());
                if (dtPcode.Rows.Count > 0)
                {
                    CommonHelper.HideSuccessMessage(msginfo, msgwarning, msgerror);
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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (drpReelID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select part barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpReelID.Focus();
                    return;
                }

                string sLineCode = Session["LINECODE"].ToString();
                string sUserID = Session["UserID"].ToString();
                blobj = new BL_SplitReelQuality();
                DataTable dt = new DataTable();
                DataTable _Result = blobj.SaveQuality(drpReelID.Text, 1, sUserID, Session["SiteCode"].ToString(), sLineCode);

                if (_Result != null && _Result.Rows.Count > 0)
                {
                    Message = _Result.Rows[0][0].ToString();
                }
                else
                {
                    Message = "N~Nothing returned form DB";
                }

                string[] msg = Message.Split('~');
                string Msgs = msg[0];

                if (Message.Length > 0)
                {
                    if (msg[0].StartsWith("N") || msg[0].StartsWith("ERROR"))
                    {
                        CommonHelper.ShowMessage(msg[1], msgerror, CommonHelper.MessageType.Error.ToString());
                    }
                    else
                    {
                        CommonHelper.ShowMessage(msg[1], msgsuccess, CommonHelper.MessageType.Success.ToString());
                        drpReelID.Items.Clear();
                        drpReelID.Focus();
                        BindReelID();
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

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (drpReelID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select part barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpReelID.Focus();
                    return;
                }

                string sLineCode = Session["LINECODE"].ToString();
                string sUserID = Session["UserID"].ToString();
                blobj = new BL_SplitReelQuality();
                DataTable dt = new DataTable();

                DataTable _Result = blobj.SaveQuality(drpReelID.Text, 2, sUserID, Session["SiteCode"].ToString(), sLineCode);

                if (_Result != null && _Result.Rows.Count > 0)
                {
                    Message = _Result.Rows[0][0].ToString();
                }
                else
                {
                    Message = "N~Nothing returned form DB";
                }

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
                        drpReelID.Focus();
                        BindReelID();
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
                drpReelID.Focus();
            }
            BindReelID();
        }
    }
}