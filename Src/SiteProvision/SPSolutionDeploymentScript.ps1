<# 
.DESCRIPTION 
    - Run this script to deploy/retract the farm level solution and activate/deactivate features
 
.GOAL
 - Build the most generic farm level soution deployment script
  
.ASSUMPTION
 - Batch files, log files, and WSPs exists in same directory during deployment process
  
.MAJOR FEATURES IMPLEMENTED
 - Parameters passed in from the batch file
 - Loading SharePoint PowerShell Plugin - Check if its already been loaded
 - Logging of Powershell script execution transcript, New file gets created for each run
 - Solution Deploy or Retract in single click
 - Waiting for  timer job execution while deploying and retracting solution
 - Nullable $siteFeatureIds and $webFeatureIds parameters to support deployment only option, no activation of features
 - Support Multiple Site and Web Level Features - Site feature and web features are passed in as array paramters
 - Support Activation of features at Multiple Webs - Activate/Deactivate Web Level features on all webs in site collection
 - Check for webapplication level resources to deploy or retract solutions at the web application level or all web application level
 - Prompt the script execution details with different color codes - green for major steps, yellow for detailed info, and red for errors
 
#>

 
 
param (

 [string]$solutionName =  $null, #"$(Read-Host 'Enter the Solution WSP Name. [e.g. Sample.wsp]')",
 [string]$webAppUrl =  $null,#"$(Read-Host 'Enter the Web Application URL.')",
 [string]$siteUrl = $null,#"$(Read-Host 'Enter the Site Collection URL.')",
 [string[]]$siteFeatureIds = $null, #"$(Read-Host 'Enter the Site Level Features. ')",
 [string[]]$webFeatureIds = $null, #"$(Read-Host 'Enter the Web Level Features.')",
 [string]$logFileName = $null,# "$(Read-Host 'Enter the Log File Name. ')",
 [string]$action = "$(Read-Host 'Enter [SD] to deploy solutions or [SR] to retract the solution')"
)


 
# Defination of main function
function main() {

 # find the current directory
 $currentDirectory = Get-Location
 $todayDate = Get-Date -UFormat "%m-%d-%Y at %H-%M" 
 $logFilePath = "$currentDirectory\Logs\$action-$todayDate.txt"
 
 $check = Test-Path -PathType Container Logs
if($check -eq $false){
    New-Item 'Logs' -type Directory
}

 # create new log file and start logging
 Start-Transcript $logFilePath
 
 # check to ensure Microsoft.SharePoint.PowerShell is loaded
 $snapin = Get-PSSnapin | Where-Object {$_.Name -eq 'Microsoft.SharePoint.Powershell'}
 if ($snapin -eq $null)
 {
  Write-Host "Loading SharePoint Powershell Snapin"
  Add-PSSnapin "Microsoft.SharePoint.Powershell"
 }

$services = Get-SPServiceInstance | Select-Object -Property Service,Status
[System.Collections.ArrayList]$NotStartedServices = @()
$dict = New-Object 'system.collections.generic.dictionary[[string],[system.collections.generic.list[string]]]'
ForEach($value in $services) {
[string]$name=$value.Service
[string]$status=$value.Status
$dict.Add($name,$status)
}
$userProfileService=$dict.Get_Item("UserProfileService")
$metadataService=$dict.Get_Item("MetadataWebService")
if($userProfileService)
{
if($userProfileService -ne "Online"){
$test=$NotStartedServices.Add("UserProfileService")
}
}
else
{
$test=$NotStartedServices.Add("UserProfileService")
}
if($metadataService)
{
if($metadataService -ne "Online"){
$test=$NotStartedServices.Add("MetadataWebService")
}
}
else
{
$test=$NotStartedServices.Add("MetadataWebService")
}
if($NotStartedServices.Count -gt 0)
{

Write-Host "Please enable the following services in central administration to proceed.."
 for($i=0; $i -lt $NotStartedServices.Count; $i++)
{
$j=1+$i
Write-Host $j. $NotStartedServices[$i]
}
}

else{

	#Get WebApplication Settings
	[xml] $xdoc = get-content ".\Settings\WebSettings.xml"
	$getWebApplication=$xdoc.SelectSingleNode("//Web/WebApplication")
	$siteCollections=$xdoc.SelectNodes("//Web/SiteCollections/SiteCollection")
	$WebApplicationRemove=$xdoc.SelectSingleNode("//Web/RemoveFromWebApplication").InnerText.ToLower()
	$webAppUrl = $getWebApplication.Url#"$(Read-Host 'Enter the Web Application URL.')",

	# Get WSP Files
	[xml] $xdoc = get-content ".\Settings\WSPSettings.xml"
	$wspCollections=$xdoc.SelectNodes("//WSPS/WSP")
	
	#Get Feature Settings
	[xml] $xdocFeature = get-content ".\Settings\FeatureSettings.xml"
	$siteFeatures=$xdocFeature.SelectNodes("//Features/SiteFeatures/Feature")
	
	$SiteFeatureIdMapping = @{}
	

	ForEach($value in $siteFeatures) {
		$getFeatureIds=$value.SelectNodes("FeatureIds/FeatureId")
		[string]$featureWSP=$value.SelectSingleNode("WSP").InnerText.ToLower().Trim()
		[System.Collections.ArrayList]$FeatureIds =@()
		
			ForEach($feautureValue in $getFeatureIds) {
			$ok=$FeatureIds.Add($feautureValue.InnerText.ToLower().Trim())
			}

		$SiteFeatureIdMapping.Add($featureWSP,$FeatureIds)
	}
	
	$webFeatures=$xdocFeature.SelectNodes("//Features/WebFeatures/Feature")
	
	$WebFeatureIdMapping = @{}
	

	ForEach($value in $webFeatures) {
		$getFeatureIds=$value.SelectNodes("FeatureIds/FeatureId")
		[string]$featureWSP=$value.SelectSingleNode("WSP").InnerText.ToLower().Trim()
		[System.Collections.ArrayList]$FeatureIds =@()
		
			ForEach($feautureValue in $getFeatureIds) {
			$ok=$FeatureIds.Add($feautureValue.InnerText.ToLower().Trim())
			}

		$WebFeatureIdMapping.Add($featureWSP,$FeatureIds)
	}
	
	

 # deploy the solution
 if ($action -eq "SD")
 {
 
  if($siteCollections.Count -gt 0)
  {
 
    # $solutionNameSite="Akumina.SiteDefinition.Provision.wsp"
	
    ForEach($file in $wspCollections)
    {
		 $solutionName =$file.InnerText.ToLower().Trim()
		 $wspFile = Test-Path $currentDirectory\WSP\$solutionName
		 if($wspFile){
		
			
			  Write-Host "Execute Deployment steps for the Solution"  $solutionName  -foregroundcolor Green
				
			  Write-Host "Step 1 - Add Solution Package: " $solutionName -foregroundcolor Green
			  AddSolution
			   
			  Write-Host "Step 2 - Deploy Solution Package: " $solutionName -foregroundcolor Green
			  InstallSolution
			  
			  Write-Host "Step 3 - Timer Job to deploy the Solution Package" -foregroundcolor Green
			  Wait4TimerJob
		  
		  }
		  else{
		  Write-Host The $wspFile file is not located in the WSP Folder
		  }
	}
	# $solutionName = $solutionNameSite
	# $solutionIsthere= Test-Path $currentDirectory\WSP\$solutionName
	# if($solutionIsthere1)
	# {
		# Write-Host "Step 1 - Add Solution Package: " $solutionName -foregroundcolor Green
	   # AddSolution
	   
	  # Write-Host "Step 2 - Deploy Solution Package: " $solutionName -foregroundcolor Green
	   # InstallSolution
	   
	  # Write-Host "Step 3 - Timer Job to deploy the Solution Package" -foregroundcolor Green
	   # Wait4TimerJob
	# }
	 foreach($siteCollection in $siteCollections){
			$siteUrl = $siteCollection.Url			
			$site = Get-SPSite $siteUrl -ErrorAction SilentlyContinue
			if ($site)
			{
			# Write-Host "Site [$siteUrl] already exists. Change in xml and try again.." -f Cyan
			#$siteExits="true"
			}
			
		}
  }

  #if($siteExits -eq "false"){
	$template =Get-SPWebTemplate | where-object {$_.Title -eq "Akumina Site Definition"}
	if ($template -ne $null)
	{
		    foreach($siteCollection in $siteCollections){
			$siteUrl = $siteCollection.Url
			
			$site = Get-SPSite $siteUrl -ErrorAction SilentlyContinue
			if ($site)
			{
			if($siteCollection.SiteExists.ToLower() -eq "yes"){
			UpdateActivateFeatures
			}
			}
			else
		   {
		   if($siteCollection.SiteExists.ToLower() -eq "yes"){
			Write-Host This site Collection $siteUrl, does not exist set "<SiteExists>" to NO "in" the "webSettings.xml" file. -foregroundcolor Green
			}
			else
			{
		   Write-Host Creating a Site Collection $siteCollection.Url
		   if($siteCollection.HostHeaderWebApplication.ToLower() -eq "yes")
		   {
		   $w = Get-SPWebApplication $webAppUrl
		   New-SPSite -Url $siteUrl -OwnerAlias $siteCollection.Admin -Name $siteCollection.Title -HostHeaderWebApplication $w -Template $template
		   }
		   else
		   {
		   New-SPSite -Url $siteUrl -OwnerAlias $siteCollection.Admin -Name $siteCollection.Title -Template $template
		   Wait4TimerJob
		   }
		   }
		   }
		}
	}

 #}

} 
  
 
 
  
 # retract the solution
 if ($action -eq "SR")
 {
	 #Check
	 if($WebApplicationRemove -eq "yes"){
	 $siteCollections=Get-SPWebApplication $webAppUrl | Get-SPSite -limit all 
	 }
 	foreach($siteCollection in $siteCollections)
	{
	
	$siteUrl = $siteCollection.Url
	
   
    Write-Host "Step  - Remove WebParts and lists" -foregroundcolor Green
    RemoveListAndWebParts $siteUrl 
  
	Write-Host "Step  - Setting default master page" -foregroundcolor Green
	SetMasterPage $siteUrl 
	
    ForEach($file in $wspCollections)
    {
		 $solutionName =$file.InnerText.ToLower().Trim()		 
		 
		  $siteFeatureIds=$SiteFeatureIdMapping.Get_Item($solutionName.ToLower())	 
		 if($siteFeatureIds){
			  Write-Host "Execute Retract steps for the Solution "  $solutionName  -foregroundcolor Blue	
		  Write-Host "Step 2 - Deactivate Site Collection Level Features" -foregroundcolor Green
		  DeactivateSiteFeatures
		  }
		  else
		  {
		   $siteFeatureIds=$WebFeatureIdMapping.Get_Item($solutionName.ToLower())	 
			if($siteFeatureIds){
		
		  Write-Host "Execute Retract steps for the Solution "  $solutionName  -foregroundcolor Blue			
		  Write-Host "Step 1 - Deactivate Web Level Features" -foregroundcolor Green
		  DeactivateWebFeatures
			}
		  }
		   
		 
				
	}
	}
	if($WebApplicationRemove -eq "yes"){
		 ForEach($file in $wspCollections)
			{
			  $solutionName  = $file.InnerText.ToLower().Trim()		 
			  Write-Host "Step 3 - Uninstall Solution Package: " $solutionName -foregroundcolor Green
			  UnInstallSolution
			   
			  Write-Host "Step 4 - Timer Job to Retract the Solution Package" -foregroundcolor Green
			  Wait4TimerJob
			   
			  Write-Host "Step 5 - Remove Solution Package: " $solutionName -foregroundcolor Green
			  RemoveSolution		  
			
		  }

		}
	
}
 # stop the logging
 Stop-Transcript
}
}
function SetMasterPage()
{

$site = Get-SPSite $siteUrl

foreach ($web in $site.AllWebs)
{
 
   #$web.MasterUrl = $site.RootWeb.ServerRelativeUrl +  "/_catalogs/masterpage/seattle.master";
   $web.CustomMasterUrl = $site.RootWeb.ServerRelativeUrl.TrimEnd("/", " ") +  "/_catalogs/masterpage/seattle.master";
  
   $web.Update();
   
}
}

