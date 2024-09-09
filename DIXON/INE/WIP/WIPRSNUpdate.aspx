<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WIPRSNUpdate.aspx.cs"
    Inherits="DIXON.INE.WIP.WIPRSNUpdate" %>

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
            <h1>RSN Update</h1>
        </section>
        <section class="content">
            <div class="box box-default">
                <div class="box-header with-border">
                    <h3 class="box-title">RSN Update </h3>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Scan RSN : </label>
                                <asp:TextBox ID="txtrsn" runat="server" placeholder="Enter RSN" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" MaxLength="100" OnTextChanged="txtrsn_TextChanged" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>MAC ID : </label>
                                <asp:TextBox ID="txtmacid" runat="server" placeholder="MAC ID" autocomplete="off" class="form-control"
                                    Style="width: 100%;" MaxLength="100" AutoPostBack="true" ReadOnly="True"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Current RSN Month : </label>
                                <asp:TextBox ID="txtcuurentRSNmonth" runat="server" placeholder="Current RSN Month" autocomplete="off" class="form-control"
                                    Style="width: 100%;" MaxLength="100" AutoPostBack="true" ReadOnly="True"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>New RSN : </label>
                                <asp:TextBox ID="txtnewrsn" runat="server" placeholder="Enter New RSN" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" MaxLength="100"   AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Wifi Key : </label>
                                <asp:TextBox ID="txtwifikey" runat="server" placeholder="Enter Wifi Key" autocomplete="off" class="form-control"
                                    Style="width: 100%;" MaxLength="100" AutoPostBack="true"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Wireless SSID : </label>
                                <asp:TextBox ID="txtssid" runat="server" placeholder="Enter Wireless SSID" autocomplete="off" class="form-control"
                                    Style="width: 100%;" MaxLength="100" AutoPostBack="true"></asp:TextBox>
                            </div>

                            <div class="col-xs-3">
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" class="btn btn-primary btn-block btn-flat" OnClick="btnUpdate_Click" />
                            </div>
                            <div class="col-xs-3">
                                <asp:Button ID="btnReset" runat="server" Text="Reset" class="btn btn-primary btn-block btn-flat" OnClick="btnReset_Click" />
                            </div>
                        </div>
                    </div> 
                </div>
            </div>
    </section>
    </div> 
</asp:Content>
