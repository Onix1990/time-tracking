using Core.Domain.Entities;

namespace Api.Domain.Entities {
    public class User : Entity<long> {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
    }
}