<# 
create the required aspnetdb database 
#>

$dbfile = "${PSScriptRoot}\App_Data\aspnetdb.mdf"
Write-Host "Creating aspnetdb on $dbfile"
& osql -S "(localdb)\MSSQLLocalDB" -E -Q "Create DATABASE [aspnetdb] ON PRIMARY (NAME='aspnetdb', FILENAME='$dbfile')"