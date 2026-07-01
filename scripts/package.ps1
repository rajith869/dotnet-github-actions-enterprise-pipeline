param(
    [string]$BuildOutputPath = "./artifacts/build",
    [string]$PackageOutputPath = "./artifacts/package",
    [string]$PackageName = "sample-api",
    [string]$Version = "1.0.0"
)

$ErrorActionPreference = "Stop"

$publishPath = Join-Path $BuildOutputPath "Sample.Api"
if (-not (Test-Path -LiteralPath $publishPath)) {
    throw "Expected published application output at '$publishPath'."
}

if (Test-Path -LiteralPath $PackageOutputPath) {
    Remove-Item -LiteralPath $PackageOutputPath -Recurse -Force
}

New-Item -ItemType Directory -Force -Path $PackageOutputPath | Out-Null

$stagingPath = Join-Path $PackageOutputPath "staging"
New-Item -ItemType Directory -Force -Path $stagingPath | Out-Null

Copy-Item -Path (Join-Path $publishPath "*") -Destination $stagingPath -Recurse

$buildManifestPath = Join-Path $BuildOutputPath "build-manifest.json"
if (Test-Path -LiteralPath $buildManifestPath) {
    Copy-Item -LiteralPath $buildManifestPath -Destination (Join-Path $stagingPath "build-manifest.json")
}

$zipFileName = "$PackageName-$Version.zip"
$zipPath = Join-Path $PackageOutputPath $zipFileName
Compress-Archive -Path (Join-Path $stagingPath "*") -DestinationPath $zipPath -Force

$packageHash = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash
$packageManifest = [ordered]@{
    packageName = $PackageName
    version = $Version
    packageFile = $zipFileName
    sha256 = $packageHash
    createdAtUtc = (Get-Date).ToUniversalTime().ToString("O")
    promotionModel = "Download this package once and promote the unchanged artifact."
    deploymentTarget = "No real deployment target; simulation only."
}

$packageManifest | ConvertTo-Json -Depth 4 |
    Set-Content -Path (Join-Path $PackageOutputPath "package-manifest.json") -Encoding utf8

Remove-Item -LiteralPath $stagingPath -Recurse -Force

Write-Host "Package prepared at $zipPath"
