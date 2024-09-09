<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" 
    CodeBehind="WIPAutoSampleClear.aspx.cs"
    Inherits="DIXON.INE.WIP.WIPAutoSampleClear" %>

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
            <h1>WIP Auto Sample Clear</h1>
        </section>
        <section class="content">
            <div class="box box-default">
                <div class="box-header with-border">
                    <h3 class="box-title">WIP Auto Sample Clear</h3>
                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Scan Machine ID/Work Station ID : </label>
                                <asp:TextBox ID="txtScanMachineID" runat="server"
                                    class="form-control" Style="width: 100%;" Height="35"
                                    OnTextChanged="txtScanMachineID_TextChanged" autocomplete="off" AutoPostBack="true">
                                </asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Machine Name/ Work Station Name : </label>
                                <asp:Label ID="lblMachineName" runat="server" Style="width: 100%;" Height="35" Font-Size="35px" ForeColor="Blue">
                                </asp:Label>
                                <label>Model Name/ Process Name : </label>
                                <asp:Label ID="lblModelNo" runat="server" Style="width: 100%;" Height="35" Font-Size="35px" ForeColor="Blue">
                                </asp:Label>
                                <label>Machine Type : </label>
                                <asp:Label ID="lblmachinetype" runat="server" Style="width: 100%;" Height="35" Font-Size="35px" ForeColor="Blue">
                                </asp:Label>
                            </div>
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
                                <label>Scan PCB Barcode : </label>
                                <asp:TextBox ID="txtpcbBarcode" runat="server" placeholder="Scan PCB Barcode" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;" MaxLength="100" OnTextChanged="txtpcbBarcode_TextChanged" AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div>

                             <div class="form-group">
                                <label>Remars : </label>
                                <asp:TextBox ID="txtremark" runat="server" placeholder="Enter Remark" autocomplete="off" class="form-control flat"
                                    Style="width: 100%;"    AutoPostBack="true">                                  
                                </asp:TextBox>
                            </div> 
                        </div>
                    </div>
                     
                   <div class="col-xs-4">
                            <asp:Button ID="btnOk" runat="server" class="btn btn-primary btn-block btn-flat"
                                Text="OK" Visible="true" OnClick="btnOk_Click" />
                        </div>
                          <div class="col-xs-4">
                            <asp:Button ID="btnReject" runat="server" class="btn btn-danger btn-block btn-flat"
                                Text="Reject" Visible="true" OnClick="btnReject_Click" />
                        </div> 
                        <div class="col-xs-4">
                            <asp:Button ID="btnReset" runat="server" class="btn btn-primary btn-block btn-flat"
                                Text="Reset" OnClick="btnReset_Click" Visible="true" />
                        </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>
