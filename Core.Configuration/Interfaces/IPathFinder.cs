namespace Core.Configuration.Interfaces
{
    public interface IPathFinder
    {
        string FindFilePathInAncestors(string fileName, string currentDirectory = null);
    }
}