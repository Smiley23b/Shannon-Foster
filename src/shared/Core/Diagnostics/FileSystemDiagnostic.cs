using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GitCredentialManager.Diagnostics
{
    public class FileSystemDiagnostic : Diagnostic
    {
        private readonly IFileSystem _fs;

        public FileSystemDiagnostic(IFileSystem fs)
            : base("File system")
        {
            EnsureArgument.NotNull(fs, nameof(fs));

            _fs = fs;
        }

        protected override Task<bool> RunInternalAsync(StringBuilder log, IList<string> additionalFiles)
        {
            string tempDir = Path.GetTempPath();
            log.AppendLine($"Temporary directory is '{tempDir}'...");

            log.AppendLine("Checking basic file I/O...");
            const string testContent = "Hello, GCM!";

            string fileName = Guid.NewGuid().ToString("N").Substring(8);
            string path = Path.Combine(tempDir, fileName);
            log.Append($"Writing to temporary file '{path}'...");
            File.WriteAllText(path, testContent);
            log.AppendLine(" OK");

            log.Append($"Reading from temporary file '{path}'...");
            string actualContent = File.ReadAllText(path);
            log.AppendLine(" OK");

            if (!StringComparer.Ordinal.Equals(testContent, actualContent))
            {
                log.AppendLine("File data did not match!");
                log.AppendLine($"Expected: {testContent}");
                log.AppendLine($"Actual: {actualContent}");
                return Task.FromResult(false);
            }

            log.Append($"Deleting temporary file '{path}'...");
            File.Delete(path);
            log.AppendLine(" OK");

            log.AppendLine("Testing IFileSystem instance...");
            log.AppendLine($"UserHomePath: {_fs.UserHomePath}");
            log.AppendLine($"UserDataDirectoryPath: {_fs.UserDataDirectoryPath}");
            log.AppendLine($"GetCurrentDirectory(): {_fs.GetCurrentDirectory()}");

            return Task.FromResult(true);
        }
    }
}
