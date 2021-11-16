using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.Queries
{
    [TestClass]
    public class StudentQueriesTest
    {
        private IList<Student> _students;
        public StudentQueriesTest()
        {
            for (var i = 0; i <= 10; i++)
            {
                _students.Add(new Student(
                    new Name("Aluno", i.ToString()),
                    new Document("12345678911" + i.ToString() , EDocumentType.CPF),
                    new Email(i.ToString() + "@hotmail.com")
                ));
            }
        }
        
        [TestMethod]
        public void ShouldReturnNullWhenDocummentNotExists()
        {
            var exp = StudentQueries.GetStudentInfo("1235670389");
            var student = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreEqual(null, student);
        }

        [TestMethod]
        public void ShouldReturnNullWhenDocummentExists()
        {
            var exp = StudentQueries.GetStudentInfo("12345678912");
            var student = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreEqual(null, student);
        }
    }
}