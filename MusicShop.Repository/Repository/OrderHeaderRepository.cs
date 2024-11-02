using MusicShop.Data.Access.Data;
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
    public class OrderHeaderRepository:Repository<OrderHeader>,IOrderHeaderRepository
    {

        private readonly ApplicationDbContext _context;

        public OrderHeaderRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);  
        }
    }
}
