echo run tests
test\tools\nunit-console.exe app\Simpler.Tests\bin\Debug\Simpler.Tests.dll /xml=app\Simpler.Tests\bin\Debug\nunit-result.xml /framework=net-4.0
exit /B %ERRORLEVEL%