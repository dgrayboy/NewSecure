<%@ Page Title="" Language="C#" MasterPageFile="~/NewSecurePrologue.Master" AutoEventWireup="true" CodeBehind="NewSecure.aspx.cs" Inherits="NewSecure.Test1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* 
	        Cusco Sky table styles
	        written by Braulio Soncco http://www.buayacorp.com
        */

        table, th, td {
	        border: 1px solid #D4E0EE;
	        border-collapse: collapse;
	        font-family: "Trebuchet MS", Arial, sans-serif;
	        font-size: 75%;
	        color: #555;
        }

        caption {
	        font-size: 150%;
	        font-weight: bold;
	        margin: 5px;
        }

        td, th {
	        padding: 4px;
        }

        thead th {
	        text-align: center;
	        background: #E6EDF5;
	        color: #4F76A3;
	        font-size: 100% !important;
        }

        tbody th {
	        font-weight: bold;
        }

        tbody tr { background: #FCFDFE; }

        tbody tr.odd { background: #F7F9FC; }

        table a:link {
	        color: #718ABE;
	        text-decoration: none;
        }

        table a:visited {
	        color: #718ABE;
	        text-decoration: none;
        }

        table a:hover {
	        color: #718ABE;
	        text-decoration: underline !important;
        }

        tfoot th, tfoot td {
	        font-size: 85%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" runat="server"
    ContentPlaceHolderID="ContentPlaceHolder3">
    <div id="main">
        <!-- Today -->
        <section id="today" class="one">
            <div class="container">
                   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel runat="server" ID="dropdownlist">
                    <ContentTemplate>
                    <div style=" width:90%; float:left">
                        <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem disabled="disabled">Please choose a user</asp:ListItem>
                        <asp:ListItem Value="WebSecurityNews">@WebSecurityNews</asp:ListItem>
                        <asp:ListItem Value="ITSecurityWatch">@ITSecurityWatch</asp:ListItem>
                        <asp:ListItem Value="SecurityIT">@SecurityIT</asp:ListItem>
                    </asp:DropDownList>
                         </div>
                    <div style=" float:left;"><asp:DropDownList ID="tweetnum" runat="server" ToolTip="Select the number of Tweets to fetch. Do note the higher the number, the longer the loading time."  AutoPostBack="True">
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem>80</asp:ListItem>
                        <asp:ListItem>120</asp:ListItem>
                        <asp:ListItem>160</asp:ListItem>
                        <asp:ListItem>200</asp:ListItem>
                    </asp:DropDownList>
                    </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="dropdownlist">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: justify; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.85;">
                            <asp:Image ID="dropdownloading" runat="server" ImageUrl="/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: absolute; top: 5%; left: 50%;" />
                        </div>
                        </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: justify; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.85;">
                            <asp:Image ID="imgUpdateProgress1" runat="server" ImageUrl="/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 15%; left: 50%;" />
                        </div>
                        <%--<div id="overlay">
                            <div id="modalprogress">
                                <div id="theprogress">
                                    <img src="images/matryoshka.gif" width="700" height="700" alt="My Image"/>
                                    Please wait...
                                </div>
                            </div>
                        </div>--%>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Chart ID="Chart1" runat="server" Height="700px" Palette="SemiTransparent"
                            Width="700px">
                            <Titles>
                                <asp:Title Name="All Tweets" Font="Arial, 18pt, style=Bold" Text="All Tweets"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="false" Name="Legend" LegendStyle="Row"></asp:Legend>
                            </Legends>
                            <Series>
                                <asp:Series ChartType="Pie" Name="Series1"
                                    Font="Cambria, 14pt, style=Bold, Italic">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>


                        <asp:ListView ID="ListView1" runat="server">
                            <LayoutTemplate>                               
                                <table id="Table1" runat="server">
                                    <tr id="Tr1" runat="server">
                                        <th><b>Category</b> </th>
                                        <th><b>Tweet</b> </th>
                                        <th><b>Date/Time</b> </th>
                                    </tr>
                                    <tr id="itemPlaceholder">
                                    </tr>
                                </table>
                                 <asp:DataPager ID="DataPager1" runat="server" PageSize="5" OnPreRender="DataPager1_PreRender">
                                    <Fields>
                                        <asp:NumericPagerField ButtonType="Button" />
                                    </Fields>
                                </asp:DataPager>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval ("category") %></td>
                                    <td><%# Eval ("url") != null ? "<a href=" + Eval("url") + ">" + Eval("tweet") + "</a>" : Eval("tweet") %></td>
                                    <td><%# Eval ("date") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%-- <footer>
            <a href="#network" class="button scrolly">Continue to Network</a>
            </footer> --%>
            </div>
        </section>

        <!-- Network -->
        <section id="network" class="two">
            <div class="container">
                <header>
                    <h2>Network</h2>
                </header>

                <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel2">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: justify; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.85;">
                            <asp:Image ID="imgUpdateProgress2" runat="server" ImageUrl="/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 35.5%; left: 50%;" />
                        </div>
                        <%--<div id="overlay">
                            <div id="modalprogress">
                                <div id="theprogress">
                                    <asp:Image ID="imgWaitIcon2" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/Cover.jpg" />
                                    Please wait...
                                </div>
                            </div>
                        </div>--%>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Chart ID="Chart2" runat="server" Height="700px" Palette="SemiTransparent"
                            Width="700px">
                            <Titles>
                                <asp:Title Name="Network Tweets" Font="Arial, 18pt, style=Bold" Text="Network Tweets"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="false" Name="Legend" LegendStyle="Row"></asp:Legend>
                            </Legends>
                            <Series>
                                <asp:Series ChartType="Pie" Name="Series1"
                                    Font="Cambria, 14pt, style=Bold, Italic">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>

                        <asp:ListView ID="ListView2" runat="server">
                            <LayoutTemplate>                                
                                <table id="Table1" runat="server">
                                    <tr id="Tr1" runat="server">
                                        <th><b>Category</b> </th>
                                        <th><b>Tweet</b> </th>
                                        <th><b>DateTime</b> </th>
                                    </tr>
                                    <tr id="itemPlaceholder">
                                    </tr>
                                </table>
                                <asp:DataPager ID="DataPager1" runat="server" PageSize="5" OnPreRender="DataPager1_PreRender">
                                    <Fields>
                                        <asp:NumericPagerField ButtonType="Button" />
                                    </Fields>
                                </asp:DataPager>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval ("category") %></td>
                                    <td><%# Eval ("url") != null ? "<a href=" + Eval("url") + ">" + Eval("tweet") + "</a>" : Eval("tweet") %></td>
                                    <td><%# Eval ("date") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--<footer>
            <a href="#mobile" class="button scrolly">Continue to Mobile</a>
            </footer>--%>
            </div>
        </section>

        <!-- Mobile -->
        <section id="mobile" class="three">
            <div class="container">
                <header>
                    <h2>Mobile</h2>
                </header>

                <asp:UpdateProgress ID="UpdateProgress3" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel3">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: justify; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.85;">
                            <asp:Image ID="imgUpdateProgress3" runat="server" ImageUrl="/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 59%; left: 50%;" />
                        </div>
                        <%--<div id="overlay">
                            <div id="modalprogress">
                                <div id="theprogress">
                                    <asp:Image ID="imgWaitIcon3" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/Cover.jpg" />
                                    Please wait...
                                </div>
                            </div>
                        </div>--%>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Chart ID="Chart3" runat="server" Height="700px" Palette="SemiTransparent"
                            Width="700px">
                            <Titles>
                                <asp:Title Name="Mobile Tweets" Font="Arial, 18pt, style=Bold" Text="Mobile Tweets"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="false" Name="Legend" LegendStyle="Row"></asp:Legend>
                            </Legends>
                            <Series>
                                <asp:Series ChartType="Pie" Name="Series1"
                                    Font="Cambria, 14pt, style=Bold, Italic">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                        <asp:ListView ID="ListView3" runat="server">
                            <LayoutTemplate>                                
                                <table id="Table1" runat="server" border="1">
                                    <tr id="Tr1" runat="server">
                                        <th><b>Category</b> </th>
                                        <th><b>Tweet</b> </th>
                                        <th><b>Date/Time</b> </th>
                                    </tr>
                                    <tr id="itemPlaceholder">
                                    </tr>                                    
                                </table>
                                <asp:DataPager ID="DataPager1" runat="server" PageSize="5" OnPreRender="DataPager1_PreRender">
                                    <Fields>
                                        <asp:NumericPagerField ButtonType="Button" />
                                    </Fields>
                                </asp:DataPager>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval ("category") %></td>
                                    <td><%# Eval ("url") != null ? "<a href=" + Eval("url") + ">" + Eval("tweet") + "</a>" : Eval("tweet") %></td>
                                    <td><%# Eval ("date") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--<footer>
            <a href="#desktop" class="button scrolly">Continue to Desktop</a>
            </footer>--%>
            </div>
        </section>

        <!-- Desktop -->
        <section id="desktop" class="four">
            <div class="container">
                <header>
                    <h2>Desktop</h2>
                </header>

                <asp:UpdateProgress ID="UpdateProgress4" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel4">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: justify; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.85;">
                            <asp:Image ID="imgUpdateProgress4" runat="server" ImageUrl="/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 81%; left: 50%;" />
                        </div>
                        <%--<div id="overlay">
                            <div id="modalprogress">
                                <div id="theprogress">
                                    <asp:Image ID="imgWaitIcon4" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/Cover.jpg" />
                                    Please wait...
                                </div>
                            </div>
                        </div>--%>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:Chart ID="Chart4" runat="server" Height="700px" Palette="SemiTransparent"
                            Width="700px">
                            <Titles>
                                <asp:Title Name="Desktop Tweet" Font="Arial, 18pt, style=Bold" Text="Desktop Tweets"></asp:Title>
                            </Titles>
                            <Legends>
                                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="false" Name="Legend" LegendStyle="Row"></asp:Legend>
                            </Legends>
                            <Series>
                                <asp:Series ChartType="Pie" Name="Series1"
                                    Font="Cambria, 14pt, style=Bold, Italic">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                        
                        <asp:ListView ID="ListView4" runat="server">
                            <LayoutTemplate>       
                                <table id="Table1" runat="server" border="1">
                                    <tr id="Tr1" runat="server">
                                        <th><b>Category</b> </th>
                                        <th><b>Tweet</b> </th>
                                        <th><b>Date/Time</b> </th>
                                    </tr>
                                    <tr id="itemPlaceholder">
                                    </tr>
                                    </table>
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="5" OnPreRender="DataPager1_PreRender">
                                    <Fields>
                                        <asp:NumericPagerField ButtonType="Button" />
                                    </Fields>
                                    </asp:DataPager>
                                                                
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval ("category") %></td>
                                    <td><%# Eval ("url") != null ? "<a href=" + Eval("url") + ">" + Eval("tweet") + "</a>" : Eval("tweet") %></td>
                                    <td><%# Eval ("date") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </section>

        <!-- Others -->
        <section id="others" class="five">
            <div class="container">
                <header>
                    <h2>Others</h2>
                </header>

                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel5">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: justify; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.85;">
                            <asp:Image ID="imgUpdateProgress5" runat="server" ImageUrl="/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 90%; left: 50%;" />
                        </div>
                        <%--<div id="overlay">
                            <div id="modalprogress">
                                <div id="theprogress">
                                    <asp:Image ID="imgWaitIcon4" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/Cover.jpg" />
                                    Please wait...
                                </div>
                            </div>
                        </div>--%>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Always">
                    <ContentTemplate>

                        <asp:ListView ID="ListView5" runat="server">
                            <LayoutTemplate>
                                <table id="Table1" runat="server" border="1">
                                    <tr id="Tr1" runat="server">
                                        <th><b>Category</b> </th>
                                        <th><b>Tweet</b> </th>
                                        <th><b>Date/Time</b> </th>
                                    </tr>
                                    <tr id="itemPlaceholder">
                                    </tr>
                                </table>
                                <asp:DataPager ID="DataPager1" runat="server" PageSize="5" OnPreRender="DataPager1_PreRender">
                                    <Fields>
                                        <asp:NumericPagerField ButtonType="Button" />
                                    </Fields>
                                </asp:DataPager>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval ("category") %></td>
                                    <td><%# Eval ("url") != null ? "<a href=" + Eval("url") + ">" + Eval("tweet") + "</a>" : Eval("tweet") %></td>
                                    <td><%# Eval ("date") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                </asp:UpdatePanel>  
                </div>
        </section> 
    </div>
</asp:Content>


<asp:Content ID="Content3" runat="server"
    ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="top">

        <!-- Twitter Picture -->
        <div id="logo">
            <span class="image avatar48">
                <img src="images/dp.jpg" id="profilepic" alt="" runat="server" /></span>
            <h1 name="username" id="username" runat="server"></h1>
        </div>

        <!-- Navigation List -->
        <nav id="nav">
            <ul>
                <li><a href="#today" id="today-link" class="skel-panels-ignoreHref">
                    <span class="fa fa-th">Today</span></a></li>
                <li><a href="#network" id="network-link" class="skel-panels-ignoreHref">
                    <span class="fa fa-th">Network</span></a></li>
                <li><a href="#mobile" id="mobile-link" class="skel-panels-ignoreHref">
                    <span class="fa fa-th">Mobile</span></a></li>
                <li><a href="#desktop" id="desktop-link" class="skel-panels-ignoreHref">
                    <span class="fa fa-th">Desktop</span></a></li>
                <li><a href="#others" id="other-link" class="skel-panels-ignoreHref">
                    <span class="fa fa-th">Others</span></a></li>
            </ul>
        </nav>
    </div>
</asp:Content>
