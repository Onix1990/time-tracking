using System;
using Core.Domain.Entities;

namespace Api.Domain.Entities {
    public class Audit : Entity<long> {
        public string Description { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
    }
}