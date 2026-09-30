using Soenneker.Managers.HashSaving.Abstract;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.Task;
using Soenneker.Git.Util.Abstract;
using Soenneker.Utils.File.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.Directory.Abstract;

namespace Soenneker.Managers.HashSaving;

public sealed class HashSavingManager : IHashSavingManager
{
    private readonly ILogger<HashSavingManager> _logger;
    private readonly IFileUtil _fileUtil;
    private readonly IGitUtil _gitUtil;
    private readonly IDirectoryUtil _directoryUtil;

    public HashSavingManager(ILogger<HashSavingManager> logger, IFileUtil fileUtil, IGitUtil gitUtil, IDirectoryUtil directoryUtil)
    {
        _logger = logger;
        _fileUtil = fileUtil;
        _gitUtil = gitUtil;
        _directoryUtil = directoryUtil;
    }

    public async ValueTask SaveHashToGitRepoWithoutClearingResources(string gitDirectory, string newHash, string hashFileName, string name, string email,
        string token, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving hash to Git repo...");

        string targetHashFile = GetPathWithin(gitDirectory, hashFileName, "Hash file");
        await _fileUtil.DeleteIfExists(targetHashFile, cancellationToken: cancellationToken)
                       .NoSync();
        await _fileUtil.Write(targetHashFile, newHash, true, cancellationToken)
                       .NoSync();

        await _gitUtil.AddIfNotExists(gitDirectory, targetHashFile, cancellationToken)
                      .NoSync();

        await _gitUtil.CommitAndPush(gitDirectory, $"Update resources (SHA256 {newHash[..System.Math.Min(12, newHash.Length)]})\n\nContent SHA256: {newHash}", token, name, email, cancellationToken)
                      .NoSync();
    }

    public async ValueTask SaveHashToGitRepoAsFile(string gitDirectory, string libraryName, string newHash, string fileName, string hashFileName, string name,
        string email, string username, string token, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving hash to Git repo...");

        // Write new hash
        string targetHashFile = GetPathWithin(gitDirectory, hashFileName, "Hash file");
        await _fileUtil.DeleteIfExists(targetHashFile, cancellationToken: cancellationToken)
                       .NoSync();
        await _fileUtil.Write(targetHashFile, newHash, true, cancellationToken)
                       .NoSync();

        // Clean up the resource file from the repo
        string resourceFile = GetPathWithin(gitDirectory, Path.Combine("src", libraryName, "Resources", fileName), "Resource file");
        string commitMessage = await GetCommitMessage(libraryName, resourceFile, newHash, cancellationToken);
        await _fileUtil.DeleteIfExists(resourceFile, cancellationToken: cancellationToken)
                       .NoSync();

        // Stage the new hash file
        await _gitUtil.AddIfNotExists(gitDirectory, targetHashFile, cancellationToken)
                      .NoSync();

        await _gitUtil.CommitAndPush(gitDirectory, commitMessage, token, name, email, cancellationToken)
                      .NoSync();
    }

    public async ValueTask SaveHashToGitRepoAsDirectory(string gitDirectory, string newHash, string targetDir, string hashFileName, string name, string email,
        string username, string token, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving hash to Git repo...");

        // Write new hash
        string targetHashFile = GetPathWithin(gitDirectory, hashFileName, "Hash file");
        await _fileUtil.DeleteIfExists(targetHashFile, cancellationToken: cancellationToken)
                       .NoSync();
        await _fileUtil.Write(targetHashFile, newHash, true, cancellationToken)
                       .NoSync();

        string resourceDirectory = GetPathWithin(gitDirectory, targetDir, "Resource directory");
        string libraryName = Path.GetRelativePath(Path.Combine(gitDirectory, "src"), resourceDirectory).Split(Path.DirectorySeparatorChar)[0];
        string commitMessage = await GetCommitMessage(libraryName, resourceDirectory, newHash, cancellationToken);
        await _directoryUtil.Delete(resourceDirectory, cancellationToken);

        // Stage the new hash file
        await _gitUtil.AddIfNotExists(gitDirectory, targetHashFile, cancellationToken)
                      .NoSync();

        await _gitUtil.CommitAndPush(gitDirectory, commitMessage, token, name, email, cancellationToken)
                      .NoSync();
    }

    private static async ValueTask<string> GetCommitMessage(string libraryName, string resourcePath, string hash, CancellationToken cancellationToken)
    {
        string? version = null;
        if (Directory.Exists(resourcePath))
        {
            string versionPath = Path.Combine(resourcePath, "VERSION.txt");
            if (File.Exists(versionPath))
                version = await File.ReadAllTextAsync(versionPath, cancellationToken);
        }
        else if (File.Exists(resourcePath) && Path.GetExtension(resourcePath).Equals(".exe", System.StringComparison.OrdinalIgnoreCase))
        {
            version = System.Diagnostics.FileVersionInfo.GetVersionInfo(resourcePath).ProductVersion;
        }

        string details = string.IsNullOrWhiteSpace(version) ? $"SHA256 {hash[..System.Math.Min(12, hash.Length)]}" : version.Replace('\r', ' ').Replace('\n', ' ').Trim();
        return $"Update {libraryName} ({details})\n\nResource: {Path.GetFileName(resourcePath)}\nContent SHA256: {hash}";
    }

    private static string GetPathWithin(string gitDirectory, string path, string description)
    {
        string root = Path.GetFullPath(gitDirectory);
        string candidate = Path.GetFullPath(path, root);
        string rootPrefix = Path.TrimEndingDirectorySeparator(root) + Path.DirectorySeparatorChar;

        if (!candidate.StartsWith(rootPrefix, System.StringComparison.OrdinalIgnoreCase))
            throw new System.InvalidOperationException($"{description} must be located within the Git directory.");

        return candidate;
    }
}
