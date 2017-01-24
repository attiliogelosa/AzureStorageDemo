using AzureStorageDemo.Utilities;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.IO;

namespace AzureStorageDemo.BusinessLogic
{

    public class FileStorageDemo
    {

        #region Fields

        CloudFileClient client = null;

        #endregion

        #region Constructors

        public FileStorageDemo(CloudFileClient client)
        {
            this.client = client;
        }

        #endregion

        #region Methods

        public void FileStorage()
        {
            string shareName = Helpers.GetConfigurationValue("shareName");
            string folderName = Helpers.GetConfigurationValue("folderName");
            string fileName = Helpers.GetConfigurationValue("fileName");
            string filePath = Helpers.GetConfigurationValue("filePath");

            CloudFileShare share = client.GetShareReference(shareName);

            share.CreateIfNotExists();

            Console.WriteLine(string.Format("Share created ({0})", shareName));

            CloudFileDirectory folder = share.GetRootDirectoryReference().GetDirectoryReference(folderName);

            folder.CreateIfNotExists();

            Console.WriteLine(string.Format("Folder creating ({0})", folderName));

            CloudFile file = folder.GetFileReference(fileName);

            using (Stream fileStream = File.OpenRead(filePath))
            {
                file.UploadFromStream(fileStream);
            }

            Console.WriteLine(string.Format("File uploaded ({0})", fileName));
            Console.WriteLine();
            Console.WriteLine("Now you can map a network drive in File Explorer with the following 2 lines:");
            Console.WriteLine();
            Console.WriteLine(@"cmdkey /add:[ACCOUNT_NAME].file.core.windows.net /user:[ACCOUNT_NAME] /pass:[STORAGE_KEY]");
            Console.WriteLine(@"net use [DRIVE_LETTER]: \\[ACCOUNT_NAME].file.core.windows.net\[SHARE-NAME] /u:[ACCOUNT_NAME] [STORAGE_KEY]");

            Console.WriteLine();
            Console.WriteLine("Press any key to delete the share...");
            Console.ReadKey();

            share.DeleteIfExists();
            Console.WriteLine("Share deleted.");
            Console.WriteLine();
        }

        #endregion

    }

}
