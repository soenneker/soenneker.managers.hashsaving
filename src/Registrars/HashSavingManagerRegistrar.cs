using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Git.Util.Registrars;
using Soenneker.Managers.HashSaving.Abstract;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.FileSync.Registrars;

namespace Soenneker.Managers.HashSaving.Registrars;

/// <summary>
/// Handles hashing and saving
/// </summary>
public static class HashSavingManagerRegistrar
{
    /// <summary>
    /// Adds <see cref="IHashSavingManager"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddHashSavingManagerAsSingleton(this IServiceCollection services)
    {
        services.AddFileUtilAsSingleton()
                .AddGitUtilAsSingleton()
                .AddFileUtilSyncAsSingleton()
                .TryAddSingleton<IHashSavingManager, HashSavingManager>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IHashSavingManager"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddHashSavingManagerAsScoped(this IServiceCollection services)
    {
        services.AddFileUtilAsScoped()
                .AddGitUtilAsScoped()
                .AddFileUtilSyncAsScoped()
                .TryAddScoped<IHashSavingManager, HashSavingManager>();

        return services;
    }
}