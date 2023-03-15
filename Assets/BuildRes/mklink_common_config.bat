@echo off
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if '%errorlevel%' NEQ '0' (
goto UACPrompt
) else ( goto gotAdmin )
:UACPrompt
echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs"
"%temp%\getadmin.vbs"
exit /B
:gotAdmin
if exist "%temp%\getadmin.vbs" ( del "%temp%\getadmin.vbs" )


cd /d %~dp0
mklink /d Table ..\..\..\001_GameFramework_Table\CommonData\Table
mklink /d Plot ..\..\..\001_GameFramework_Table\CommonData\Plot
mklink /d BattleTrigger ..\..\..\001_GameFramework_Table\CommonData\BattleTrigger
mklink /d BattleMap ..\..\..\001_GameFramework_Table\CommonData\BattleMap
pause