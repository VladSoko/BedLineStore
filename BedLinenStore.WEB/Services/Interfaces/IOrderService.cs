using System.Collections;
using System.Collections.Generic;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();

        void DeleteById(int id);

        Order GetById(int id);

        void Create(Order order);
    }
}