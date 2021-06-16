<%@ Page Title="Dish" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dish.aspx.cs" Inherits="EateryDuwamish.Dish" %>
<%@ Register Src="~/UserControl/NotificationControl.ascx" TagName="NotificationControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%--Datatable Configuration--%>
    <script type="text/javascript">
        function ConfigureDatatable() {
            var table = null;
            if ($.fn.dataTable.isDataTable('#htblDish')) {
                table = $('#htblDish').DataTable();
            }
            else {
                table = $('#htblDish').DataTable({
                    stateSave: false,
                    order: [[1, "asc"]],
                    columnDefs: [{ orderable: false, targets: [0] }]
                });
            }
            return table;
        }
    </script>
    <%--Checkbox Event Configuration--%>
    <script type="text/javascript">
        function ConfigureCheckboxEvent() {
            $('.checkDelete input').change(function () {
                var parent = $(this).parent();
                var value = $(parent).attr('data-value');
                var deletedList = [];

                if ($('#<%=hdfDeletedDishes.ClientID%>').val())
                    deletedList = $('#<%=hdfDeletedDishes.ClientID%>').val().split(',');

                if ($(this).is(':checked')) {
                    deletedList.push(value);
                    $('#<%=hdfDeletedDishes.ClientID%>').val(deletedList.join(','));
                }
                else {
                    var index = deletedList.indexOf(value);
                    if (index >= 0)
                        deletedList.splice(index, 1);
                    $('#<%=hdfDeletedDishes.ClientID%>').val(deletedList.join(','));
                }
            });
        }
    </script>
    <%--Main Configuration--%>
    <script type="text/javascript">
        function ConfigureElements() {
            ConfigureDatatable();
            ConfigureCheckboxEvent();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                $(document).ready(function () {
                    ConfigureElements();
                });
                <%--On Partial Postback Callback Function--%>
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    ConfigureElements();
                });
            </script>
            <uc1:NotificationControl ID="notifDish" runat="server" />
            <div class="page-title">Master Dish</div><hr style="margin:0"/>
            <%--FORM DISH--%>
            <asp:Panel runat="server" ID="pnlFormDish" Visible="false">
                <div class="form-slip">
                    <div class="form-slip-header">
                        <div class="form-slip-title">
                            FORM DISH - 
                            <asp:Literal runat="server" ID="litFormType"></asp:Literal>
                        </div>
                        <hr style="margin:0"/>
                    </div>
                    <div class="form-slip-main">
                        <asp:HiddenField ID="hdfDishId" runat="server" Value="0"/>
                        <div>
                            <%--Dish Name Field--%>
                            <div class="col-lg-6 form-group">
                                <div class="col-lg-4 control-label">
                                    Dish Name*
                                </div>
                                <div class="col-lg-6">
                                    <asp:TextBox ID="txtDishName" CssClass="form-control" runat="server"></asp:TextBox>
                                    <%--Validator--%>
                                    <asp:RequiredFieldValidator ID="rfvDishName" runat="server" ErrorMessage="Please fill this field"
                                        ControlToValidate="txtDishName" ForeColor="Red" 
                                        ValidationGroup="InsertUpdateDish" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revDishName" runat="server" ErrorMessage="This field has a maximum of 100 characters"
                                        ControlToValidate="txtDishName" ValidationExpression="^[\s\S]{0,100}$" ForeColor="Red"
                                        ValidationGroup="InsertUpdateDish" Display="Dynamic">
                                    </asp:RegularExpressionValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Dish Name Field--%>
                            <%--Dish Type Field--%>
                            <div class="col-lg-6 form-group">
                                <div class="col-lg-4 control-label">
                                    Dish Type*
                                </div>
                                <div class="col-lg-6">
                                    <asp:DropDownList ID="ddlDishType" CssClass="form-control" runat="server"></asp:DropDownList>
                                    <%--Validator--%>
                                    <asp:RequiredFieldValidator ID="rfvDishType" runat="server" ErrorMessage="Please fill this field"
                                        ControlToValidate="ddlDishType" ForeColor="Red" InitialValue="0"
                                        ValidationGroup="InsertUpdateDish" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Dish Type Field--%>
                            <%--Dish Price Field--%>
                            <div class="col-lg-6 form-group">
                                <div class="col-lg-4 control-label">
                                    Price*
                                </div>
                                <div class="col-lg-6">
                                    <div class="input-price">
                                        <div class="price-prefix">Rp. </div>
                                        <asp:TextBox ID="txtPrice" CssClass="form-control" runat="server" type="number"
                                             Min="0" Max="999999999"></asp:TextBox>
                                    </div>
                                    <%--Validator--%>
                                    <asp:RequiredFieldValidator ID="rfvPrive" runat="server" ErrorMessage="Please fill this field"
                                        ControlToValidate="txtPrice" ForeColor="Red"
                                        ValidationGroup="InsertUpdateDish" Display="Dynamic">
                                    </asp:RequiredFieldValidator>
                                    <%--End of Validator--%>
                                </div>
                            </div>
                            <%--End of Dish Type Field--%>
                        </div>
                        <div class="col-lg-12">
                            <div class="col-lg-2">
                            </div>
                            <div class="col-lg-2">
                                <asp:Button runat="server" ID="btnSave" CssClass="btn btn-primary" Width="100px"
                                    Text="SAVE" OnClick="btnSave_Click" ValidationGroup="InsertUpdateDish">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%--END OF FORM DISH--%>

            <div class="row">
                <div class="table-header">
                    <div class="table-header-title">
                        DISHES
                    </div>
                    <div class="table-header-button">
                        <asp:Button ID="btnAdd" runat="server" Text="ADD" CssClass="btn btn-primary" Width="100px"
                            OnClick="btnAdd_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="DELETE" CssClass="btn btn-danger" Width="100px"
                            OnClick="btnDelete_Click" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="table-main col-sm-12">
                    <asp:HiddenField ID="hdfDeletedDishes" runat="server" />
                    <asp:Repeater ID="rptDish" runat="server" OnItemDataBound="rptDish_ItemDataBound" OnItemCommand="rptDish_ItemCommand">
                        <HeaderTemplate>
                            <table id="htblDish" class="table">
                                <thead>
                                    <tr role="row">
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1"
                                            tabindex="0" class="sorting_asc center">
                                        </th>
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1" tabindex="0"
                                            class="sorting_asc text-center">
                                            Dish Name
                                        </th>
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1" tabindex="0"
                                            class="sorting_asc text-center">
                                            Dish Type
                                        </th>
                                        <th aria-sort="ascending" style="" colspan="1" rowspan="1" tabindex="0"
                                            class="sorting_asc text-center">
                                            Price
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="odd" role="row" runat="server" onclick="">
                                <td>
                                    <div style="text-align: center;">
                                        <asp:CheckBox ID="chkChoose" CssClass="checkDelete" runat="server">
                                        </asp:CheckBox>
                                    </div>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbDishName" runat="server" CommandName="EDIT"></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:Literal ID="litDishType" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litPrice" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> 
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>