using System;
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
        private readonly ICartLineService cartLineService;
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;

        public OrderService(ApplicationDbContext context,
            IUserService userService,
            ICartLineService cartLineService)
        {
            this.context = context;
            this.userService = userService;
            this.cartLineService = cartLineService;
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

        public void Checkout(Order order)
        {
            context.Orders.Add(order);

            var user = userService.GetByEmail(order.Email);
            var cartLine = cartLineService.GetByEmail(order.Email);
            user.CartLine = new CartLine();
            cartLineService.Delete(cartLine);

            context.SaveChanges();
        }

        public IEnumerable<Order> GetAllByPeriod(DateTime from, DateTime to)
        {
            return context.Orders
                .Include(item => item.Products)
                .ThenInclude(a => a.Category)
                .Include(item => item.Products)
                .ThenInclude(a => a.MainInfo)
                .Where(order => order.CreatedDate >= from && order.CreatedDate <= to)
                .ToList();
        }
    }
}