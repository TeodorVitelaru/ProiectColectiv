namespace DatingApp.Domain.Primitives
{
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>> where TKey : struct
    {
        public TKey Id { get; private set; }

        protected Entity()
        {
        }
        protected Entity(TKey id)
        {
            Id = id;
        }

        public void SetId(TKey id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            if (obj is not Entity<TKey> entity)
            {
                return false;
            }

            return EqualityComparer<TKey>.Default.Equals(entity.Id, Id);
        }

        public bool Equals(Entity<TKey>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return EqualityComparer<TKey>.Default.Equals(other.Id, Id);
        }
    }
}
