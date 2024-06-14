@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\System32\
set PATH=%SystemRoot%\System32\

echo UnInstalling Parafait Kiosk Service...
echo ---------------------------------------------------
sc delete "Parafait Kiosk Service" binpath="\"C:\Program Files (x86)\Semnox Solutions\Parafait\Kiosk\ParafaitKioskService.exe\""
echo ---------------------------------------------------
echo Done.
exit