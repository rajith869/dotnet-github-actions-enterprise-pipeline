# Workflow Design

The workflows are split into one orchestration workflow, three reusable workflows, and one manual release simulation.

## Main CI Workflow

`ci.yml` runs on pull requests, pushes to `main`, and manual dispatch. It coordinates build, test, package, and summary jobs.

## Reusable Build Workflow

`reusable-dotnet-build.yml` accepts the .NET SDK version, solution path, configuration, API project path, artifact name, and version prefix. It runs `scripts/build.ps1`, publishes the sample API, writes a build manifest, and uploads the build artifact.

The reusable workflow returns:

- `build_artifact_name`
- `package_version`

## Reusable Test Workflow

`reusable-dotnet-test.yml` restores the solution and runs `dotnet test`. Test results are uploaded as an artifact so failed runs still retain useful diagnostics.

## Reusable Package Workflow

`reusable-package.yml` downloads the build artifact produced earlier, runs `scripts/package.ps1`, creates a zip package and package manifest, then uploads the release package artifact.

This models artifact-based release flow because the package job consumes build output instead of rebuilding application code.

## Release Promotion Simulation

`release-promotion-simulation.yml` is a manual workflow. It builds, tests, packages, and then runs a promotion job associated with the selected GitHub Environment name.

The promotion job downloads the package artifact and runs `scripts/promote-artifact.ps1`. The script writes a promotion record but does not deploy anywhere.

## Permissions And Secrets

The workflows use `contents: read` and do not require secrets. This keeps the sample safe for a public repository and reinforces that no real deployment target is included.
