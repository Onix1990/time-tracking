﻿namespace Api.Domain.Dto {
    public class CreateUserDto {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
    }
}