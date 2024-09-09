using Common;
using DataLayer.Reports;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DIXON.INE.Reports
{
    public partial class WIPDeviceVsGBComparisonReport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usertype"].ToString() != "ADMIN")
            {
                string _strRights = CommonHelper.GetRights("WIP DEVICE VS GB REPORT", (DataTable)Session["USER_RIGHTS"]);
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
                    BindFgItemCode();
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    txtFromDate.Text = currentDate;
                    txtToDate.Text = currentDate;
                }
                catch (Exception ex)
                {
                    CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                }
            }
        }

        private void BindFgItemCode()
        {
            try
            {
                DeviceVsGBComparisonReportDL dlobj = new DeviceVsGBComparisonReportDL();

                DataTable dt = dlobj.BindFgItemCode();
                if (dt.Rows.Count > 0)
                {
                    CommonHelper.HideSuccessMessage(msginfo, msgwarning, msgerror);

                    foreach (DataRow row in dt.Rows)
                    {
                        string fgItemCode = row[0].ToString().Trim();
                        drpFgItemCode.Items.Add(new ListItem(fgItemCode, fgItemCode));
                    }

                    drpFgItemCode.SelectedIndex = -1;
                    drpFgItemCode.Focus();
                }
                else
                {
                    CommonHelper.ShowMessage("No data found.", msginfo, CommonHelper.MessageType.Info.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                CommonHelper.HideMessage(msginfo, msgsuccess, msgwarning, msgerror);
                DeviceVsGBComparisonReportDL dlobj = new DeviceVsGBComparisonReportDL();

                if (drpFgItemCode.SelectedIndex < 0)
                {
                    CommonHelper.ShowCustomErrorMessage("Please select Fg Item Code", msgerror);
                    drpFgItemCode.Focus();
                    return;
                }

                string fromDate = txtFromDate.Text.Trim();
                string toDate = txtToDate.Text.Trim();
                string fgItemCode = drpFgItemCode.Text.Trim();

                DataTable dtReport = dlobj.GetReport(fgItemCode, fromDate, toDate);

                if (dtReport.Rows.Count == 0)
                {
                    CommonHelper.ShowCustomErrorMessage("No record found", msgerror);
                    return;
                }

                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage,
                     System.Reflection.MethodBase.GetCurrentMethod().Name,
                     "WIP Device Vs GB comparison report count:" + dtReport.Rows.Count);

                ReportViewer.LocalReport.ReportPath = Server.MapPath("~/ReportViewer/DeviceVsGBComparisonReport.rdlc");
                ReportDataSource datasource = new ReportDataSource("Data", dtReport);
                ReportViewer.ProcessingMode = ProcessingMode.Local;
                ReportViewer.Visible = true;
                ReportViewer.LocalReport.DataSources.Clear();
                ReportViewer.LocalReport.DataSources.Add(datasource);
                ReportViewer.LocalReport.EnableExternalImages = true;

                DataTable dtHeaderDetail = dlobj.GetHeaderDetail(fgItemCode);

                if (dtHeaderDetail.Rows.Count == 0)
                {
                    CommonHelper.ShowCustomErrorMessage("Report header detail not found", msgerror);
                }

                string fgItemDesc = dtHeaderDetail?.Rows[0][0]?.ToString().Trim();

                string headerLogo = new Uri(Server.MapPath("~/images/ReportLogo.png")).AbsoluteUri;
                ReportParameter parameter1 = new ReportParameter("rptLogo", headerLogo);
                ReportParameter parameter2 = new ReportParameter("rptFgItemCode", fgItemCode);
                ReportParameter parameter3 = new ReportParameter("rptFgItemDesc", fgItemDesc);
                ReportParameter parameter4 = new ReportParameter("rptFromDate", fromDate);
                ReportParameter parameter5 = new ReportParameter("rptToDate", toDate);

                ReportViewer.LocalReport.SetParameters(parameter1);
                ReportViewer.LocalReport.SetParameters(parameter2);
                ReportViewer.LocalReport.SetParameters(parameter3);
                ReportViewer.LocalReport.SetParameters(parameter4);
                ReportViewer.LocalReport.SetParameters(parameter5);
                ReportViewer.LocalReport.Refresh();
                ScriptManager.RegisterStartupScript(Page, GetType(), "script", "ShowMessage();", true);
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}