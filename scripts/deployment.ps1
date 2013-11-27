Param (
    $variables = @{},   
    $artifacts = @{},
    $scriptPath,
    $buildFolder,
    $srcFolder,
    $outFolder,
    $tempFolder,
    $projectName,
    $projectVersion,
    $projectBuildNumber
)

$nuspec_template=cat .\scripts\AwesomeSockets.dll.template.nuspec
$a=$a -replace "<version>1.0.0</version>", "<version>$projectVersion.$projectBuildNumber</version>"
echo $a > "$buildFolder\AwesomeSockets.dll.nuspec"
cd ..\
.\.nuget\NuGet.exe setapikey $variables.Get_Item("SecureNugetKey")
.\.nuget\NuGet.exe pack "$buildFolder\AwesomeSockets.dll.nuspec"
.\.nuget\NuGet.exe push "$buildFolder\AwesomeSockets.dll.nupkg"