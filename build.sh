#!/bin/bash

# Define variables (that might or might not be set)
# Without this, the script will fail with an unbound variable" error (because of the 'set -e')
TF_BUILD=$TF_BUILD

set -e

if [[ -n "${TF_BUILD}" ]]; then
    echo "##[group]Install .NET SDK"
fi

# Install SDK and runtime as specified in global.json
source ./build/dotnet-install.sh --jsonfile "global.json"

dotnet --info


if [[ -n "${TF_BUILD}" ]]; then
    echo "##[endgroup]"
fi

dotnet run --project build/Build.csproj -- $@
