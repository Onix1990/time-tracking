namespace Core.Domain.Entities {
    public abstract class Entity<TId> : IEntity<TId> {
        public virtual TId Id { get; set; }

        public override bool Equals(object obj) {
            if (!(obj is Entity<TId> item)) {
                return false;
            }

            return GetType().IsInstanceOfType(obj) && Equals(item.Id, Id);
        }

        public static bool operator ==(Entity<TId> x, Entity<TId> y) {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.Equals(y);
        }

        public static bool operator !=(Entity<TId> x, Entity<TId> y) {
            return !(x == y);
        }

        public override int GetHashCode() => (Id, GetType()).GetHashCode();
    }
}