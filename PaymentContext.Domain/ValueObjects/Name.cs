using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            AddNotification("Name.LastName","O nome deve conter pelo menos 3 caracteres");
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
    }
}