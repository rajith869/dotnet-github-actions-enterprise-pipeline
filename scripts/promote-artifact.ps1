param(
    [string]$PackagePath = "./artifacts/package",
    [string]$TargetEnvironment = "development",
    [string]$OutputPath = "./artifacts/promotion"
)

$ErrorActionPreference = "Stop"

$allowedEnvironments = @("development", "test", "staging")
if ($allowedEnvironments -notcontains $TargetEnvironment.ToLowerInvariant()) {
    throw "Target environment '$TargetEnvironment' is not part of this public-safe simulation."
}

if (Test-Path -LiteralPath $PackagePath -PathType Container) {
    $packageFile = Get-ChildItem -LiteralPath $PackagePath -Filter "*.zip" | Select-Object -First 1
    if ($null -eq $packageFile) {
        throw "No package zip file was found in '$PackagePath'."
    }
} elseif (Test-Path -LiteralPath $PackagePath -PathType Leaf) {
    $packageFile = Get-Item -LiteralPath $PackagePath
} else {
    throw "Package path '$PackagePath' does not exist."
}

New-Item -ItemType Directory -Force -Path $OutputPath | Out-Null

$record = [ordered]@{
    artifactFile = $packageFile.Name
    artifactSha256 = (Get-FileHash -LiteralPath $packageFile.FullName -Algorithm SHA256).Hash
    targetEnvironment = $TargetEnvironment.ToLowerInvariant()
    promotedAtUtc = (Get-Date).ToUniversalTime().ToString("O")
    promotionType = "simulation"
    deploymentStatus = "No deployment performed."
    publicSafety = "This record intentionally omits real endpoints, credentials, and infrastructure names."
}

$recordPath = Join-Path $OutputPath "promotion-record.json"
$record | ConvertTo-Json -Depth 4 | Set-Content -Path $recordPath -Encoding utf8

Write-Host "Promotion simulation record written to $recordPath"
