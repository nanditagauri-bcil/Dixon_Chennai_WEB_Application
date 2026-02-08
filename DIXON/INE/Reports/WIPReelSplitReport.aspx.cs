using Common;
using DataLayer.Reports;
using DIXON.Helper;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;

namespace DIXON.INE.Reports
{
    public partial class WIPReelSplitReport : System.Web.UI.Page
    {
        private readonly DL_WIPReelSplitReport blobj = new DL_WIPReelSplitReport();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindReportType();

                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
            }
        }

        private void BindReportType()
        {
            try
            {
                DataTable dt = blobj.GetReportTypes();

                if (dt != null && dt.Rows.Count > 0)
                {
                    clsCommon.FillComboBox(drpReportType, dt, true);
                    drpReportType.Focus();
                }
                else
                {
                    drpReportType.Items.Clear();
                    CommonHelper.ShowMessage("Part Barcode not available", msgerror, CommonHelper.MessageType.Error.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpReportType.SelectedIndex == 0)
                {
                    CommonHelper.ShowMessage("Please select a Report Type.", msgerror, CommonHelper.MessageType.Error.ToString());
                    return;
                }

                string fromDate = string.IsNullOrWhiteSpace(txtFromDate.Text) ? null : txtFromDate.Text;
                string toDate = string.IsNullOrWhiteSpace(txtToDate.Text) ? null : txtToDate.Text;
                string barcode = txtBarcode.Text.Trim();
                string reportType = drpReportType.SelectedValue;

                DataTable dt = blobj.GetReport(fromDate, toDate, barcode, reportType);

                if (dt != null && dt.Rows.Count > 0)
                {
                    GenerateReport(dt);
                }
                else
                {
                    CommonHelper.ShowMessage("No records found.", msgerror, CommonHelper.MessageType.Info.ToString());
                    rvSplitHistory.Visible = false;
                }
            }
            catch (Exception ex)
            {
                CommonHelper.ShowCustomErrorMessage(ex.Message, msgerror);
            }
        }

        private void GenerateReport(DataTable dt)
        {
            rvSplitHistory.Visible = true;
            rvSplitHistory.ProcessingMode = ProcessingMode.Local;
            rvSplitHistory.LocalReport.DataSources.Clear();

            rvSplitHistory.LocalReport.LoadReportDefinition(DynamicRDLCGenerator.GenerateReport(dt, "SPLIT REEL HISTORY REPORT"));

            rvSplitHistory.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            string imagePath = new Uri(Server.MapPath("~/images/ReportLogo.png")).AbsoluteUri;
            string userName = Session["UserID"] != null ? Session["UserID"].ToString() : "Admin";

            rvSplitHistory.LocalReport.EnableExternalImages = true;
            rvSplitHistory.LocalReport.SetParameters(new ReportParameter[] {
                new ReportParameter("rptLogo", imagePath),
                new ReportParameter("rptTitle", "SPLIT REEL HISTORY REPORT"),
                new ReportParameter("rptGeneratedBy", userName),
                new ReportParameter("rptGeneratedOn", DateTime.Now.ToString("dd-MM-yyyy HH:mm"))
            });

            rvSplitHistory.LocalReport.Refresh();
        }
    }
}