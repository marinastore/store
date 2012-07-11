using System;

namespace Marina.Store.Web.Models
{
    public class RegistrationRequest
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public ShoppingCart Cart { get; set; }

        public int? ShoppingCartId { get; set; }
    }
}