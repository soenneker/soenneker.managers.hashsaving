using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.HashSaving.Abstract;

/// <summary>
/// Handles hashing and saving
/// </summary>
public interface IHashSavingManager
{
    ValueTask SaveHashToGitRepo(string gitDirectory, string newHash, string fileName, string hashFileName, string name, string email, string username, string token, CancellationToken cancellationToken = default);
}
