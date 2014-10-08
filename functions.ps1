function Delete-Directory($directoryname)
{
    Write-Host "Removing $directoryname"
	Remove-Item -Force -Recurse $directoryname -ErrorAction SilentlyContinue
}
 
function Create-Directory($directoryname)
{
	New-Item $directoryname -ItemType Directory | Out-Null
}