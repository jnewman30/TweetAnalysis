using System;
using System.IO;
using Core.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.ConfigurationTests
{
    [TestClass]
    public class PathFinderTests
    {
        private const string SettingsFileName = "environmentSettings.json";
        private readonly string _relativeSettingsPath = $"../../../../{SettingsFileName}";

        private static string BogusFileName => $"{Guid.NewGuid():N}.json";
        private static string BogusFolderName => $"C:/{Guid.NewGuid():N}";
        
        [TestMethod]
        public void Can_Find_Path()
        {
            var expectedPath = Path.GetFullPath(_relativeSettingsPath);

            var pathFinder = new PathFinder();
            var actualPath = pathFinder.FindFilePathInAncestors(SettingsFileName);

            Assert.AreEqual(expectedPath, actualPath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Null_FileName_Throws()
        {
            var pathFinder = new PathFinder();
            pathFinder.FindFilePathInAncestors(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Folder_InvalidCharacters_Throws()
        {
            var pathFinder = new PathFinder();
            var folderName = new string(Path.GetInvalidPathChars());
            pathFinder.FindFilePathInAncestors(BogusFileName, folderName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void File_InvalidCharacters_Throws()
        {
            var pathFinder = new PathFinder();
            var fileName = new string(Path.GetInvalidFileNameChars());
            var folderName = Directory.GetCurrentDirectory();
            pathFinder.FindFilePathInAncestors(fileName, folderName);
        }

        /// <summary>
        /// Return null if not found because we may want to know as opposed to throwing exception.
        /// </summary>        
        [TestMethod]
        public void Returns_Null_If_File_Not_Found()
        {
            var pathFinder = new PathFinder();
            var actualPath = pathFinder.FindFilePathInAncestors(BogusFileName);
            Assert.IsNull(actualPath, "Should not find a file.");
        }

        /// <summary>
        /// Return null if not found because we may want to know as opposed to throwing exception.
        /// </summary>
        [TestMethod]
        public void Returns_Null_If_Folder_Not_Found()
        {
            var pathFinder = new PathFinder();
            var actualPath = pathFinder.FindFilePathInAncestors(
                BogusFileName, BogusFolderName);
            Assert.IsNull(actualPath, "Should not find a folder/file.");
        }
    }
}