:: CreateReadMe.cmd
:: Create the ReadMe.md file from Docs/ReadMe.docx.
:: This command is meant to be run from within the Scripts folder.
SET FAR=C:\OneDrive\Develop\Active\FindAndReplace\FindAndReplace\bin\Debug\net6.0\FindAndReplace.exe
SET SOURCE=..\Docs\ReadMe.docx
SET TARGETHTML=..\Docs\README.html
SET TARGETMARKDOWN=..\README.md
SET PATTERN=ReadmePostProcessing.json

:: Embedded resources are only created if output is HTML.
PANDOC -t html --embed-resources=true "%SOURCE%" -o "%TARGETHTML%"
PANDOC -t markdown_strict "%TARGETHTML%" -o "%TARGETMARKDOWN%"
"%FAR%" /wait "/workingpath:..\Docs" "/files:%TARGETMARKDOWN%" "/patternfile:%PATTERN%"
