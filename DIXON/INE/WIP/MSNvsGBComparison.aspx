<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" 
    CodeBehind="MSNvsGBComparison.aspx.cs"
    Inherits="DIXON.INE.WIP.MSNvsGBComparison" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function PreventPostback(sender, eventArgs) {
            if (eventArgs.get_newValue() == "")
                eventArgs.set_cancel(true);
        }
    </script>
    <script>
        // WRITE THE VALIDATION SCRIPT.
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    </script>
    <style type="text/css">
        .no-select {
            -webkit-user-select: none; /* Safari */
            -moz-user-select: none; /* Firefox */
            -ms-user-select: none; /* Internet Explorer/Edge */
            user-select: none; /* Non-prefixed version, currently supported by Chrome, Opera and Edge */
        }
    </style>
    <div class="content-wrapper">
        <div id="msgdiv" runat="server">
            <div id='msgerror' runat='server' style="display: none;" class="alert alert-danger alert-dismissable flat">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-ban"></i>Alert!</h4>
            </div>
            <div id='msgwarning' runat='server' style="display: none;" class="alert alert-info alert-dismissable flat">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-info"></i>Alert!</h4>
            </div>
            <div id='msginfo' runat='server' style="display: none;" class="alert alert-warning alert-dismissable flat">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-warning"></i>Alert!</h4>
            </div>
            <div id='msgsuccess' runat='server' style="display: none;" class="alert alert-success alert-dismissable flat">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-check"></i>Alert!</h4>
            </div>
        </div>
        <section class="content-header">
            <h1>MSN vs GB Comparison</h1>
        </section>
        <section class="content">
            <div class="box box-default">
                <div class="box-header with-border">
                    <h3 class="box-title">MSN vs GB Comparison</h3>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>FG Item Code : </label>
                                <asp:DropDownList ID="ddlModel_Name" runat="server" class="form-control select2 flat"
                                    Style="width: 100%;" AutoPostBack="true" OnSelectedIndexChanged="ddlModel_Name_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label>Model Name :  </label>
                                <asp:Label ID="lblModelName" Enabled="false" runat="server"
                                    class="form-control flat" Style="width: 100%;" MaxLength="50">                                 
                                </asp:Label>
                            </div>
                            <div class="form-group">
                                <label>Scan MSN Barcode : </label>
                                <asp:TextBox ID="txtMsnBarcode" runat="server" placeholder="Scan MSN Barcode" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" MaxLength="100" OnTextChanged="txtMsnBarcode_TextChanged" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Scan GB Barcode : </label>
                                <asp:TextBox ID="txtGBBarcode" runat="server" placeholder="Scan GB Barcode" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" MaxLength="100" OnTextChanged="txtGBBarcode_TextChanged" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-12 table-responsive">
                            <asp:GridView ID="gvModel" runat="server" Visible="false"
                                class="table table-striped table-bordered table-hover GridPager"
                                AutoGenerateColumns="false">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" />
                                <PagerStyle HorizontalAlign="Left" CssClass="pgr" />
                                <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                                <RowStyle HorizontalAlign="Center" Wrap="true"></RowStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="SNo." HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BOX ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBOXID" runat="server" Text='<%#Eval("BOX_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RSN NUMBER">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRSN" runat="server" class="no-select" Text='<%#Eval("PCB_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MAC ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMACID" runat="server" class="no-select" Text='<%#Eval("IMEI1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate><b style="color: red">No Record Available</b></EmptyDataTemplate>
                                <EditRowStyle BackColor="#999999"></EditRowStyle>
                                <FooterStyle BackColor="#80ccff" Font-Bold="True" ForeColor="White"></FooterStyle>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Remarks :  </label>
                                <asp:TextBox ID="txtRemarks" runat="server" placeholder="Enter Remarks" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;"  TextMode="MultiLine" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>


                    <div class="row mt-5">
                        <div class="col-md-6">
                            <asp:Button ID="btnReject" runat="server" Text="Reject" OnClick="btnReject_Click" class="btn btn-primary btn-block btn-flat mx-0"
                                OnClientClick='return confirm("Are you sure, you want to reject this BOXID?")'/>
                        </div>
                        <div class="col-md-6">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn btn-primary btn-block btn-flat mx-0" />
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>
