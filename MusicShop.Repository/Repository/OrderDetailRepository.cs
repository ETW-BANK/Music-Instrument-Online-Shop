﻿using MusicShop.Data.Access.Data;
using MusicShop.Models;
using MusicShop.Repository.Repository.IRepository;
using MusicShop.Repository.Rpository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicShop.Repository.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {

        private ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }
    }
}
