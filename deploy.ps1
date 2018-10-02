function Stop-AppPool ($webAppPoolName, [int]$secs) {
    $retvalue = $false
    $wsec = (get-date).AddSeconds($secs)
    Stop-WebAppPool -Name $webAppPoolName
    Write-Output "$(Get-Date) waiting up to $secs seconds for the WebAppPool '$webAppPoolName' to stop"
    $poolNotStopped = $true
    while (((get-date) -lt $wsec) -and $poolNotStopped) {
        $pstate = Get-WebAppPoolState -Name $webAppPoolName
        if ($pstate.Value -eq "Stopped") {
            Write-Output "$(Get-Date): WebAppPool '$webAppPoolName' is stopped"
            $poolNotStopped = $false
            $retvalue = $true
        }
    }
    return $retvalue
}

git pull
dotnet publish
Stop-AppPool -Name "clash.lam0x86.ru"
Start-Sleep 2
Copy-Item "ClashOfClans\bin\Debug\netcoreapp2.1\publish\*" -Destination "C:\WebSites\clash.lam0x86.ru\" -Recurse -Force
Start-WebAppPool -Name "clash.lam0x86.ru"