function UpdateActivateFeatures()
{

$site = Get-SPSite $siteUrl

foreach ($web in $site.AllWebs)
{
	
		
			$siteFeatureIds=$SiteFeatureIdMapping.Get_Item("others")			
			ActivateSiteFeatures
			
			$siteFeatureIds=$WebFeatureIdMapping.Get_Item("others")	
			ActivateWebFeatures
			
			$solutionNameSite="Akumina.SiteDefinition.Provision.wsp"
			
			foreach($value in $SiteFeatureIdMapping.GetEnumerator()){
			if($value.Name -ne "others" )
			{
		
			$siteFeatureIds=$value.Value
			ActivateSiteFeatures
			}
			}
			
			foreach($value in $WebFeatureIdMapping.GetEnumerator()){
			if($value.Name -ne "others" )
			{
			 if($value.Name -ne $solutionNameSite.ToLower()){
			
			$siteFeatureIds=$value.Value			
			ActivateWebFeatures
			}
			}
			}
			 
			$siteFeatureIds=$WebFeatureIdMapping.Get_Item($solutionNameSite.ToLower())		    
			ActivateWebFeatures
			
			
			
		
			#$web.CustomMasterUrl = $site.RootWeb.ServerRelativeUrl.TrimEnd("/", " ") +  "/_catalogs/masterpage/AkuminaSpark/AkuminaSpark.master";
  
			#$web.Update();

		
			}
 
  
   

}

