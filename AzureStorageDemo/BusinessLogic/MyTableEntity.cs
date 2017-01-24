using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageDemo.BusinessLogic
{
    public class MyTableEntity : TableEntity
    {

        public string Name { get; set; }

        public string Surname { get; set; }

    }
}
