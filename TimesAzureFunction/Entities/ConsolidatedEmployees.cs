using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimesAzureFunction.Entities
{
   public  class ConsolidatedEmployees : TableEntity
    {
        public string EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public double MinutesWorked { get; set; }


    }
}
