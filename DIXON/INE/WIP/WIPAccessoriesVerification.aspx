<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WIPAccessoriesVerification.aspx.cs"
    Inherits="DIXON.INE.WIP.WIPAccessoriesVerification" %>

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
            <h1>Accessories Verification</h1>
        </section>
        <section class="content">
            <div class="box box-default">
                <div class="box-header with-border">
                    <h3 class="box-title">Accessories Verification </h3>
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
                                <label>Scan Pcb Barcode : </label>
                                <asp:TextBox ID="txtScanHere" runat="server" placeholder="Enter Barcode" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" MaxLength="100" OnTextChanged="txtScanHere_TextChanged" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Scan Accessories Barcode : </label>
                                <asp:TextBox ID="txtAccessoriesBarcode" runat="server" placeholder="Enter Barcode" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" MaxLength="1000" OnTextChanged="txtAccessoriesBarcode_TextChanged" AutoPostBack="true">                                  
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
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccessName" runat="server" Text='<%#Eval("ACCNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Digit">
                                        <ItemTemplate>
                                            <asp:Label ID="StartDigit" runat="server" Text='<%#Eval("START_DIGIT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Digit">
                                        <ItemTemplate>
                                            <asp:Label ID="EndDigit" runat="server" Text='<%#Eval("END_DIGIT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:Label ID="Value" runat="server" Text='<%#Eval("KEY_VAL1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ScanBarcode">
                                        <ItemTemplate>
                                            <asp:Label ID="ScanBarcode" runat="server" Text='<%#Eval("SCANBARCODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate><b style="color: red">No Record Available</b></EmptyDataTemplate>
                                <EditRowStyle BackColor="#999999"></EditRowStyle>
                                <FooterStyle BackColor="#80ccff" Font-Bold="True" ForeColor="White"></FooterStyle>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row mt-5">
                        <div class="col-md-6">
                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn btn-primary btn-block btn-flat mx-0" />
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>
