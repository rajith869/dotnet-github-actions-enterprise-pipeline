# Workflow Design

The workflows are split into one orchestration workflow, three reusable workflows, and one manual release simulation. The file names are intentionally plain so the reusable workflow boundaries are easy to inspect in a portfolio review.

## Main CI Workflow

`ci.yml` appears in GitHub Actions as `CI - Build, Test, Package`. It runs on pull requests, pushes to `main`, and manual dispatch. It coordinates application build, test execution, artifact packaging, and a short run summary.

## Reusable Build Workflow

`reusable-dotnet-build.yml` appears as `Reusable .NET Build`. It accepts the .NET SDK version, solution path, configuration, API project path, artifact name, and version prefix. It runs `scripts/build.ps1`, publishes the sample API, writes a build manifest, and uploads the build artifact.

The reusable workflow returns:

- `build_artifact_name`
- `package_version`

## Reusable Test Workflow

`reusable-dotnet-test.yml` appears as `Reusable .NET Test`. It restores the solution and runs `dotnet test`. Test results are uploaded as `sample-api-test-results` so failed runs still retain useful diagnostics.

## Reusable Package Workflow

`reusable-package.yml` appears as `Reusable Package Artifact`. It downloads the build artifact produced earlier, runs `scripts/package.ps1`, creates a zip package and package manifest, then uploads the release package artifact.

This models artifact-based release flow because the package job consumes build output instead of rebuilding application code.

The main CI artifact names are:

- `sample-api-build-output`
- `sample-api-release-package`
- `sample-api-test-results`

## Release Promotion Simulation

`release-promotion-simulation.yml` appears as `Release Promotion Simulation`. It is a manual workflow. It builds, tests, packages, and then runs a promotion job associated with the selected GitHub Environment name.

The promotion job downloads the package artifact and runs `scripts/promote-artifact.ps1`. The script writes a promotion record but does not deploy anywhere.

The release simulation artifact names are:

- `sample-api-release-build-output`
- `sample-api-release-candidate-package`
- `sample-api-promotion-record-<environment>`

## Permissions And Secrets

The workflows use `contents: read` and do not require secrets. This keeps the sample safe for a public repository and reinforces that no real deployment target is included.