function RemoveListAndWebParts([string]$sitecollectionUrl)
{
$listWspIsFolder= Test-Path $currentDirectory\WSP\$solutionListDefintion
if($listWspIsFolder)
{
[xml] $xdoc = get-content ".\Settings\ListSettings.xml"

$deleteLists=$xdoc.SelectNodes("//List")
$deleteContentTypes=$xdoc.SelectNodes("//ContentType")
$deleteSiteColumns=$xdoc.SelectNodes("//SiteColumn")
$deleteWebParts=$xdoc.SelectNodes("//WebPart")
$ListItemOnlyDelete =$xdoc.SelectSingleNode("//Lists/ListItemOnlyDelete").InnerText.ToLower()
   $site = New-Object Microsoft.SharePoint.SPSite($sitecollectionUrl)
    $web=$site.OpenWeb()


    # Delete our Lists so we can Delete the ContentTypes
   foreach($value in $deleteLists)
    {
        $list=$web.Lists[$value.InnerText]
		
		 if($list) {
			if($ListItemOnlyDelete -eq "yes"){
			$collListItems = $list.Items; 
			$count = $collListItems.Count - 1
			for($intIndex = $count; $intIndex -gt -1; $intIndex--) 
			{ 	
					$collListItems.Delete($intIndex); 
			}
			}
			else
			{	
				write-host "Deleting list" $list.Title
				$list.Delete();        
			}
		}

    }
    

    # Empty our Recyle Bin so we can delete our ContentTypes
    for ($i=0;$i -lt $site.allwebs.count;$i++)
    {
      write-host $site.allwebs[$i].url "...emptying recyle bin";
      $site.allwebs[$i].recyclebin.deleteall();
    }

    $site.recyclebin.deleteall();


    # Delete Our Content Types
   

   # foreach($value in $deleteContentTypes)
    # {
        # $contentTypeName = $value.InnerText
        # $ct = $web.ContentTypes[$contentTypeName] 
        # if ($ct) { 
            # $ctusage = [Microsoft.SharePoint.SPContentTypeUsage]::GetUsages($ct) 
            # foreach ($ctuse in $ctusage) { 
                # $list = $web.GetList($ctuse.Url) 
                # $contentTypeCollection = $list.ContentTypes; 
                # $contentTypeCollection.Delete($contentTypeCollection[$ContentType].Id);             
            # }
            # Write-host "Deleted Content Type $contentTypeName from site."  
            # $ct.Delete() 
        
            # } 
    # }




    # Collect our site columns so we can delete them
    $customSiteColumns = @();
	foreach($value in $deleteSiteColumns){
		foreach ($field in $site.rootweb.Fields ) {            
			if($field.Group.Equals($value)) {                     
				$customSiteColumns += $field.InternalName.ToString();        
			}     
		}  
	}    
	#Delete our Site Columns
    foreach ($column in $customSiteColumns ) {            
       $site.rootweb.Fields.Delete($column)        
    }

	

	Write-host "Delete start web part."  	
	$wpCatlog =[Microsoft.SharePoint.SPListTemplateType]::WebPartCatalog

	$list = $site.GetCatalog($wpCatlog)
	$wpID = New-Object System.Collections.ObjectModel.Collection[System.Int32]

	 foreach($value in $deleteWebParts)
    {
	foreach ($item in $list.Items)
	{
	  if($item.DisplayName.ToLower().Contains($value.InnerText.ToLower()))
	  {   $wpID.Add($item.ID) }
	}
	}


	foreach($wp in $wpID)
	{ 

	   $wpItem = $list.GetItemById($wp)
	   $wpItem.Delete()
	}
	$list.Update()    
	Write-host "Delete web parts ended."  	

    # Clean up our objects
    $web.Dispose();
    $site.Dispose();
}
}
function DeleteField([string]$siteUrl, [string]$fieldName) {
    Write-Host "Start removing field:" $fieldName -ForegroundColor DarkGreen
    $site = Get-SPSite $siteUrl
    $web = $site.RootWeb

    #Delete field from all content types
    foreach($ct in $web.ContentTypes) {
        $fieldInUse = $ct.FieldLinks | Where {$_.Name -eq $fieldName }
        if($fieldInUse) {
            Write-Host "Remove field from CType:" $ct.Name -ForegroundColor DarkGreen
            $ct.FieldLinks.Delete($fieldName)
            $ct.Update()
        }
    }

    #Delete column from all lists in all sites of a site collection
    $site | Get-SPWeb -Limit all | ForEach-Object {
       #Specify list which contains the column
        $numberOfLists = $_.Lists.Count
        for($i=0; $i -lt $_.Lists.Count ; $i++) {
            $list = $_.Lists[$i]
            #Specify column to be deleted
            if($list.Fields.ContainsFieldWithStaticName($fieldName)) {
                $fieldInList = $list.Fields.GetFieldByInternalName($fieldName)

                if($fieldInList) {
                    Write-Host "Delete column from " $list.Title " list on:" $_.URL -ForegroundColor DarkGreen

                 #Allow column to be deleted
                 $fieldInList.AllowDeletion = $true
                 #Delete the column
                 $fieldInList.Delete()
                 #Update the list
                 $list.Update()
                }
            }
        }
    }

    # Remove the field itself
    if($web.Fields.ContainsFieldWithStaticName($fieldName)) {
        Write-Host "Remove field:" $fieldName
        $web.Fields.Delete($fieldName)
    }

    $web.Dispose()
    $site.Dispose()
}
 
