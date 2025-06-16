using Soenneker.Managers.HashSaving.Abstract;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Soenneker.Git.Util.Abstract;
using Soenneker.Utils.File.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.FileSync.Abstract;

namespace Soenneker.Managers.HashSaving;

/// <inheritdoc cref="IHashSavingManager"/>
public sealed class HashSavingManager : IHashSavingManager
{
    private readonly ILogger<HashSavingManager> _logger;
    private readonly IFileUtil _fileUtil;
    private readonly IFileUtilSync _fileUtilSync;
    private readonly IGitUtil _gitUtil;

    public HashSavingManager(ILogger<HashSavingManager> logger, IFileUtil fileUtil, IFileUtilSync fileUtilSync, IGitUtil gitUtil)
    {
        _logger = logger;
        _fileUtil = fileUtil;
        _fileUtilSync = fileUtilSync;
        _gitUtil = gitUtil;
    }

    public async ValueTask SaveHashToGitRepo(string gitDirectory, string newHash, string fileName, string hashFileName, string name, string email, string username, string token,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving hash to Git repo...");

        // Write new hash
        string targetHashFile = Path.Combine(gitDirectory, hashFileName);
        _fileUtilSync.DeleteIfExists(targetHashFile);
        await _fileUtil.Write(targetHashFile, newHash, true, cancellationToken).NoSync();

        // Clean up the resource file from the repo
        string resourceFile = Path.Combine(gitDirectory, "src", "Resources", fileName);
        _fileUtilSync.DeleteIfExists(resourceFile);

        // Stage the new hash file
        _gitUtil.AddIfNotExists(gitDirectory, targetHashFile);

        await _gitUtil.CommitAndPush(gitDirectory, username, name, email, token, "Updates hash for new version", cancellationToken)
                      .NoSync();
    }
}