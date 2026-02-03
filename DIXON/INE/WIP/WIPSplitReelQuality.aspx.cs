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
                    BindPartCode();
                }
                catch (Exception ex)
                {
                    CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
        }

        public void BindPartCode()
        {
            try
            {
                drpReelID.Items.Clear();
                hidqty.Value = "";
                drpItemCode.Items.Clear();
                blobj = new BL_SplitReelQuality();
                DataTable dtPcode = blobj.BindINELPartNo(Session["SiteCode"].ToString());
                if (dtPcode.Rows.Count > 0)
                {
                    CommonHelper.HideSuccessMessage(msginfo, msgwarning, msgerror);
                    clsCommon.FillComboBox(drpItemCode, dtPcode, true);
                    drpItemCode.Focus();
                }
                else
                {
                    CommonHelper.ShowMessage("No part code is available for prinitng.", msginfo, CommonHelper.MessageType.Info.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                if (drpItemCode.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select part code", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpItemCode.Focus();
                    return;
                }
                if (drpReelID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select part barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpItemCode.Focus();
                    return;
                }

                string sLineCode = Session["LINECODE"].ToString();
                string sUserID = Session["UserID"].ToString();
                blobj = new BL_SplitReelQuality();
                DataTable dt = new DataTable();
                DataTable _Result = blobj.SaveQuality(drpItemCode.Text, drpReelID.Text, 1, sUserID, Session["SiteCode"].ToString(), sLineCode);

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
                        drpItemCode.SelectedIndex = 0;
                        drpItemCode.Focus();
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
                if (drpItemCode.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select part code", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpItemCode.Focus();
                    return;
                }
                if (drpReelID.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select part barcode", msgerror, CommonHelper.MessageType.Error.ToString());
                    drpItemCode.Focus();
                    return;
                }

                string sLineCode = Session["LINECODE"].ToString();
                string sUserID = Session["UserID"].ToString();
                blobj = new BL_SplitReelQuality();
                DataTable dt = new DataTable();

                DataTable _Result = blobj.SaveQuality(drpItemCode.Text, drpReelID.Text, 2, sUserID, Session["SiteCode"].ToString(), sLineCode);

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
                        drpItemCode.SelectedIndex = 0;
                        drpItemCode.Focus();
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
            if (drpItemCode.SelectedIndex > 0)
            {
                drpReelID.Items.Clear();
                drpItemCode.SelectedIndex = 0;
                drpItemCode.Focus();
            }
            BindPartCode();
        }

        public void bindReelID()
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                blobj = new BL_SplitReelQuality();
                DataTable dtPcode = blobj.BindReelBarcode(drpItemCode.Text, Session["SiteCode"].ToString());
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

        //protected void drpReelID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        txtQuantity.Text = string.Empty;
        //        txtQty.Text = string.Empty;
        //        CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
        //        if (drpReelID.Text == "--Select Reel ID--" || drpReelID.Text == "0")
        //        {
        //            txtQty.Text = string.Empty;
        //            txtQuantity.Text = string.Empty;
        //            drpReelID.Focus();
        //            return;
        //        }
        //        blobj = new BL_SplitReelQuality();
        //        DataTable dt = new DataTable();
        //        //dt = blobj.(drpItemCode.Text, drpReelID.Text, Session["SiteCode"].ToString());
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0][0].ToString().StartsWith("N~") || dt.Rows[0][0].ToString().StartsWith("ERROR~"))
        //            {
        //                CommonHelper.ShowMessage(dt.Rows[0][0].ToString().Split('~')[1], msgerror, CommonHelper.MessageType.Error.ToString());
        //                return;
        //            }
        //            else
        //            {
        //                Message = dt.Rows[0][0].ToString();
        //                txtQty.Text = Convert.ToString(Message.Split('~')[1]);
        //                hidqty.Value = Convert.ToString(txtQty.Text);
        //            }
        //        }
        //        else
        //        {
        //            CommonHelper.ShowMessage("No result found, Please try again", msgerror, CommonHelper.MessageType.Error.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
        //        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //    }
        //}

        protected void drpItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpItemCode.SelectedIndex <= 0)
                {
                    drpReelID.Items.Clear();
                    return;
                }
                bindReelID();
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}