# Add the solution package
# Adds a SharePoint solution package to the farm Solution gallery, It doesn't deploy the uploaded solution yet.
function AddSolution()
{
 $solution = Get-SPSolution | where-object {$_.Name -eq $solutionName}
 if ($solution -eq $null)
 {
  Write-Host "Adding solution package" -foregroundcolor Yellow
  $solutionPath = "$currentDirectory\WSP\$solutionName"
  Add-SPSolution -LiteralPath $solutionPath -Confirm:$false
 }
}

# Update the solution package
# Update a SharePoint solution package to the farm Solution gallery, Its already deployed.
function UpdateSolution()
{
 $solution = Get-SPSolution | where-object {$_.Name -eq $solutionName}
 if ($solution -ne $null)
 {
  Write-Host "Updating solution package" -foregroundcolor Yellow
  $solutionPath = "$currentDirectory\WSP\$solutionName"
  Update-SPSolution -Identity $solutionName -LiteralPath $solutionPath -GACDeployment
 }
}
 
# Deploy the solution package 
# Deploys an installed SharePoint solution on all the WFE servers in the farm.
# Deploying solution in the farm installs the feature automatically. It installs all the features
# on each server at the farm, web, site collection, and site level but It doesn't activate them.
# Since it provisions the file on each WFE, it is a timer job.
function InstallSolution()
{

 $solution = Get-SPSolution | where-object {$_.Name -eq $solutionName}
 $solutionId = $solution.Id

 if ($solution -ne $null)
 {
  $solutionDeployed = Get-SPSolution -Identity $solutionId | where-object {$_.Deployed -eq "False"}
 
   if ( $solution.ContainsWebApplicationResource )
   {
  
  
	
	$exists="false"
	
	$applications=$solution.DeployedWebApplications	
	foreach($app in $applications){
	if($app.Url.Equals( $webAppUrl))
	{
	 $exists="true"
	}
	}
	
	if($exists -eq "false")
	{
	  Write-Host "Installing solution package to web application: " $webAppUrl -foregroundcolor Yellow
    Install-SPSolution -Identity $solutionName -WebApplication $webAppUrl -GACDeployment -Confirm:$false -Force
	}
	else{
	UpdateSolution
	}
	
   }
   else
   {
    if ($solutionDeployed -eq $null)
    {
    Write-Host "Installing solution package to all web applications" -foregroundcolor Yellow
    Install-SPSolution -Identity $solutionName -GACDeployment -Confirm:$false
	}
	else{
	UpdateSolution
	}
   }
  }
  
  
 }

 
