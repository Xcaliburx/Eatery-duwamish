<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagingControl.ascx.cs" Inherits="EateryDuwamish.UserControl.PagingControl" %>
<asp:Panel ID="panelPaging" runat="server">
    <div class="row">
        <div class="col-sm-6">
            <asp:Label ID="lblText" class="dataTables_info" role="status" aria-live="polite" runat="server" />
        </div>
        <div class="col-sm-12">
            <div class="dataTables_paginate paging_simple_numbers" id="Div2">
                <ul class="pagination">
                    <li class="paginate_button previous" ID="liPrev" runat="server" style="margin-left:2px;">
		                <asp:LinkButton ID="lbPrev" runat="server" OnClick="lbPrev_Click" Text="Previous"
                        OnClientClick="window.document.forms[0].target='_self';"/>
                    </li>
                    
                    <li class="paginate_button" ID="liFirst" runat="server" style="margin-left:2px;">
                        <asp:LinkButton ID="lbFirst" runat="server" Text="1" OnClick="lbFirst_Click" 
                        OnClientClick="window.document.forms[0].target='_self';"/>
                    </li>
                    <li class="paginate_button disabled" ID="liPreviousJump" runat="server" visible="false" style="margin-left:2px;">
                       <a>...</a>
                    </li>

                    <asp:Repeater ID="rptPaging" runat="server" OnItemDataBound="rptPaging_ItemDataBound" OnItemCommand="rptPaging_ItemCommand">
                        <ItemTemplate>
                            <li class="paginate_button" ID="liPage" runat="server" style="margin-left:2px;">
		                        <asp:LinkButton ID="lbNo" runat="server" 
                            OnClientClick="window.document.forms[0].target='_self';" />
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>

                    <li class="paginate_button disabled" ID="liNextJump" runat="server" visible="false" style="margin-left:2px;">
                        <a>...</a>
                    </li>
                    <li class="paginate_button" ID="liLast" runat="server" style="margin-left:2px;">
                        <asp:LinkButton ID="lbLast" runat="server" class="item" OnClick="lbLast_Click" 
                            OnClientClick="window.document.forms[0].target='_self';" />
                    </li>

		            <li class="paginate_button next" ID="liNext" runat="server" style="margin-left:2px;">
		                <asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click"
                            OnClientClick="window.document.forms[0].target='_self';" Text="Next"/>
                    </li>
                 </ul>
            </div>
         </div>
    </div>
</asp:Panel>