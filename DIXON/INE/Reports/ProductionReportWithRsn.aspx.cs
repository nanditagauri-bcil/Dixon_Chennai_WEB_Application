using BusinessLayer.Reports;
using ClosedXML.Excel;
using Common;
using System;
using System.Data;
using System.IO;

namespace DIXON.INE.Reports
{
    public partial class ProductionReportWithRsn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            if (Session["usertype"].ToString() != "ADMIN")
            {
                string _strRights = CommonHelper.GetRights("ProdDataReportwithRSNforInnopia", (DataTable)Session["USER_RIGHTS"]);
                CommonHelper._strRights = _strRights.Split('^');
                if (CommonHelper._strRights[0] == "0")
                {
                    Response.Redirect("~/NoAccess/UnAuthorized.aspx");
                }
            }
            if (!IsPostBack)
            {
                try
                {


                    //BindFGItemCode();
                    string current_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //drpFGItemCode('drpFGItemCode');
                    txtFromDate.Text = current_date;
                    txtToDate.Text = current_date;
                }
                catch (Exception ex)
                {
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }

        }
        //private void BindFGItemCode()
        //{
        //    try
        //    {
        //        BL_OQCSamplingReport blobj;
        //        blobj = new BL_OQCSamplingReport();
        //        DataTable dtGRN = blobj.BindProdFGItemCode();
        //        if (dtGRN.Rows.Count > 0)
        //        {
        //            CommonHelper.HideSuccessMessage(msginfo, msgwarning, msgerror);
        //            clsCommon.FillComboBox(drpFGItemCode, dtGRN, true);
        //            drpFGItemCode.Focus();
        //        }
        //        else
        //        {
        //            CommonHelper.ShowMessage("No data found.", msginfo, CommonHelper.MessageType.Info.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
        //        CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
        //    }
        //}
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                ShowGridData();
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        private void ShowGridData()
        {
            try
            {
                string sFGItemCode = string.Empty;
                //if (sFGItemCode = drpFGItemCode.Text!= null)
                //{
                //    CommonHelper.ShowMessage("Please select model", msgerror, CommonHelper.MessageType.Error.ToString());
                //    return;
                //}
                sFGItemCode = drpFGItemCode.Text;
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                BL_OQCSamplingReport blobj = new BL_OQCSamplingReport();
                DataTable dtTrackingData = blobj.GetProdDataReport(txtFromDate.Text, txtToDate.Text, sFGItemCode);
                if (dtTrackingData.Rows.Count > 0)
                {
                    if (dtTrackingData.Columns.Count == 1 && dtTrackingData.Rows[0][0].ToString().StartsWith("ERROR~"))
                    {
                        CommonHelper.ShowMessage(dtTrackingData.Rows[0][0].ToString(), msgerror, CommonHelper.MessageType.Error.ToString());
                        return;
                    }
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage,
                          System.Reflection.MethodBase.GetCurrentMethod().Name, "Prod Data Report No of records :" + dtTrackingData.Rows.Count);
                    string sFileName = "ProRptRSN-" + drpFGItemCode.Text.Trim() + DateTime.Now.ToString("_dd_MM_yyyy");
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dtTrackingData, "ProRptRSN");
                        string myName = Server.UrlEncode(sFileName + ".xlsx");
                        System.IO.MemoryStream stream = GetStream(wb);// The method is defined below
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition",
                        "attachment; filename=" + myName);
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.BinaryWrite(stream.ToArray());
                        Response.End();
                    }
                }
                else
                {
                    CommonHelper.ShowCustomErrorMessage("No Records Found", msgerror);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }
        }
        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }
}