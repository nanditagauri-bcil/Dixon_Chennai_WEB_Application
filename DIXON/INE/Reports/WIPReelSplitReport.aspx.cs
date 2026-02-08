using Common;
using DataLayer.Reports;
using DIXON.Helper;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace DIXON.INE.Reports
{
    public partial class WIPReelSplitReport : System.Web.UI.Page
    {
        DL_WIPReelSplitReport blobj = new DL_WIPReelSplitReport();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Default Dates
                txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = blobj.GetReport(txtFromDate.Text, txtToDate.Text, txtBarcode.Text.Trim());

                if (dt != null && dt.Rows.Count > 0)
                {
                    GenerateReport(dt);
                }
                else
                {
                    rvSplitHistory.Visible = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void GenerateReport(DataTable dt)
        {
            rvSplitHistory.Visible = true;
            rvSplitHistory.ProcessingMode = ProcessingMode.Local;
            rvSplitHistory.LocalReport.DataSources.Clear();

            rvSplitHistory.LocalReport.LoadReportDefinition(DynamicRDLCGenerator.GenerateReport(dt, "SPLIT REEL HISTORY REPORT"));

            rvSplitHistory.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            // 3. Set Parameters (Logo, Date, User)
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