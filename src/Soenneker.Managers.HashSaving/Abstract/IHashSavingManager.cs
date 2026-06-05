using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.HashSaving.Abstract;

/// <summary>
/// Handles hashing and saving
/// </summary>
public interface IHashSavingManager
{
    /// <summary>
    /// Executes the save hash to git repo without clearing resources operation.
    /// </summary>
    /// <param name="gitDirectory">The git directory.</param>
    /// <param name="newHash">The new hash.</param>
    /// <param name="hashFileName">The hash file name.</param>
    /// <param name="name">The name.</param>
    /// <param name="email">The email address.</param>
    /// <param name="token">The token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask SaveHashToGitRepoWithoutClearingResources(string gitDirectory, string newHash, string hashFileName, string name, string email, string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the save hash to git repo as file operation.
    /// </summary>
    /// <param name="gitDirectory">The git directory.</param>
    /// <param name="libraryName">The library name.</param>
    /// <param name="newHash">The new hash.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="hashFileName">The hash file name.</param>
    /// <param name="name">The name.</param>
    /// <param name="email">The email address.</param>
    /// <param name="username">The username.</param>
    /// <param name="token">The token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask SaveHashToGitRepoAsFile(string gitDirectory, string libraryName, string newHash, string fileName, string hashFileName, string name, string email, string username,
        string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the save hash to git repo as directory operation.
    /// </summary>
    /// <param name="gitDirectory">The git directory.</param>
    /// <param name="newHash">The new hash.</param>
    /// <param name="targetDir">The target dir.</param>
    /// <param name="hashFileName">The hash file name.</param>
    /// <param name="name">The name.</param>
    /// <param name="email">The email address.</param>
    /// <param name="username">The username.</param>
    /// <param name="token">The token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask SaveHashToGitRepoAsDirectory(string gitDirectory, string newHash, string targetDir, string hashFileName, string name, string email,
        string username, string token, CancellationToken cancellationToken = default);
}