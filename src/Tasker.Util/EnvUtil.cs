using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tasker.Util;

public static class EnvUtil
{
    public static IServiceCollection LoadEnvConfiguration(this IServiceCollection services)
    {
        var envVariables = LoadEnvFile();

        services.AddSingleton<IConfiguration>(provider =>
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(envVariables!);

            return configurationBuilder.Build();
        });

        return services;
    }

    private static Dictionary<string, string> LoadEnvFile()
    {
        var rootDirectory = GetProjectRoot();
        var envFilePath = Path.Combine(rootDirectory, ".env");

        if (!File.Exists(envFilePath))
            throw new FileNotFoundException("Environment file not found");

        return File.ReadLines(envFilePath)
            .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith($"#"))
            .Select(line => line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries))
            .Where(parts => parts.Length == 2)
            .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());
    }

    private static string GetProjectRoot()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var projectRoot = currentDirectory;

        while (!Directory.Exists(Path.Combine(projectRoot, "src")))
        {
            var parent = Directory.GetParent(projectRoot);
            if (parent == null || parent.FullName == projectRoot)
            {
                break;
            }

            projectRoot = parent.FullName;
        }

        return projectRoot;
    }
}