using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace AzureStorageDemo.BusinessLogic
{

    public class CloudStorageAccountDemo
    {

        public static CloudStorageAccount GetCloudStorageAccount(string accountName, string accountKey)
        {
            StorageCredentials credentials = new StorageCredentials(accountName, accountKey);

            return new CloudStorageAccount(credentials, true);

        }

    }

}
