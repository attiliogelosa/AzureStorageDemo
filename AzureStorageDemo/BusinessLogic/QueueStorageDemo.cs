using AzureStorageDemo.Utilities;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AzureStorageDemo.BusinessLogic
{
    public class QueueStorageDemo
    {

        #region Fields

        CloudQueueClient client = null;

        #endregion

        #region Constructors
        
        public QueueStorageDemo(CloudQueueClient client)
        {
            this.client = client;
        }

        #endregion

        #region Methods

        public void QueueStorage()
        {
            string queueName = Helpers.GetConfigurationValue("queueName");

            CloudQueue queue = client.GetQueueReference(queueName);

            queue.CreateIfNotExists();

            Console.WriteLine(string.Format("Queue created ({0})", queueName));

            CloudQueueMessage message1 = new CloudQueueMessage("Hello World! Message 1");
            CloudQueueMessage message2 = new CloudQueueMessage("Hello World! Message 2");
            CloudQueueMessage message3 = new CloudQueueMessage("Hello World! Message 3");
            CloudQueueMessage message4 = new CloudQueueMessage("Hello World! Message 4");
            CloudQueueMessage message5 = new CloudQueueMessage("Hello World! Message 5");

            queue.AddMessage(message1);
            queue.AddMessage(message2);
            queue.AddMessage(message3);
            queue.AddMessage(message4);
            queue.AddMessage(message5);

            Console.WriteLine(string.Format("5 messages written to queue ({0})", queueName));
            Console.WriteLine();
            Console.WriteLine(string.Format("Peeking a message ({0})", queueName));

            CloudQueueMessage peekedMessage = queue.PeekMessage();

            Console.WriteLine(string.Format("  Message: {0}", peekedMessage.AsString));

            Console.WriteLine(string.Format("Peeking three messages ({0})", queueName));

            IEnumerable<CloudQueueMessage> peekedMessages = queue.PeekMessages(3);

            foreach (CloudQueueMessage item in peekedMessages)
            {
                Console.WriteLine(string.Format("  Message: {0}", item.AsString));
            }

            Console.WriteLine(string.Format("Peeking and hiding message for 5 seconds ({0})", queueName));

            CloudQueueMessage gettedMessage = queue.GetMessage(TimeSpan.FromSeconds(5));

            Console.WriteLine(string.Format("  Message: {0}", gettedMessage.AsString));

            Console.WriteLine(string.Format("Peeking 5 messages ({0})", queueName));

            IEnumerable<CloudQueueMessage> gettedMessages = queue.PeekMessages(5);

            foreach (CloudQueueMessage item in gettedMessages)
            {
                Console.WriteLine(string.Format("  Message: {0}", item.AsString));
            }

            Console.WriteLine("Waiting 10 seconds...");

            Thread.Sleep(10000);

            Console.WriteLine(string.Format("Peeking 5 messages again ({0})", queueName));

            gettedMessages = queue.PeekMessages(5);

            foreach (CloudQueueMessage item in gettedMessages)
            {
                Console.WriteLine(string.Format("  Message: {0}", item.AsString));
            }

            Console.WriteLine(string.Format("Dequeuing 1 message and picking 3 messages ({0})", queueName));

            CloudQueueMessage dequeuedMessage = queue.GetMessage();

            queue.DeleteMessage(dequeuedMessage);
            foreach (CloudQueueMessage item in queue.PeekMessages(3))
            {
                Console.WriteLine(string.Format("  Message: {0}", item.AsString));
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to delete the queue...");
            Console.ReadKey();

            queue.DeleteIfExists();
            Console.WriteLine("Queue deleted.");
            Console.WriteLine();
        }

        #endregion

    }
}
