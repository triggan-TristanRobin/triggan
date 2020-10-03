using System;
using System.Collections.Generic;
using Model;
using Data;
using triggan.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class MessageRepository : IRepository<Message>
    {
        private readonly TrigganDBContext context;

        public MessageRepository()
        {
            context = new TrigganDBContext();
        }

        public MessageRepository(TrigganDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Message> Get()
        {
            return context.Messages.ToList();
        }

        public Message Get(int MessageId)
        {
            return context.Messages.Find(MessageId);
        }

        public IEnumerable<Message> Get(Expression<Func<Message, bool>> filter = null, Func<IQueryable<Message>, IOrderedQueryable<Message>> orderBy = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public void Insert(Message Message)
        {
            context.Messages.Add(Message);
        }

        public void Update(Message Message)
        {
            context.Entry(Message).State = EntityState.Modified;
        }

        public void Delete(Message Message)
        {
            context.Messages.Remove(Message);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
