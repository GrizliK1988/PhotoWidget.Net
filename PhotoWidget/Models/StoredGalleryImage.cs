namespace PhotoWidget.Models
{
    public class StoredGalleryImage
    {
        public StoredGalleryImage(string path, StorageType storageType)
        {
            StorageType = storageType;
            Path = path;
        }

        public string Path { get; private set; }

        public StorageType StorageType { get; private set; }
    }

    public enum StorageType
    {
        FileSystem,
        Dropbox
    };
}