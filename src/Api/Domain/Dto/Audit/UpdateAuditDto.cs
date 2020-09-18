using System;

namespace Api.Domain.Dto {
    public class UpdateAuditDto {
        public string Description { get; set; }
        public int? Hours { get; set; }
        public DateTime? Date { get; set; }
    }
}