@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\System32\
set PATH=%SystemRoot%\System32\

echo Installing Parafait Kiosk Service...
echo ---------------------------------------------------
sc create "Parafait Kiosk Service" binpath= "C:\Program Files (x86)\Semnox Solutions\Parafait\Kiosk\ParafaitKioskService.exe" start= auto
sc description "Parafait Kiosk Service" "Run parafait kiosk as service."
sc start "Parafait Kiosk Service" "C:\Program Files (x86)\Semnox Solutions\Parafait\Kiosk\Parafait Kiosk.exe"
echo ---------------------------------------------------
echo Done.
exit