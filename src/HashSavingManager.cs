using Soenneker.Managers.HashSaving.Abstract;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Soenneker.Git.Util.Abstract;
using Soenneker.Utils.File.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.Directory.Abstract;

namespace Soenneker.Managers.HashSaving;

/// <inheritdoc cref="IHashSavingManager"/>
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

    public async ValueTask SaveHashToGitRepoAsFile(string gitDirectory, string newHash, string fileName, string hashFileName, string name, string email, string username, string token,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving hash to Git repo...");

        // Write new hash
        string targetHashFile = Path.Combine(gitDirectory, hashFileName);
        await _fileUtil.DeleteIfExists(targetHashFile, cancellationToken: cancellationToken).NoSync();
        await _fileUtil.Write(targetHashFile, newHash, true, cancellationToken).NoSync();

        // Clean up the resource file from the repo
        string resourceFile = Path.Combine(gitDirectory, "src", "Resources", fileName);
        await _fileUtil.DeleteIfExists(resourceFile, cancellationToken: cancellationToken).NoSync();

        // Stage the new hash file
        await _gitUtil.AddIfNotExists(gitDirectory, targetHashFile, cancellationToken).NoSync();

        await _gitUtil.CommitAndPush(gitDirectory, "Updates hash for new version", token, name, email, cancellationToken)
                      .NoSync();
    }

    public async ValueTask SaveHashToGitRepoAsDirectory(string gitDirectory, string newHash, string targetDir, string hashFileName, string name, string email, string username, string token,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving hash to Git repo...");

        // Write new hash
        string targetHashFile = Path.Combine(gitDirectory, hashFileName);
        await _fileUtil.DeleteIfExists(targetHashFile, cancellationToken: cancellationToken).NoSync();
        await _fileUtil.Write(targetHashFile, newHash, true, cancellationToken).NoSync();

        _directoryUtil.Delete(targetDir);

        // Stage the new hash file
        await _gitUtil.AddIfNotExists(gitDirectory, targetHashFile, cancellationToken).NoSync();

        await _gitUtil.CommitAndPush(gitDirectory, "Updates hash for new version", token, name, email, cancellationToken)
                      .NoSync();
    }
}