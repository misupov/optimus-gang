git pull
dotnet publish
Stop-WebSite -Name "clash.lam0x86.ru"
Copy-Item "ClashOfClans\bin\Debug\netcoreapp2.1\publish\*" -Destination "C:\WebSites\clash.lam0x86.ru\" -Recurse -Force
Start-WebSite -Name "clash.lam0x86.ru"