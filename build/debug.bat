echo build solution
build\tools\MSBuild.exe app\Simpler.sln /p:Configuration=Debug

exit /B %ERRORLEVEL%
