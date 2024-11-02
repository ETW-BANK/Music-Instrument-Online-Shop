﻿using MusicShop.Models;
using MusicShop.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicShop.Repository.Repository.IRepository
{
    public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {

        void Update (OrderHeader orderHeader);  
    }
}
