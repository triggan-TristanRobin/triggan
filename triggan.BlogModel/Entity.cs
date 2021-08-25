using System;
using System.Reflection;

namespace triggan.BlogModel
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.MinValue;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string Slug { get; set; }
        public int Stars { get; set; }
        public bool Deleted { get; set; }

        public void Update(Entity updatedEntity)
        {
            var entityType = updatedEntity.GetType();
            if (entityType != GetType())
            {
                throw new TypeAccessException($"Cannot update entity of type {GetType()} with entity of type {entityType}");
            }
            foreach (PropertyInfo property in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.Name == "Id")
                {
                    continue;
                }
                var newValue = property.GetValue(updatedEntity);
                property.SetValue(this, newValue);
            }
        }
    }
}