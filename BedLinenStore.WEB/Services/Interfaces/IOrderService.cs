using System;
using System.Collections.Generic;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();

        void DeleteById(int id);

        Order GetById(int id);

        void Checkout(Order order);

        IEnumerable<Order> GetAllByPeriod(DateTime from, DateTime to);
    }
}