using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Tests.Handlers.Mocks;

namespace PaymentContext.Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Andressa";
            command.LastName = "Teste";
            command.Document = "99999999999";
            command.Email = "hello@hotmail.com";
            command.BarCode = "123456789";
            command.BoletoNumber = "134578928";
            command.PaymentNumber = "390847";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 55;
            command.TotalPaid = 55;
            command.Payer = "Andressa Ltda";
            command.PayerDocument = "89765432987";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "andressa@hotmail.com";
            command.Street = "Rua Teste";
            command.Number = "345";
            command.Neighborhood = "Bairro Teste";
            command.City = "Sao Paulo";
            command.State = "Sao Paulo";
            command.Country = "Brasil";
            command.ZipCode = "87689-045";

            handler.Handle(command);
            Assert.AreEqual(false, command.Valid);
        }
    }
}