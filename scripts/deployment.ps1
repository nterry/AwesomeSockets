Param (
    $variables = @{},   
    $artifacts = @{},
    $scriptPath,
	$scriptFolder,
    $buildFolder,
    $srcFolder,
    $outFolder,
    $tempFolder,
    $projectName,
    $projectVersion,
    $projectBuildNumber
)

$artifactToUse=$null
foreach($artifact in $artifacts.values)
{
	If ($artifact.name -notmatch ".*\.Tests$")
	{
		$artifactToUse=$artifact.sourcePath
	}
}
if ($artifactToUse -eq $null)
{
	exit
}

echo $artifactToUse
$nugetFolder=".nuget"
$nugetStage="$tempFolder\nuget-staging"
md "$nugetStage\lib\Net20"
md "$nugetStage\lib\Net40"

$nuspecTemplate=cat "$scriptFolder\AwesomeSockets.dll.template.nuspec"
$nuspecTemplate=$nuspecTemplate -replace "<version>1.0.0</version>", "<version>$projectVersion</version>"
echo $nuspecTemplate > "$nugetStage\AwesomeSockets.dll.nuspec"

Copy-Item "$artifactToUse\AwesomeSockets.dll" "$nugetStage\lib\Net20"
Copy-Item "$artifactToUse\AwesomeSockets.dll" "$nugetStage\lib\Net40"

$apiKey=$variables.Get_Item("SecureNugetKey")
iex "$nugetFolder\NuGet.exe setapikey $apiKey"
iex "$nugetFolder\NuGet.exe pack $nugetStage\AwesomeSockets.dll.nuspec"
iex "$nugetFolder\NuGet.exe push $srcFolder\AwesomeSockets.dll.$projectVersion.nupkg"