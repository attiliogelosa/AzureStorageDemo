using AzureStorageDemo.Utilities;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageDemo.BusinessLogic
{
    public class BlobStorageDemo
    {

        #region Fields

        CloudBlobClient client = null;

        string containerName = Helpers.GetConfigurationValue("containerName");

        string blobFile1To = Helpers.GetConfigurationValue("blobFile1To");
        string blobFile2To = Helpers.GetConfigurationValue("blobFile2To");
        string appendFileTo = Helpers.GetConfigurationValue("appendFileTo");
        string pageFileTo = Helpers.GetConfigurationValue("pageFileTo");

        string blobFileFrom = Helpers.GetConfigurationValue("blobFileFrom");
        string pageFileFrom = Helpers.GetConfigurationValue("pageFileFrom");
        string newText = Helpers.GetConfigurationValue("newText");

        #endregion

        #region Constructors
        public BlobStorageDemo(CloudBlobClient client)
        {
            this.client = client;
        }

        #endregion

        #region Methods

        public void BlobStorage()
        {
            CloudBlobContainer container = client.GetContainerReference(containerName);

            container.CreateIfNotExists();

            Console.WriteLine(string.Format("Container created ({0})", containerName));

            container.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            Console.WriteLine(string.Format("Container permission setted up ({0})", containerName));
            Console.WriteLine();

            WorkingWithBlockBlob(container);
            WorkingWithAppendBlob(container);
            WorkingWithPageBlob(container);

            Console.WriteLine("Press any key to delete the container...");
            Console.ReadKey();

            container.DeleteIfExists();

            Console.WriteLine("Container deleted");
        }

        protected void WorkingWithBlockBlob(CloudBlobContainer container)
        {
            Console.WriteLine("==>  Working with BLOCK BLOB");
            Console.WriteLine();

            CloudBlockBlob cloudBlob1 = container.GetBlockBlobReference(blobFile1To);

            using (Stream stream = File.OpenRead(blobFileFrom))
            {
                cloudBlob1.UploadFromStream(stream);
            }

            Console.WriteLine(string.Format("Block Blob uploaded ({0})", blobFile1To));

            CloudBlockBlob clodBlob2 = container.GetBlockBlobReference(blobFile2To);

            using (Stream stream = File.OpenRead(blobFileFrom))
            {
                clodBlob2.UploadFromStream(stream);
            }

            Console.WriteLine(string.Format("Block Blob uploaded ({0})", blobFile2To));
            Console.WriteLine();
            Console.WriteLine("Listing blobs in container (root):");

            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("  File: ...{0}", blob.Uri.ToString().Substring(10));
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("  Directory: ...{0}", directory.Uri.ToString().Substring(10));
                }
            }

            Helpers.StopAndGo();
        }

        protected void WorkingWithAppendBlob(CloudBlobContainer container)
        {
            Console.WriteLine("==>  Working with APPEND BLOB");
            Console.WriteLine();

            CloudAppendBlob appendBlob = container.GetAppendBlobReference(appendFileTo);

            appendBlob.CreateOrReplace();

            Console.WriteLine(string.Format("Append Blob created ({0})", appendFileTo));
            Console.WriteLine(string.Format("Writing text to Append Blob ({0})...", appendFileTo));

            Task task1 = Task.Factory.StartNew(() => AddTextToAppendBlob(1, appendBlob));
            Task task2 = Task.Factory.StartNew(() => AddTextToAppendBlob(2, appendBlob));
            Task task3 = Task.Factory.StartNew(() => AddTextToAppendBlob(3, appendBlob));
            Task task4 = Task.Factory.StartNew(() => AddTextToAppendBlob(4, appendBlob));
            Task task5 = Task.Factory.StartNew(() => AddTextToAppendBlob(5, appendBlob));
            Task.WaitAll(task1, task2, task3, task4, task5);

            Console.WriteLine(string.Format("All texts appened ({0})", appendFileTo));

            Helpers.StopAndGo();
        }

        protected void WorkingWithPageBlob(CloudBlobContainer container)
        {
            int fileSize = 1024 * 1024;
            int pageSize = 512;
            byte[] page = new byte[512];

            Console.WriteLine("==>  Working with PAGE BLOB");
            Console.WriteLine();

            CloudPageBlob pageBlob = container.GetPageBlobReference(pageFileTo);

            pageBlob.Create(fileSize);

            Console.WriteLine(string.Format("Page Blob created ({0})", pageFileTo));

            using (StreamReader reader = File.OpenText(pageFileFrom))
            {
                pageBlob.WritePages(reader.BaseStream, 0);
            }

            Console.WriteLine(string.Format("Page Blob uploaded ({0})", pageFileTo));

            pageBlob.DownloadRangeToByteArray(page, 0, pageSize, pageSize);

            Console.WriteLine(string.Format("Page (of {0} bytes) downloaded from Page Blob ({1})", pageSize, pageFileTo));

            using (MemoryStream stream = new MemoryStream(page))
            {
                Encoding.ASCII.GetBytes(newText).CopyTo(page, 5);
                pageBlob.WritePages(stream, pageSize);
            }

            Console.WriteLine(string.Format("Page Blob edited ({0})", pageFileTo));

            Helpers.StopAndGo();
        }

        public static void AddTextToAppendBlob(int id, CloudAppendBlob clodAppendBlob)
        {
            for (int i = 0; i < 10; i++)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                    {
                        string text = string.Format("Task id: {0}, iteration: {1}, at: {2}", id, i, DateTime.Now.ToString("o"));

                        streamWriter.WriteLine(text);
                        streamWriter.Flush();
                        memoryStream.Position = 0;
                        clodAppendBlob.AppendBlock(memoryStream);
                    }
                }
            }
        }

        #endregion

    }
}
