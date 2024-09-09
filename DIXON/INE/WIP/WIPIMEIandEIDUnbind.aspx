<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WIPIMEIandEIDUnbind.aspx.cs"
    Inherits="DIXON.INE.WIP.WIPIMEIandEIDUnbind" %>

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
    <style>
        .radio-list label {
            display: inline-flex;
            align-items: center;
            margin-right: 15px; /* Adjust the space between radio buttons */
        }

        .radio-list input[type="radio"] {
            margin-right: 5px; /* Adjust the space between radio button and text */
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
            <h1>IMEI&EID / CHIPID Unbind</h1>
        </section>
        <section class="content">
            <div class="box box-default">
                <div class="box-header with-border">
                    <h3 class="box-title">IMEI&EID / CHIPID Unbind</h3>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Scan PCB ID : </label>
                                <asp:TextBox ID="txtpcbid" runat="server" placeholder="Scan PCBID" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Scan MACID : </label>
                                <asp:TextBox ID="txtmacid" runat="server" placeholder="Scan MACID" autocomplete="off" class="form-control"
                                    Style="width: 100%;" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:RadioButtonList ID="rdIsImeiEid" runat="server" CssClass="radio-list" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdIsImeiEid_SelectedIndexChanged">
                                    <asp:ListItem Text=" IS IMEI&EID" Value="IMEI&EID" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text=" IS CHIPID" Value="CHIPID"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>

                    <div class="row" id="dvchipid" runat="server" visible="false">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Scan CHIPID : </label>
                                <asp:TextBox ID="txtchipid" runat="server" placeholder="Scan CHIPID" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row" id="dvimeieid" runat="server">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Scan IMEI : </label>
                                <asp:TextBox ID="txtimei" runat="server" placeholder="Scan IMEI" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Scan EID : </label>
                                <asp:TextBox ID="txteid" runat="server" placeholder="Scan EID" autocomplete="off" class="form-control"
                                    Style="width: 100%;" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="col-xs-3">
                                <asp:Button ID="btnUnbind" runat="server" Text="Unbind" class="btn btn-primary btn-block btn-flat"
                                    OnClick="btnUnbind_Click" />
                            </div>
                            <div class="col-xs-3">
                                <asp:Button ID="btnReset" runat="server" Text="Reset" class="btn btn-primary btn-block btn-flat"
                                    OnClick="btnReset_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </section>
    </div>
</asp:Content>