# Activate the Site level features
function ActivateSiteFeatures()
{ 
 if ($siteFeatureIds -ne $null)
 {

 if($siteUrl -ne $null){ 
  $spSite = Get-SPSite $siteUrl  
  if($spSite){
  foreach($siteFeatureName in $siteFeatureIds)
  {
   Write-Host "Trying to Activate Site Collection Level Feature: " $siteFeatureName -foregroundcolor Yellow
   $siteFeature = Get-SPFeature -site $spSite.url | where-object {$_.Id -eq $siteFeatureName}
   if ($siteFeature -eq $null)
   {
   if($siteFeature.Scope -eq $null){
   
	  $webFeature = Get-SPFeature -web $spSite.url | where-object {$_.id -eq $siteFeatureName} 
    if ($webFeature -eq $null)
    {
	if($webFeature.Scope -eq $null){
	 Write-Host "Activating Site Level Features at " $spSite.url -foregroundcolor Yellow
    Enable-SPFeature –identity $siteFeatureName -URL $spSite.url -Confirm:$false
	}
	}
	}
   }
  }
}  
  $spSite.Dispose()
 }
 }
}

# Activate the Web level features
function ActivateWebFeatures()
{

 if ($siteFeatureIds -ne $null)
 {
 if($siteUrl -ne $null){
  $spSite = Get-SPSite $siteUrl
  
  #Cycle through all webs in the collection and activate all the features
  foreach($spWeb in $spSite.AllWebs)
  { 
   foreach($webFeatureName in $siteFeatureIds)
   {  
    Write-Host "Trying to Activate Web Level Features: " $webFeatureName -foregroundcolor Yellow
    $webFeature = Get-SPFeature -web $spWeb.url | where-object {$_.id -eq $webFeatureName} 
    if ($webFeature -eq $null)
    { if($webFeature.Scope -eq $null){
     Write-Host "Activating " $webFeatureName " at " $spWeb.url -foregroundcolor Yellow
     Enable-SPFeature –identity $webFeatureName -URL $spWeb.url -Confirm:$false -force
	 }
    }
   }
  }
   
  $spWeb.Dispose()
  $spSite.Dispose()
  }
 }
}
 
