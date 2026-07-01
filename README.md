# dotnet-github-actions-enterprise-pipeline

Public-safe portfolio repository for a Senior .NET Backend Engineer. It demonstrates how an enterprise-style .NET application suite can use reusable GitHub Actions workflows for build, test, package, artifact handling, and build-once/promote-later release thinking.

This is not production code from an employer. It uses only generic sample names and intentionally avoids confidential architecture, credentials, endpoints, repositories, servers, databases, deployment targets, and business workflows.

## Project Overview

The repository contains a small multi-project .NET solution and a set of GitHub Actions workflows that separate continuous integration from release promotion. The application code is intentionally simple so the CI/CD design is the main focus.

The sample API exposes public-safe endpoints for health, release readiness, and promotion decision checks. The Core project contains neutral release policy logic, while Infrastructure provides an in-memory implementation that stands in for external dependencies without naming or modeling any real systems.

## Why Reusable Workflows Matter

Reusable workflows keep enterprise CI/CD pipelines consistent across many repositories. Instead of copying restore, build, test, package, and artifact-handling steps into every repository, teams can centralize the standard path and pass only the values that vary.

That design improves maintainability, reduces pipeline drift, makes modernization easier, and lets teams apply release controls consistently without exposing sensitive implementation details.

## What This Demonstrates

- Reusable GitHub Actions workflow design.
- Multi-project .NET solution structure.
- Build once, promote later release thinking.
- Artifact-based deployment flow.
- CI/CD modernization from older pipeline tools to GitHub Actions.
- Production-minded release separation without exposing real deployment details.

## Repository Structure

```text
.
├── .github/workflows
│   ├── ci.yml
│   ├── reusable-dotnet-build.yml
│   ├── reusable-dotnet-test.yml
│   ├── reusable-package.yml
│   └── release-promotion-simulation.yml
├── docs
│   ├── architecture.md
│   ├── release-flow.md
│   ├── what-this-demonstrates.md
│   └── workflow-design.md
├── scripts
│   ├── build.ps1
│   ├── package.ps1
│   └── promote-artifact.ps1
├── src
│   ├── Sample.Api
│   ├── Sample.Core
│   └── Sample.Infrastructure
└── tests
    └── Sample.Tests
```

## How The CI Workflow Works

`ci.yml` is the main pull request and branch workflow. It delegates work to reusable workflows:

1. `reusable-dotnet-build.yml` restores, builds, publishes the sample API, creates build metadata, and uploads a build artifact.
2. `reusable-dotnet-test.yml` restores and runs the test suite, then uploads test results.
3. `reusable-package.yml` downloads the build artifact, packages the unchanged output, creates package metadata, and uploads a release-style package artifact.

The result is a clean separation between compile/test validation and package creation.

## How The Release Promotion Simulation Works

`release-promotion-simulation.yml` is manually triggered with a target environment choice: `development`, `test`, or `staging`.

The workflow builds and packages the release candidate once, then downloads the package artifact in a promotion job. The promotion step writes a public-safe promotion record that includes the artifact file name, hash, target environment, and timestamp. It does not deploy to any real target.

This demonstrates the release-management idea that the artifact promoted between environments should be the same artifact that was already built and tested.

## Public-Safety Note

All names, workflows, scripts, and sample data are generic. This repository does not describe real production infrastructure, real deployment environments, employer systems, vendor systems, customer details, credentials, secrets, URLs, or internal business processes.

## What This Intentionally Does Not Cover

- This project does not include real production deployment targets.
- This project does not include confidential business logic.
- This project does not include Docker, Kubernetes, Kafka, Redis, Microservices, or Azure DevOps.
- This project is a simplified public-safe portfolio sample.

## Enterprise .NET CI/CD Modernization Context

This repository reflects the kind of modernization work common in established .NET environments: moving from older, duplicated pipeline definitions toward reusable GitHub Actions workflows; making package artifacts explicit; separating build and release concerns; and improving release traceability without exposing operational details.

The sample is intentionally small, but the design mirrors production-minded habits: deterministic build commands, scripted packaging, artifact metadata, environment gates, and clear documentation.

## Run Locally

```powershell
dotnet restore .\dotnet-github-actions-enterprise-pipeline.sln
dotnet build .\dotnet-github-actions-enterprise-pipeline.sln --configuration Release
dotnet test .\dotnet-github-actions-enterprise-pipeline.sln --configuration Release
.\scripts\build.ps1 -Version "1.0.1"
.\scripts\package.ps1 -Version "1.0.1"
.\scripts\promote-artifact.ps1 -TargetEnvironment "development"
```
