using System;

namespace Api.Domain.Dto {
    public class CreateAuditDto {
        public string Description { get; set; }
        public int? Hours { get; set; }
        public DateTime? Date { get; set; }
        public long? UserId { get; set; }
    }
}