# Deactivate the Web level features
function DeactivateWebFeatures()
{

 if ($siteFeatureIds -ne $null)
 {
 if($siteUrl -ne $null){
  $spSite = Get-SPSite $siteUrl
  
  #Cycle through all webs in the collection and deactivate all the features
  foreach($spWeb in $spSite.AllWebs)
  { 
   foreach($webFeatureName in $siteFeatureIds)
   {  
    Write-Host "Trying to Deactivate Web Level Features: " $webFeatureName -foregroundcolor Yellow
    $webFeature = Get-SPFeature -web $spWeb.url | where-object {$_.Id -eq $webFeatureName} 
    if ($webFeature -ne $null)
    {
     Write-Host "Deactivating " $webFeatureName " at " $spWeb.url -foregroundcolor Yellow
     Disable-SPFeature –identity $webFeatureName -URL $spWeb.url -Confirm:$false -force
    }
   }
  }
   
  $spWeb.Dispose()
  $spSite.Dispose()
  }
 }
}
 
# Deactivate the Site level features
function DeactivateSiteFeatures()
{
 if ($siteFeatureIds -ne $null)
 {
 if($siteUrl -ne $null){
  $spSite = Get-SPSite $siteUrl
  foreach($siteFeatureName in $siteFeatureIds)
  {
   Write-Host "Trying to Deactivate Site Collection Level Feature: " $siteFeatureName -foregroundcolor Yellow
   $siteFeature = Get-SPFeature -site $spSite.url | where-object {$_.Id -eq $siteFeatureName}
   if ($siteFeature -ne $null)
   {
    Write-Host "Deactivating Site Level Features at " $spSite.url -foregroundcolor Yellow
    Disable-SPFeature –identity $siteFeatureName -URL $spSite.url -Confirm:$false -force
   }  
  } 
  $spSite.Dispose()
 }
 }
}
 
