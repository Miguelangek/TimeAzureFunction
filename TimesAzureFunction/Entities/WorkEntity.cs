using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimesAzureFunction.Entities
{
    public class WorkEntity : TableEntity
    {
        public string EmployeeId { get; set; }

        public DateTime WorkingTime { get; set; }

        public string Type { get; set; }

        public bool Consolidate { get; set; }
    }
}
