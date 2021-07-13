﻿using MochaCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MochaCoffee.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetOrders();
        ServiceResponse<bool> GenerateOpenOrder(SalesOrder order);
        ServiceResponse<bool> MarkFulfilled(int id);
    }
}
