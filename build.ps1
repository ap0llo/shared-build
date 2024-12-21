$ErrorActionPreference = "Stop"

if ($env:GITHUB_ACTIONS -eq "true") {
    Write-Host "::group::Install .NET SDK"
}
if ($env:TF_BUILD) {
    Write-Host "##[group]Install .NET SDK"
}

# Install SDK and runtime as specified in global.json
./build/dotnet-install.ps1 -JsonFile "$PSScriptRoot/global.json"

Invoke-Expression "dotnet --info"

if ($env:GITHUB_ACTIONS -eq "true") {
    Write-Host "::endgroup::"
}
if ($env:TF_BUILD) {
    Write-Host "##[endgroup]"
}

dotnet run --project build/Build.csproj -- $args
exit $LASTEXITCODE