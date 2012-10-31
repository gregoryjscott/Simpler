call build\debug
if %ERRORLEVEL% GTR 0 exit /B %ERRORLEVEL%

call test\debug
exit /B %ERRORLEVEL%
