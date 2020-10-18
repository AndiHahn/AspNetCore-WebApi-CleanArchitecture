namespace CleanArchitecture.Core.Interfaces
{
    public class Constants
    {
        public static class Authentication
        {
            public const int SALT_SIZE = 16;
        }

        public static class ImageStorage
        {
            public const string CONTAINER_NAME = "bills";
            public const string IMAGES_FOLDER_NAME = "images";
            public const string FOLDER_DELIMITER = "/";
        }

        public static class Common
        {
            public const string ENTITY_POSTFIX = "Entity";
        }
    }
}
