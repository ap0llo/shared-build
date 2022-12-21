$ErrorActionPreference = "Stop"

if ($env:TF_BUILD) {
    Write-Host "##[group]Install .NET SDK"
}

# Install SDK and runtime as specified in global.json
./build/dotnet-install.ps1 -JsonFile "$PSScriptRoot/global.json"

# Install .NET 6 runtime (required for running tests)
./build/dotnet-install.ps1 -Channel 6.0 -Runtime dotnet

Invoke-Expression "dotnet --info"

if ($env:TF_BUILD) {
    Write-Host "##[endgroup]"
}

dotnet run --project build/Build.csproj -- $args
exit $LASTEXITCODE