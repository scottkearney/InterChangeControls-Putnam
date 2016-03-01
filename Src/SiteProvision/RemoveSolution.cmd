powershell -Command "& {Set-ExecutionPolicy bypass}" -NoExit
powershell -Command "& {.\SPSolutionDeploymentScript.ps1 -action:SR}" -NoExit

pause