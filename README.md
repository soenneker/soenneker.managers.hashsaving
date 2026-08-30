[![](https://img.shields.io/nuget/v/soenneker.managers.hashsaving.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashsaving/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashsaving/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashsaving/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.managers.hashsaving.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashsaving/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashsaving/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashsaving/actions/workflows/codeql.yml)

# Soenneker.Managers.HashSaving

Writes a precomputed hash into a repository checkout, optionally removes packaged resources, then commits and pushes the change.

## Install

```bash
dotnet add package Soenneker.Managers.HashSaving
```

## Usage

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Managers.HashSaving.Abstract;
using Soenneker.Managers.HashSaving.Registrars;

services.AddHashSavingManagerAsSingleton();

IHashSavingManager hashes =
    serviceProvider.GetRequiredService<IHashSavingManager>();

await hashes.SaveHashToGitRepoWithoutClearingResources(
    gitDirectory: repositoryPath,
    newHash: verifiedHash,
    hashFileName: "hash.txt",
    name: "Automation",
    email: "automation@example.com",
    token: githubToken,
    cancellationToken);
```

Each method writes the hash and immediately commits and pushes all working-tree changes using the message `Updates hash for new version`. Use it only with a disposable, dedicated checkout whose complete contents are intended for that commit.

## What you get

- `IHashSavingManager` — Handles hashing and saving.
- `HashSavingManagerRegistrar` — Handles hashing and saving.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IHashSavingManager.SaveHashToGitRepoWithoutClearingResources(gitDirectory, newHash, hashFileName, name, email, token, cancellationToken)` | Saves hash To Git Repo Without Clearing Resources. | A task that completes when the hash to git repo without clearing resources has been saved. |
| `IHashSavingManager.SaveHashToGitRepoAsFile(gitDirectory, libraryName, newHash, fileName, hashFileName, name, email, username, token, cancellationToken)` | Saves hash To Git Repo As File. | A task that completes when the hash to git repo as file has been saved. |
| `IHashSavingManager.SaveHashToGitRepoAsDirectory(gitDirectory, newHash, targetDir, hashFileName, name, email, username, token, cancellationToken)` | Saves hash To Git Repo As Directory. | A task that completes when the hash to git repo as directory has been saved. |
| `HashSavingManagerRegistrar.AddHashSavingManagerAsSingleton(services)` | Adds `IHashSavingManager` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `HashSavingManagerRegistrar.AddHashSavingManagerAsScoped(services)` | Adds `IHashSavingManager` as a scoped service. | The same service collection, so additional registrations can be chained. |

## Practical notes

- `SaveHashToGitRepoWithoutClearingResources()` leaves packaged resources in place.
- `SaveHashToGitRepoAsFile()` deletes `src/<libraryName>/Resources/<fileName>` before committing.
- `SaveHashToGitRepoAsDirectory()` deletes the supplied resource directory before committing.
- Hash and resource paths are required to remain inside `gitDirectory`; traversal and outside paths are rejected before deletion.
- `username` is retained for API compatibility but is not used. `name` and `email` set commit attribution; `token` authenticates the push.
- Cancellation can stop the operation between local mutation, commit, and push. Run recovery or discard the dedicated checkout rather than assuming rollback.
