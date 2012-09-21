release\tools\NuGet.exe pack release\template\Simpler.nuspec -OutputDirectory release\output -NoPackageAnalysis

exit /B %ERRORLEVEL%
