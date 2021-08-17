﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ISlugRepository<TEntity> : IRepository<TEntity> where TEntity: Entity
    {
        TEntity Get(string slug);
    }
}
