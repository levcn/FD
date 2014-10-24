C:\Windows\microsoft.net\framework\v4.0.30319\MSBuild.exe FD.Core.ObjConfigSL.sln /target:Rebuild /property:Configuration=Debug;Msbuild35=true > SL.log.txt
start notepad SL.log.txt
C:\Windows\microsoft.net\framework\v4.0.30319\MSBuild.exe FD.Core.ObjConfig.csproj /target:Rebuild /property:Configuration=Debug;Msbuild35=true > WP.log.txt
start notepad WP.log.txt
