using System;

namespace Core.Domain.Exceptions {
    public class ObjectNotFoundException : Exception {
        public ObjectNotFoundException() { }

        public ObjectNotFoundException(string message) : base(message) { }
    }
}