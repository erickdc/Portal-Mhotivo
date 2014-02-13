﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Models;

namespace Mhotivo.App_Data.Repositories
{
    public class RoleRepository
    {
        private readonly MhotivoContext _context;

        public RoleRepository(MhotivoContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Role First(Expression<Func<Role, Role>> query)
        {
            return _context.Roles.Select(query).FirstOrDefault();
        }

        public Role GetById(long id)
        {
            return _context.Roles.First(x => x.RoleId == id);
        }

        public Role Create(Role itemToCreate)
        {
            return _context.Roles.Add(itemToCreate);
        }

        public IQueryable<Role> Query(Expression<Func<Role, Role>> expression)
        {
            return _context.Roles.Select(expression);
        }

        public Role Update(Role itemToUpdate)
        {
            _context.Roles.Attach(itemToUpdate);
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public void Delete(Role itemToDelete)
        {
            _context.Roles.Remove(itemToDelete);
        }
    }
}