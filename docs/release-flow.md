# Release Flow

The release flow demonstrates build-once/promote-later thinking without deploying to a real target.

## CI Flow

1. Restore dependencies.
2. Build the solution.
3. Publish the sample API output.
4. Upload the build output as an artifact.
5. Run tests.
6. Download the build artifact in the package workflow.
7. Package the unchanged published output.
8. Upload the package artifact.

## Promotion Simulation

The release promotion workflow is manually triggered. It accepts a target environment choice of `development`, `test`, or `staging`.

The workflow builds and packages once, then downloads the package artifact in the promotion job. The promotion script writes a `promotion-record.json` file containing:

- Package file name.
- Package SHA-256 hash.
- Target environment.
- Promotion timestamp.
- A note that no deployment was performed.

## Why The Artifact Matters

Promoting the same package through environment gates helps avoid rebuilding different binaries for each stage. In a real enterprise setting, this supports traceability and repeatability. In this public repository, the concept is represented only through generic artifacts and metadata.

## No Real Deployment Targets

The workflow does not publish to servers, cloud resources, databases, or internal systems. The final promotion step is a documentation-friendly simulation that demonstrates release separation safely.
