@echo off
cls

.paket\paket.bootstrapper.exe
if errorlevel 1 (
    exit /b %errorlevel%
)

.paket\paket.exe install
.paket\paket.exe update

packages\Cake\Cake.exe build.cake %*
if errorlevel 1 (
    exit /b %errorlevel%
)