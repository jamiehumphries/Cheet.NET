@echo off
set /p version="Version: "
msbuild Cheet.sln /P:Configuration=Release
rmdir /S /Q nuget-pack\lib
xcopy Cheet.Wpf\bin\Release\Cheet.Wpf.dll nuget-pack\lib\4.5\ /Y
xcopy Cheet.Wpf\bin\Release\Cheet.Core.dll nuget-pack\lib\4.5\ /Y
.nuget\nuget pack nuget-pack\Cheet.Wpf.nuspec -Version %version%
pause