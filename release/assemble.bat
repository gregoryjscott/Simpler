echo delete previous release
del /s /q release\template\lib\*.*

echo build
call build\release

echo copy files to release
xcopy app\Simpler\bin\Release\Simpler.dll release\template\lib /y /i
xcopy app\Simpler\bin\Release\Simpler.xml release\template\lib /y /i

exit /B %ERRORLEVEL%
