using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;

namespace PaymentContext.Tests.Handlers.Mocks
{
    public class FakeStudentRepository : IStudentRepository
    {
        public void CreateSubscription(Student student)
        {
            throw new System.NotImplementedException();
        }

        public bool DocumentExists(string document)
        {
            if(document == "99999999999")
                return true;
            else
                return false;
        }

        public bool EmailExists(string email)
        {
            if(email == "hello@hotmail.com")
                return true;
            else
                return false;
        }
    }
}