using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
                Notifiable,
                IHandler<CreateBoletoSubscriptionCommand>,
                IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }
        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            command.Validate();

            if(command.Valid)
            {
                //Para agrupar as notificações
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            if(_repository.DocumentExists(command.Document))
                AddNotification("Document","Este CPF já está em uso");

            if(_repository.EmailExists(command.Email))
                AddNotification("Email","Este e-mail já está em uso");

            //checar as notificações
            if(Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            //Gerar value onjects
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, Enums.EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //gerar as entidades
            var student = new Student(name, document,email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument , command.PayerDocumentType),
                address,
                email
            );

            //relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as notificações
            AddNotifications(name, document, email, student, subscription, payment);

            //Salvar as informções
            _repository.CreateSubscription(student);

            // Enviar e-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo","Sua assinatura foi criada");

            
            //retornar
            return new CommandResult(true, "Assinatura cadastrada com sucesso!");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            if(_repository.DocumentExists(command.Document))
                AddNotification("Document","Este CPF já está em uso");

            if(_repository.EmailExists(command.Email))
                AddNotification("Email","Este e-mail já está em uso");

            //Gerar value onjects
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, Enums.EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            //Gerar as entidades
            var student = new Student(name, document,email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument , command.PayerDocumentType),
                address,
                email
            );

            //relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as notificações
            AddNotifications(name, document, email, student, subscription, payment);

            //Salvar as informções
            _repository.CreateSubscription(student);

            // Enviar e-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo","Sua assinatura foi criada");

            
            //retornar
            return new CommandResult(true, "Assinatura cadastrada com sucesso!");
        }
    }
}