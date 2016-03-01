Add-PSSnapin -Name Microsoft.SharePoint.Powershell

$web = get-spweb "http://vmsp2013dev"
$list = $web.Lists["InterAction Banner"]
$list.BreakRoleInheritance($true)
$list.AnonymousPermMask64= $list.AnonymousPermMask64 -bor ([int][Microsoft.SharePoint.SPBasePermissions]::AnonymousSearchAccessList) #binary or adding the permissions
$list.Update()
