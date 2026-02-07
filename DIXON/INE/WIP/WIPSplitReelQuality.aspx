<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WIPSplitReelQuality.aspx.cs" Inherits="DIXON.INE.WIP.WIPSplitReelQuality" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            <div id='msgerror' runat='server' style="display: none;" class="alert alert-danger alert-dismissable">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-ban"></i></h4>
            </div>
            <div id='msgwarning' runat='server' style="display: none;" class="alert alert-info alert-dismissable">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-info"></i></h4>
            </div>
            <div id='msginfo' runat='server' style="display: none;" class="alert alert-warning alert-dismissable">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-warning"></i></h4>
            </div>
            <div id='msgsuccess' runat='server' style="display: none;" class="alert alert-success alert-dismissable">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">
                    &times;</button>
                <h4>
                    <i class="icon fa fa-check"></i></h4>
            </div>
        </div>
        <section class="content-header">
            <h1>Split Reel Quality</h1>

        </section>
        <section class="content">
            <div class="box box-default">
                <div class="box-header with-border">
                    <h3 class="box-title">WIP Split Reel Quality</h3>

                </div>
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Part Barcode : </label>
                                <asp:DropDownList ID="drpReelID" runat="server" class="form-control select2" AutoPostBack="true"
                                    Style="width: 100%;">
                                </asp:DropDownList>
                            </div>

                            <div class="col-xs-4">
                                <asp:Button ID="btnOk" runat="server" UseSubmitBehavior="false" Text="Okay" class="btn btn-success btn-block btn-flat" OnClick="btnOk_Click" />
                            </div>
                            <div class="col-xs-4">
                                <asp:Button ID="btnReject" runat="server" UseSubmitBehavior="false" Text="Reject" class="btn btn-danger btn-block btn-flat" OnClick="btnReject_Click" />
                            </div>
                            <div class="col-xs-4">
                                <asp:Button ID="btnReset" runat="server" UseSubmitBehavior="false" Text="Reset" class="btn btn-primary btn-block btn-flat" OnClick="btnReset_Click" />
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <asp:HiddenField ID="hidRNO" runat="server" />
                    <asp:HiddenField ID="hidqty" runat="server" />
                    <asp:HiddenField ID="hidEXPDATE" runat="server" />
                </div>
            </div>
        </section>
    </div>
</asp:Content>
