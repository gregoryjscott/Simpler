echo build solution
build\tools\MSBuild.exe app\Simpler.sln /p:Configuration=Release;DeployOnBuild=true;DeployTarget=Package

exit /B %ERRORLEVEL%
