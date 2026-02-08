<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WIPReelSplitReport.aspx.cs" Inherits="DIXON.INE.Reports.WIPReelSplitReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <section class="content-header">
            <h1>Split Reel History Report</h1>
        </section>

        <section class="content">
            <div class="box box-default">

                <div class="box-header with-border">
                    <h3 class="box-title">Filter Criteria</h3>
                    <div class="box-tools pull-right">
                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    </div>
                </div>

                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>From Date:</label>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>To Date:</label>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Barcode (Optional):</label>
                                        <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" placeholder="Scan/Enter Barcode"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="btnShow" runat="server" Text="Show Report" CssClass="btn btn-primary btn-block btn-flat" OnClick="btnShow_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-12">
                            <rsweb:ReportViewer ID="rvSplitHistory" runat="server"
                                Width="100%" Height="800px"
                                AsyncRendering="False"
                                SizeToReportContent="False"
                                ZoomMode="PageWidth"
                                ShowPrintButton="true">
                            </rsweb:ReportViewer>
                        </div>
                    </div>

                </div>
            </div>
        </section>
    </div>
</asp:Content>
