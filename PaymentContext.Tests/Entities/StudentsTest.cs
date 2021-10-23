using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.Entities
{
    
    [TestClass]
    public class StudentsTest
    {
        private readonly Name _name;
        private readonly Document _document;
        private readonly Email _email;
        private readonly Address _address;
        private readonly Student _student;
        private readonly Subscription _subscription;

        public StudentsTest()
        {
            _name = new Name("Bruce","Wayne");
            _document = new Document("34562456789", Domain.Enums.EDocumentType.CPF);
            _email = new Email("batman@dc.com");
            _address = new Address("Rua das Flores","70", "Conde Klaus","Gotham","SP","Brazil", "907854-980");
            _student = new Student(_name, _document, _email);
            _subscription = new Subscription(null);
            
        }
        [TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {
            var payment = new PayPalPayment("12345", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "Wayne Corp", _document, _address, _email);

            _subscription.AddPayment(payment);
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
        {
           _student.AddSubscription(_subscription);
           Assert.IsTrue(_student.Invalid);
        }

        public void ShouldReturnSuccessWhenAddSubscription()
        {
            var payment = new PayPalPayment("12345", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, "Wayne Corp", _document, _address, _email);

            _subscription.AddPayment(payment);
            _student.AddSubscription(_subscription);
            Assert.IsTrue(_student.Invalid);
        }
    }
}