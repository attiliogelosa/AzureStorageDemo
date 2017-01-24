using AzureStorageDemo.Utilities;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace AzureStorageDemo.BusinessLogic
{
    public class TableStorageDemo
    {

        #region Fields

        CloudTableClient client = null;

        #endregion

        #region Methods

        #region Constructors

        public TableStorageDemo(CloudTableClient client)
        {
            this.client = client;
        }

        #endregion

        public void TableStorage()
        {
            string tableName = Helpers.GetConfigurationValue("tableName");
            string tablePartition1 = Helpers.GetConfigurationValue("tablePartition1");
            string tablePartition2 = Helpers.GetConfigurationValue("tablePartition2");
            MyTableEntity myTableEntity1 = new MyTableEntity()
            {
                Name = "Attilio",
                Surname = "Gelosa",
                Timestamp = DateTime.Now,
                PartitionKey = tablePartition1,
                RowKey = "Attilio"
            };
            MyTableEntity myTableEntity2 = new MyTableEntity()
            {
                Name = "Neetu",
                Surname = "Srusti",
                Timestamp = DateTime.Now,
                PartitionKey = tablePartition1,
                RowKey = "Attilio2"
            };
            MyTableEntity myTableEntity3 = new MyTableEntity()
            {
                Name = "Stefano",
                Surname = "Paolozzi",
                Timestamp = DateTime.Now,
                PartitionKey = tablePartition1,
                RowKey = "Stefano"
            };
            MyTableEntity myTableEntity4 = new MyTableEntity()
            {
                Name = "Daniel",
                Surname = "Cali",
                Timestamp = DateTime.Now,
                PartitionKey = tablePartition2,
                RowKey = "Daniel"
            };
            string pattern = "At";
            int length = pattern.Length - 1;
            char lastChar = pattern[length];
            char nextLastChar = (char)(lastChar + 1);
            string endPattern = pattern.Substring(0, length) + nextLastChar;

            CloudTable table = client.GetTableReference(tableName);

            table.CreateIfNotExists();

            Console.WriteLine(string.Format("Table Storage created ({0})", tableName));

            TableOperation insertOperation = TableOperation.Insert(myTableEntity4);
            table.Execute(insertOperation);

            Console.WriteLine(string.Format("Insert operation ({0})", tableName));

            TableBatchOperation batchOperation = new TableBatchOperation();
            batchOperation.Insert(myTableEntity1);
            batchOperation.Insert(myTableEntity2);
            batchOperation.Insert(myTableEntity3);
            table.ExecuteBatch(batchOperation);

            Console.WriteLine(string.Format("Batch insert operation ({0})", tableName));
            Console.WriteLine();
            Console.WriteLine(string.Format("Retriving single entity  ({0})", tableName));

            TableOperation query = TableOperation.Retrieve<MyTableEntity>(tablePartition2, "Daniel");
            TableResult result = table.Execute(query);
            MyTableEntity myEntity = result.Result as MyTableEntity;

            Console.WriteLine(string.Format("  Name: {0}, Surname: {1}", myEntity.Name, myEntity.Surname));

            Console.WriteLine(string.Format("Retriving a range of entities  ({0})", tableName));

            string startsWithCondition = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, pattern),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, endPattern));
            string filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, tablePartition1),
                TableOperators.And,
                startsWithCondition);
            IEnumerable<MyTableEntity> list = table.ExecuteQuery<MyTableEntity>(new TableQuery<MyTableEntity>().Where(filter));

            foreach (MyTableEntity entity in list)
            {
                Console.WriteLine(string.Format("  Name: {0}, Surname: {1}", entity.Name, entity.Surname));
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to delete the table...");
            Console.ReadKey();


            table.DeleteIfExists();
            Console.WriteLine("Table deleted.");
            Console.WriteLine();
        }

        #endregion

    }
}
