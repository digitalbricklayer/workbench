appveyor DownloadFile https://github.com/nunit/nunit/releases/download/3.0.0/nunit-3.0.0.zip
7z x -oC:\Tools\NUnit3 nunit-3.0.0.zip > NUL
set PATH=C:\Tools\NUnit3\bin;%PATH%
