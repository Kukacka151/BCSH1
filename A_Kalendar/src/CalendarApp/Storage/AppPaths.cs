using System;
using System.IO;

namespace CalendarApp.Storage;

public static class AppPaths
{
    public static string GetDataDirectory()
    {
        var baseDir = AppContext.BaseDirectory;

        // Try to locate the repository's `data` folder by walking up.
        var dir = new DirectoryInfo(baseDir);
        for (var i = 0; i < 8 && dir != null; i++)
        {
            var candidate = Path.Combine(dir.FullName, "data");
            if (Directory.Exists(candidate))
                return candidate;

            dir = dir.Parent;
        }

        // Fallback: keep data next to the executable.
        var fallback = Path.Combine(baseDir, "data");
        Directory.CreateDirectory(fallback);
        return fallback;
    }
}

