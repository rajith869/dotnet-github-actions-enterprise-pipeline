param(
    [string]$SolutionPath = "./dotnet-github-actions-enterprise-pipeline.sln",
    [string]$Configuration = "Release",
    [string]$Version = "1.0.0",
    [string]$PublishProjectPath = "./src/Sample.Api/Sample.Api.csproj",
    [string]$OutputPath = "./artifacts/build/Sample.Api"
)

$ErrorActionPreference = "Stop"

Write-Host "Restoring $SolutionPath"
dotnet restore $SolutionPath

Write-Host "Building $SolutionPath"
dotnet build $SolutionPath `
    --configuration $Configuration `
    --no-restore `
    -p:Version=$Version `
    -p:ContinuousIntegrationBuild=true

if (Test-Path -LiteralPath $OutputPath) {
    Remove-Item -LiteralPath $OutputPath -Recurse -Force
}

Write-Host "Publishing $PublishProjectPath to $OutputPath"
dotnet publish $PublishProjectPath `
    --configuration $Configuration `
    --no-build `
    --output $OutputPath `
    -p:Version=$Version `
    -p:ContinuousIntegrationBuild=true

$artifactRoot = Split-Path -Path $OutputPath -Parent
New-Item -ItemType Directory -Force -Path $artifactRoot | Out-Null

$manifest = [ordered]@{
    application = "Sample.Api"
    version = $Version
    configuration = $Configuration
    createdAtUtc = (Get-Date).ToUniversalTime().ToString("O")
    artifactModel = "build-once-promote-later"
    publicSafety = "Sample manifest only; no real deployment target or internal system detail."
}

$manifestPath = Join-Path $artifactRoot "build-manifest.json"
$manifest | ConvertTo-Json -Depth 4 | Set-Content -Path $manifestPath -Encoding utf8

Write-Host "Build artifact prepared at $artifactRoot"
