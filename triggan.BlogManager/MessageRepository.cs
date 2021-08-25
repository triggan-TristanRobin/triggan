using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.BlogManager
{
    public class MessageRepository : IRepository<Message>
    {
        private readonly TrigganContext context;

        public MessageRepository(TrigganContext context)
        {
            this.context = context;
        }

        public IEnumerable<Message> GetAll()
        {
            return context.Messages.ToList();
        }

        public Message Get(int MessageId)
        {
            return context.Messages.Find(MessageId);
        }

        public IEnumerable<Message> Get(Expression<Func<Message, bool>> filter = null, int count = 0, string includeProperties = "")
        {
            IQueryable<Message> query = context.Messages;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            query = count == 0 ? query : query.Take(count);

            return query.ToList();
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