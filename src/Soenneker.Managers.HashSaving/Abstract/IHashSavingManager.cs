using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.HashSaving.Abstract;

/// <summary>
/// Handles hashing and saving
/// </summary>
public interface IHashSavingManager
{
    ValueTask SaveHashToGitRepoWithoutClearingResources(string gitDirectory, string newHash, string hashFileName, string name, string email, string token,
        CancellationToken cancellationToken = default);

    ValueTask SaveHashToGitRepoAsFile(string gitDirectory, string newHash, string fileName, string hashFileName, string name, string email, string username,
        string token, CancellationToken cancellationToken = default);

    ValueTask SaveHashToGitRepoAsDirectory(string gitDirectory, string newHash, string targetDir, string hashFileName, string name, string email,
        string username, string token, CancellationToken cancellationToken = default);
}