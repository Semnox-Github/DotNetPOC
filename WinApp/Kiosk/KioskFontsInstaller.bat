
@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\
set PATH=%PATH%;%DOTNETFX2%

echo Installing Arial Unicode MS Fonts...
echo ---------------------------------------------------
COPY ".\Fonts\GothamRounded-Bold.ttf" %systemroot%\fonts


COPY ".\Fonts\GothamRounded-BoldItalic.ttf" %systemroot%\fonts


COPY ".\Fonts\GothamRounded-Book.ttf" %systemroot%\fonts


COPY ".\Fonts\GothamRounded-BookItalic.ttf" %systemroot%\fonts


COPY ".\Fonts\GothamRounded-Light.ttf" %systemroot%\fonts


COPY ".\Fonts\GothamRounded-LightItalic.ttf" %systemroot%\fonts


COPY ".\Fonts\GothamRounded-Medium.ttf" %systemroot%\fonts


COPY ".\Fonts\GothamRounded-MediumItalic.ttf" %systemroot%\fonts

COPY ".\Fonts\Cubano-Regular.otf" %systemroot%\fonts

COPY ".\Fonts\Cubano-Regular.ttf" %systemroot%\fonts

COPY ".\Fonts\Bango-Pro.otf" %systemroot%\fonts


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Bold (OpenType)" /t REG_SZ /d "GothamRounded-Bold.ttf" /f


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Bold Italic (OpenType)" /t REG_SZ /d "GothamRounded-BoldItalic.ttf" /f


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Book (OpenType)" /t REG_SZ /d "GothamRounded-Book.ttf" /f


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Book Italic (OpenType)" /t REG_SZ /d "GothamRounded-BookItalic.ttf" /f


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Light (OpenType)" /t REG_SZ /d "GothamRounded-Light.ttf" /f


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Light Italic (OpenType)" /t REG_SZ /d "GothamRounded-LightItalic.ttf" /f


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Medium (TrueType)" /t REG_SZ /d "GothamRounded-Medium.ttf" /f


REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Gotham Rounded Medium Italic (TrueType)" /t REG_SZ /d "GothamRounded-MediumItalic.ttf" /f

REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Cubano Regular (TrueType)" /t REG_SZ /d "Cubano-Regular.otf" /f

REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Cubano Regular (TrueType)" /t REG_SZ /d "Cubano-Regular.ttf" /f

REG ADD "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts" /v "Bango Pro (TrueType)" /t REG_SZ /d "Bango-Pro.otf" /f


echo ---------------------------------------------------
echo Done.
