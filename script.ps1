$exelus = Get-Process Exelus.Win11DesktopSwitchAnimatior -ErrorAction SilentlyContinue
if ($exelus) {
  if (!$exelus.HasExited) {
    $exelus | Stop-Process -Force
  }
}
else{
    Start-Process -FilePath "Exelus.Win11DesktopSwitchAnimatior\Exelus.Win11DesktopSwitchAnimatior\bin\Debug\net6.0-windows\publish\Exelus.Win11DesktopSwitchAnimatior.exe"
}
Remove-Variable exelus