# Retract the solution package
# Retracts a deployed SharePoint solution from the farm entirely for all web application or given web application.
# This step removes files from all the front-end Web server.
# Please note that retracting solution in the farm uninstalls the feature automatically, if it hasn't uninstalled using UnInstall-SPFeature.
# Since it removes the file on each WFE, it is a timer job.
function UnInstallSolution()
{
 $solution = Get-SPSolution | where-object {$_.Name -eq $solutionName}
 $solutionId = $solution.Id
 if ($solution -ne $null)
 {
  $solutionDeployed = Get-SPSolution -Identity $solutionId | where-object {$_.Deployed -eq "True"}
  if ($solutionDeployed -ne $null)   
  { 
   if ( $solution.ContainsWebApplicationResource )
   {
    Write-Host "Retracting solution package from web application: " $webAppUrl -foregroundcolor Yellow
    UnInstall-SPSolution -Identity $solutionName -WebApplication $webAppUrl -Confirm:$false
   }
   else
   {
    Write-Host "Retracting solution package from all web applications" -foregroundcolor Yellow
    UnInstall-SPSolution -Identity $solutionName -Confirm:$false
   }
  }
 }
}
 
# Remove the solution package
# Deletes a SharePoint solution from a farm solution gallery
function RemoveSolution()
{
 $solution = Get-SPSolution | where-object {$_.Name -eq $solutionName}
 if ($solution -ne $null)
 {
 if($solution.Deployed -ne $true){
  Write-Host "Deleting the solution package" -foregroundcolor Yellow
  Remove-SPSolution $solutionName -Confirm:$false
}
 
 }
}
 
# Wait for timer job during deploy and retract
function Wait4TimerJob()
{
 $solution = Get-SPSolution | where-object {$_.Name -eq $solutionName}
 if ($solution -ne $null)
 {
  $counter = 1  
  $maximum = 300  
  $sleeptime = 5 
   
  Write-Host "Please wait while this timer job completes.   This may take some time…"
  while( ($solution.JobExists -eq $true ) -and ( $counter -lt $maximum ) )
  {     
   sleep $sleeptime 
   $counter++  
  }
   
  Write-Host "Finished the solution timer job"   
 }
}

function DeleteSiteCollection()
{
 Remove-SPSite -Identity $webAppUrl -GradualDelete -Confirm:$False
}
#Run the Main Function
main
#CreateSiteCollection