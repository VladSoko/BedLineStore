using System.Collections.Generic;
using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public IEnumerable<Order> GetAll()
        {
            return context.Orders
                .Include(item => item.Products)
                .ThenInclude(a => a.Category)
                .Include(item => item.Products)
                .ThenInclude(a => a.MainInfo).ToList();
        }

        public void DeleteById(int id)
        {
            var order = context.Orders.FirstOrDefault(item => item.Id == id);
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public Order GetById(int id)
        {
            return context.Orders
                .Include(item => item.Products)
                .ThenInclude(a => a.Category)
                .Include(item => item.Products)
                .ThenInclude(a => a.MainInfo)
                .FirstOrDefault(item => item.Id == id);
        }

        public void Create(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}