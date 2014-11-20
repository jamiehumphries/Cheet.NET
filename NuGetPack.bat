@echo off
set /p version="Version: "
msbuild Cheet.sln /P:Configuration=Release

rmdir /S /Q nuget-pack\Cheet.Core\lib
xcopy Cheet.Core\bin\Release\Cheet.Core.dll nuget-pack\Cheet.Core\lib\4.5\ /Y
.nuget\nuget pack nuget-pack\Cheet.Core\Cheet.Core.nuspec -Version %version%

rmdir /S /Q nuget-pack\Cheet.Wpf\lib
xcopy Cheet.Wpf\bin\Release\Cheet.Wpf.dll nuget-pack\Cheet.Wpf\lib\4.5\ /Y
.nuget\nuget pack nuget-pack\Cheet.Wpf\Cheet.Wpf.nuspec -Version %version%

pause