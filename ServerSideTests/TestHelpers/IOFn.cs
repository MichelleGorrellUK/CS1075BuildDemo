using System.IO;

namespace Support.DataConductor.ServerTests.TestHelpers
{
    public static class IOFn
    {
        public static void ManageTestOutputFolders(this string directory, FolderAction folderAction)
        {
            switch (folderAction)
            {
                case FolderAction.Create:
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                    break;
                case FolderAction.Delete:
                    if (Directory.Exists(directory))
                        Directory.Delete(directory, true);
                    break;
            }
        }

        public enum FolderAction { Create, Delete }
    }
}
