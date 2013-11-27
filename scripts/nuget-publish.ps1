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
$a=$a -replace "<version>1.0.0</version>", "<version>$projectVersion</version>"