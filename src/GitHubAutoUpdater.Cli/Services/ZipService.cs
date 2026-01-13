using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace GitHubAutoUpdater.Cli.Services;

internal class ZipService
{
    public string CreateZip(string sourcePath, string outputDir)
    {
        string zipFile = Path.Combine(outputDir, $"app_{System.IO.Path.GetFileName(sourcePath)}.zip");
        if (File.Exists(zipFile))
            File.Delete(zipFile);

        ZipFile.CreateFromDirectory(sourcePath, zipFile, CompressionLevel.Optimal, false);
        return zipFile;
    }

    public string GenerateManifest(string zipPath, string version)
    {
        var manifest = new
        {
            version,
            file = Path.GetFileName(zipPath),
            checksum = ComputeChecksum(zipPath)
        };

        string manifestPath = Path.Combine(System.IO.Path.GetTempPath(), "manifest.json");
        File.WriteAllText(manifestPath, JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true }));
        return manifestPath;
    }

    private string ComputeChecksum(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        byte[] hash = sha256.ComputeHash(stream);
        return Convert.ToHexString(hash).ToLower();
    }
}
