using System.ComponentModel.DataAnnotations;

namespace Tasker.Core.Configuration;

/// <summary>
/// Represents the configuration settings for the Tasker system.
/// </summary>
public class TaskerConfig
{
    /// <summary>
    /// Indicates whether caching should be used.
    /// </summary>
    public bool? UseCache { get; set; }

    /// <summary>
    /// Indicates whether operations should be retried in case of failure.
    /// </summary>
    public bool? UseRetry { get; set; }

    /// <summary>
    /// The maximum number of concurrent tasks allowed.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than zero.")]
    public int? MaximumConcurrentTasks { get; set; }

    /// <summary>
    /// The maximum number of retry attempts allowed.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than zero.")]
    public int? MaximumRetryAttempts { get; set; }

    /// <summary>
    /// The maximum delay between retry attempts.
    /// </summary>
    public TimeSpan? MaximumRetryDelay { get; set; }

    /// <summary>
    /// The provider for the database.
    /// </summary>
    public DatabaseProvider DatabaseProvider { get; set; }

    /// <summary>
    /// The connection string for the database.
    /// </summary>
    public string? DatabaseConnectionString { get; set; }

    /// <summary>
    /// The connection string for the caching mechanism.
    /// </summary>
    public string? CacheConnectionString { get; set; }

    /// <summary>
    /// The name of the database used for caching.
    /// </summary>
    public string? CacheDatabaseName { get; set; }

    /// <summary>
    /// The name of the table used for caching.
    /// </summary>
    public string? CacheTableName { get; set; }

    /// <summary>
    /// The expiration time for cached items.
    /// </summary>
    public TimeSpan? CacheExpiration { get; set; }
}