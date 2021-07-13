﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MochaCoffee.Web.ViewModels
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CustomerId { get; set; }
        public List<SalesOrderItemModel> LineItems { get; set; }

    }
}
