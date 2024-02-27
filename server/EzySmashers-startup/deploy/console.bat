
@echo off
set "ezyclasspath=lib\*;settings\;common\*;apps\common\*;apps\resources\*"
for /D %%d in (plugins\*) do (
    call set "ezyclasspath=%%ezyclasspath%%;%%d\*
)
echo classpath = %ezyclasspath%

java %1 -cp %ezyclasspath% org.youngmonkeys.ezysmashers.ApplicationStartup
