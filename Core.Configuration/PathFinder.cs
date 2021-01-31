using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Configuration.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Configuration
{
    public class PathFinder : IPathFinder
    {
        private static readonly char[] InvalidPathCharacters;
        private static readonly char[] InvalidFileNameCharacters;

        static PathFinder()
        {
            InvalidPathCharacters = Path.GetInvalidPathChars();
            InvalidFileNameCharacters = Path.GetInvalidFileNameChars();
        }

        /// <summary>Walks paths from the current directory backward looking for file.</summary>
        /// <remarks>Returns full path of file or null if file and/or folder is not found in case user is looking for a file and/or folder not yet created.</remarks> 
        /// <param name="fileName">The name of the file to find.</param>
        /// <param name="currentDirectory">The directory to start walking from. If null start from currently executing assembly path.</param>
        /// <exception cref="ArgumentException">Throws for invalid file and folder characters.</exception>
        /// <exception cref="ArgumentException">Throws for empty fileName argument.</exception>
        /// <exception cref="ArgumentException">Throws if <param name="currentDirectory"></param> is null and Assembly location cannot be found or access is denied.</exception>
        public string FindFilePathInAncestors(string fileName, string currentDirectory = null)
        {
            var checkDirectory = CheckDirectoryAndThrow(currentDirectory);

            // Specifically check this case... may want to look for file in directory that is not yet created.
            if (checkDirectory == null)
            {
                return null;
            }

            CheckFileNameAndThrow(fileName);
            
            var fileCheck = Path.Combine(checkDirectory, fileName);
            if (File.Exists(fileCheck))
            {
                return fileCheck;
            }
            
            var nextDirectoryInfo = Directory.GetParent(checkDirectory);
            if (nextDirectoryInfo == null)
            {
                // Return null in this cass... may want to look for a file not yet created.
                return null;
            }

            var nextDirectory = nextDirectoryInfo.FullName;
            return FindFilePathInAncestors(fileName, nextDirectory);
        }

        private static void CheckFileNameAndThrow(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("FileName is required and cannot be null, empty or whitespace.");
            }
            
            if (StringHasCharacters(fileName, InvalidFileNameCharacters))
            {
                throw new ArgumentException($"FileName \"{fileName}\" has invalid characters.");
            }
        }

        private static string CheckDirectoryAndThrow(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                return GetExecutingAssemblyDirectory();
            }
            
            if (StringHasCharacters(directory, InvalidPathCharacters))
            {
                throw new ArgumentException($"Directory \"{directory} has invalid characters.");
            }

            if (!Directory.Exists(directory))
            {
                return null;
            }

            return directory;
        }

        private static bool StringHasCharacters(string text, char[] characters)
        {
            return text.Any(characters.Contains);
        }
        
        private static string GetExecutingAssemblyDirectory()
        {
            var currentAssembly = Assembly
                .GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(currentAssembly);

            if (string.IsNullOrWhiteSpace(assemblyDirectory))
            {
                throw new IOException("There was a problem find the directory of the currently executing assembly.");
            }

            return assemblyDirectory;
        }
    }
}