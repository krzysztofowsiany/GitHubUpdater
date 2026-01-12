using System;
using System.IO;
using System.IO.Compression;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: UpdaterAgent <zipPath> <targetDir>");
            return;
        }

        var zipPath = args[0];
        var targetDir = args[1];

        Console.WriteLine($"Updating from {zipPath} to {targetDir}...");

        using (var archive = ZipFile.OpenRead(zipPath))
        {
            foreach (var entry in archive.Entries)
            {
                var destinationPath = Path.Combine(targetDir, entry.FullName);

                if (string.IsNullOrEmpty(entry.Name))
                {
                    Directory.CreateDirectory(destinationPath);
                    continue;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

                try
                {
                    entry.ExtractToFile(destinationPath, overwrite: true);
                    Console.WriteLine($"Extracted: {entry.FullName}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Skipped (in use): {entry.FullName} - {ex.Message}");
                }
            }
        }

        var updateDir = Path.GetDirectoryName(zipPath);
        if (updateDir != null && Directory.Exists(updateDir))
        {
            Directory.Delete(updateDir, recursive: true);
            Console.WriteLine($"Cleaned up: {updateDir}");
        }

        Console.WriteLine("Update applied successfully.");
    }
}
