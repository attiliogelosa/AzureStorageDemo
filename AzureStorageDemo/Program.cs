using AzureStorageDemo.BusinessLogic;
using AzureStorageDemo.Utilities;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Configuration;
using System.Text;

namespace AzureStorageDemo
{
    class Program
    {

        #region Fields

        const string HELP = "/?";
        const string BLOB_STORAGE = "Blob Storage";
        const string TABLE_STORAGE = "Table Storage";
        const string QUEUE_STORAGE = "Queue Storage";
        const string FILE_STORAGE = "File Storage";

        #endregion

        #region Main methods

        static void Main(string[] args)
        {
            int testNumber = 0;
            string demoName = string.Empty;

            try
            {
                if (args.Length != 1 || args[0] == HELP)
                {
                    Helpers.HelpOnLine();
                    return;
                }

                if (!int.TryParse(args[0], out testNumber) || testNumber < 1 || testNumber > 4)
                {
                    Helpers.HelpOnLine();
                    return;
                }

                switch (testNumber)
                {
                    case 1:
                        demoName = BLOB_STORAGE;
                        break;
                    case 2:
                        demoName = TABLE_STORAGE;
                        break;
                    case 3:
                        demoName = QUEUE_STORAGE;
                        break;
                    case 4:
                        demoName = FILE_STORAGE;
                        break;
                }

                Console.Clear();
                Console.WriteLine(string.Format("Demo \"{0}\" starts at {1}", demoName, DateTime.Now.ToString("o")));
                Console.WriteLine();

                string accountName = ConfigurationManager.AppSettings["accountName"].ToString();
                string accountKey = ConfigurationManager.AppSettings["accountKey"].ToString();

                Console.WriteLine(string.Format("Account Name ({0}) and Storage Access Key ({1}) retrived", accountName, Helpers.BuildKeyString(accountKey)));

                CloudStorageAccount account = CloudStorageAccountDemo.GetCloudStorageAccount(accountName, accountKey);

                Console.WriteLine("CloudStorageAccount builded");
                Console.WriteLine();

                switch (testNumber)
                {
                    case 1:
                        BlobStorageDemo storageDemo = new BlobStorageDemo(account.CreateCloudBlobClient());

                        storageDemo.BlobStorage();
                        break;
                    case 2:
                        TableStorageDemo tableStorageDemo = new TableStorageDemo(account.CreateCloudTableClient());

                        tableStorageDemo.TableStorage();
                        break;
                    case 3:
                        QueueStorageDemo queueStorageDemo = new QueueStorageDemo(account.CreateCloudQueueClient());

                        queueStorageDemo.QueueStorage();
                        break;
                    case 4:
                        FileStorageDemo fileStorageDemo = new FileStorageDemo(account.CreateCloudFileClient());

                        fileStorageDemo.FileStorage();
                        break;

                }
                Console.WriteLine();
                Console.WriteLine(string.Format("Demo ends at {0}", DateTime.Now.ToString("o")));
            }
            catch (Exception ex)
            {
                Exception inner = ex;
                StringBuilder sb = new StringBuilder();
                int counter = 1;

                sb.AppendLine();
                sb.AppendLine("Error(s):");
                while (inner != null)
                {
                    sb.AppendLine(string.Format("Message {0}: {1}", counter, inner.Message));
                    counter++;
                    inner = inner.InnerException;
                }
                Console.Write(sb.ToString());
            }
            finally
            {
                Console.WriteLine();

                Helpers.StopAndGo(Helpers.ExitOrContinue.Exit);
            }
        }

        #endregion

    }
}
