<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteSummaryListUserControl.ascx.cs" Inherits="Akumina.InterAction.SiteSummaryListWebPart.SiteSummaryList.SiteSummaryListUserControl" %>

<asp:Literal runat="server" ID="litTop"></asp:Literal>
<%--<div class="interAction">
		<div class="ia-transformer-tabs">

			<nav role='navigation' class="ia-transformer-tab-nav">
				<ul>
					<li class="ia-tab-active"><a href="#" class="ia-tab-active-link">New</a></li>
					<li><a href="#">My Recent</a></li>
					<li><a href="#">Popular</a></li>
					<li><a href="#">Recommended</a></li>
				</ul>
			</nav>

			<div class="ia-single-tab ia-tab-active-link">

				<!-- start site summary -->
				<div class="ia-site-summary">
					<table class="ia-site-summary-list tablesaw tablesaw-stack" data-mode="stack">
						<colgroup>
							<col style="ia-col-width-70" />
							<col style="ia-col-width-30" />
						</colgroup>

						<thead>
							<tr>

								<th>Site Name</th>

								<th>Date Available</th>
							</tr>
						</thead>
						<tbody>
							<tr>
								<td><a href="#">Marketing</a></td>
								<td>03/03/2015</td>
							</tr>
							<tr>
								<td><a href="#">HR</a></td>
								<td>03/02/2015</td>
							</tr>
							<tr>
								<td><a href="#">IT</a></td>
								<td>03/01/2015</td>
							</tr>
							<tr>
								<td><a href="#">Finance</a></td>
								<td>02/26/2015</td>
							</tr>
							<tr>
								<td><a href="#">Sales</a></td>
								<td>02/24/2015</td>
							</tr>

						</tbody>
					</table>
				</div>
				<!-- end site summary -->

			</div>

			<div class="ia-single-tab">

				<!-- start site summary -->
				<div class="ia-site-summary">
					<table class="ia-site-summary-list tablesaw tablesaw-stack" data-mode="stack">
						<colgroup>
							<col style="ia-col-width-70" />
							<col style="ia-col-width-30" />
						</colgroup>

						<thead>
							<tr>

								<th>Site Name</th>

								<th>Date Available</th>
							</tr>
						</thead>
						<tbody>
							
							<tr>
								<td><a href="#">HR</a></td>
								<td>03/02/2015</td>
							</tr>
							<tr>
								<td><a href="#">IT</a></td>
								<td>03/01/2015</td>
							</tr>
							<tr>
								<td><a href="#">Marketing</a></td>
								<td>03/03/2015</td>
							</tr>
							<tr>
								<td><a href="#">Finance</a></td>
								<td>02/26/2015</td>
							</tr>
							<tr>
								<td><a href="#">Sales</a></td>
								<td>02/24/2015</td>
							</tr>

						</tbody>
					</table>
				</div>
				<!-- end site summary -->
				
				
				

			</div>

			<div class="ia-single-tab">
				
				<!-- start site summary -->
				<div class="ia-site-summary">
					<table class="ia-site-summary-list tablesaw tablesaw-stack" data-mode="stack">
						<colgroup>
							<col style="ia-col-width-70" />
							<col style="ia-col-width-30" />
						</colgroup>

						<thead>
							<tr>

								<th>Site Name</th>

								<th>Date Available</th>
							</tr>
						</thead>
						<tbody>
							
							
							<tr>
								<td><a href="#">Sales</a></td>
								<td>02/24/2015</td>
							</tr><tr>
								<td><a href="#">HR</a></td>
								<td>03/02/2015</td>
							</tr>
							<tr>
								<td><a href="#">IT</a></td>
								<td>03/01/2015</td>
							</tr>
							<tr>
								<td><a href="#">Marketing</a></td>
								<td>03/03/2015</td>
							</tr>
							<tr>
								<td><a href="#">Finance</a></td>
								<td>02/26/2015</td>
							</tr>
							

						</tbody>
					</table>
				</div>
				<!-- end site summary -->

			</div>

			<div class="ia-single-tab">
				

				<!-- start site summary -->
				<div class="ia-site-summary">
					<table class="ia-site-summary-list tablesaw tablesaw-stack" data-mode="stack">
						<colgroup>
							<col style="ia-col-width-70" />
							<col style="ia-col-width-30" />
						</colgroup>

						<thead>
							<tr>

								<th>Site Name</th>

								<th>Date Available</th>
							</tr>
						</thead>
						<tbody>
							
							
							<tr>
								<td><a href="#">IT</a></td>
								<td>03/01/2015</td>
							</tr>
							<tr>
								<td><a href="#">Sales</a></td>
								<td>02/24/2015</td>
							</tr><tr>
								<td><a href="#">HR</a></td>
								<td>03/02/2015</td>
							</tr>
							<tr>
								<td><a href="#">Marketing</a></td>
								<td>03/03/2015</td>
							</tr>
							<tr>
								<td><a href="#">Finance</a></td>
								<td>02/26/2015</td>
							</tr>
							

						</tbody>
					</table>
				</div>
				<!-- end site summary -->

			</div>

		</div><!-- End ia-transformer-tabs -->
	</div><!-- End InterAction -->--%>
