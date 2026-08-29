using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.HashSaving.Abstract;

/// <summary>
/// Handles hashing and saving
/// </summary>
public interface IHashSavingManager
{
    /// <summary>
    /// Saves hash To Git Repo Without Clearing Resources.
    /// </summary>
    /// <param name="gitDirectory">Git repository directory to inspect or update.</param>
    /// <param name="newHash">New Hash for the save hash to git repo without clearing resources operation.</param>
    /// <param name="hashFileName">Name of the hash file to target.</param>
    /// <param name="name">Name of the Hash Saving Manager value to target.</param>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="token">Arbitrary utility token to append.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the hash to git repo without clearing resources has been saved.</returns>
    ValueTask SaveHashToGitRepoWithoutClearingResources(string gitDirectory, string newHash, string hashFileName, string name, string email, string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves hash To Git Repo As File.
    /// </summary>
    /// <param name="gitDirectory">Git repository directory to inspect or update.</param>
    /// <param name="libraryName">Name of the library to load.</param>
    /// <param name="newHash">New Hash for the save hash to git repo as file operation.</param>
    /// <param name="fileName">Name of the target file.</param>
    /// <param name="hashFileName">Name of the hash file to target.</param>
    /// <param name="name">Name of the Hash Saving Manager value to target.</param>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="username">Receives the decoded username when parsing succeeds.</param>
    /// <param name="token">Arbitrary utility token to append.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the hash to git repo as file has been saved.</returns>
    ValueTask SaveHashToGitRepoAsFile(string gitDirectory, string libraryName, string newHash, string fileName, string hashFileName, string name, string email, string username,
        string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves hash To Git Repo As Directory.
    /// </summary>
    /// <param name="gitDirectory">Git repository directory to inspect or update.</param>
    /// <param name="newHash">New Hash for the save hash to git repo as directory operation.</param>
    /// <param name="targetDir">Target Dir for the save hash to git repo as directory operation.</param>
    /// <param name="hashFileName">Name of the hash file to target.</param>
    /// <param name="name">Name of the Hash Saving Manager value to target.</param>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="username">Receives the decoded username when parsing succeeds.</param>
    /// <param name="token">Arbitrary utility token to append.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the hash to git repo as directory has been saved.</returns>
    ValueTask SaveHashToGitRepoAsDirectory(string gitDirectory, string newHash, string targetDir, string hashFileName, string name, string email,
        string username, string token, CancellationToken cancellationToken = default);
}
