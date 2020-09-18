using System;

namespace Api.Domain.Dto {
    public class AuditOutDto {
        public long Id { get; set; }
        public string Description { get; set; }
        public int Hours { get; set; }
        public DateTime Date { get; set; }
        public UserOutDto User { get; set; }
    }
}