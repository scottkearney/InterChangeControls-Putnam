param([string]$solutionName = "")

if((Get-PSSnapin | Where { $_.Name -eq "Microsoft.SharePoint.Powershell" }) -eq $null)
{      
    Add-PSSnapin Microsoft.SharePoint.PowerShell
}
       
"Solution Name:"+$solutionName

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
      
}
catch
{      
   Exit 1
}
      