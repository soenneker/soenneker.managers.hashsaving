[![](https://img.shields.io/nuget/v/soenneker.managers.hashsaving.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashsaving/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashsaving/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashsaving/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.managers.hashsaving.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashsaving/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashsaving/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashsaving/actions/workflows/codeql.yml)

# Soenneker.Managers.HashSaving

Handles hashing and saving.

## Install

```bash
dotnet add package Soenneker.Managers.HashSaving
```

## Quick start

```csharp
using Soenneker.Managers.HashSaving.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddHashSavingManagerAsSingleton();
```

Adds `IHashSavingManager` as a singleton service.

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

- Cancellation stops pending work; it does not undo work that has already completed.
