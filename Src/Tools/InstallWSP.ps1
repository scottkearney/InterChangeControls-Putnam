param([string]$solutionPath = "", [string]$solutionName = "", [string]$siteUrl = "", [string]$featureName = "")
if((Get-PSSnapin | Where { $_.Name -eq "Microsoft.SharePoint.Powershell" }) -eq $null)
{      
    Add-PSSnapin Microsoft.SharePoint.PowerShell
}
       
"Solution Path:"+$solutionPath
"Solution Name:"+$solutionName
"Site Collection:"+$siteUrl
"Feature Identity:"+$featureName   
try
{
    $existingSolution = Get-SPSolution | Where { $_.Name -eq $solutionName -and $_.Deployed -eq $true }
         
    if($existingSolution -ne $null)
    {              
        if($existingSolution.DeploymentState -eq "GlobalDeployed")
        {
            Uninstall-SPSolution -Identity $solutionName -Confirm:$false;          
       
            while (($timer = Get-SPTimerJob | ?{ $_.Name -like "*solution-deployment*" + $solutionName + "*" }) -eq $null) 
            {
                Start-Sleep -Milliseconds 100;
            }
      
            $timerName = $timer.Name
     
            while ((Get-SPTimerJob $timerName) -ne $null) 
            {   
                Start-Sleep -Seconds 1;
            }
        }
    }
      
    $existingSolution = Get-SPSolution | Where { $_.Name -eq $solutionName -and $_.Deployed -eq $false }
      
    if($existingSolution -ne $null)
    {
        Remove-SPSolution -Identity $solutionName -Confirm:$false -Force;          
    }       
      
    $solutionLiteralPath = "$solutionPath\$solutionName";
      
    Add-SPSolution -LiteralPath $solutionLiteralPath -Confirm:$false     
      
    Install-SPSolution -Identity $solutionName -GACDeployment -Confirm:$false -Force  -AllWebApplications

    Enable-SPFeature -Url $siteUrl -Identity $featureName
}
catch
{      
   Exit 1
}
      