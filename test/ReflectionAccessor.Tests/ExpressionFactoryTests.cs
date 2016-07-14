using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ReflectionAccessor.Tests.Models;
using Xunit;

namespace ReflectionAccessor.Tests
{
    public class ExpressionFactoryTests
    {
        [Fact]
        public void CreateConstructor()
        {
            var constructor = ExpressionFactory.CreateConstructor(typeof(Contact));
            Assert.NotNull(constructor);

            var contactDelegate = constructor() as Contact;
            Assert.NotNull(contactDelegate);
            Assert.IsType<Contact>(contactDelegate);

        }

        [Fact]
        public void CreateGetProperty()
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Jim",
                LastName = "Bob",
            };

            var type = typeof(Contact);
            var firstProperty = type.GetTypeInfo().GetProperty("FirstName");
            Assert.NotNull(firstProperty);

            var firstDelegate = ExpressionFactory.CreateGet(firstProperty);
            Assert.NotNull(firstDelegate);

            var firstName = firstDelegate(contact) as string;
            Assert.Equal(contact.FirstName, firstName);
        }

        [Fact]
        public void CreateSetProperty()
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Jim",
                LastName = "Bob",
            };

            var type = typeof(Contact);
            var firstProperty = type.GetTypeInfo().GetProperty("FirstName");
            Assert.NotNull(firstProperty);

            var firstDelegate = ExpressionFactory.CreateSet(firstProperty);
            Assert.NotNull(firstDelegate);

            firstDelegate(contact, "Jimmy");
            Assert.Equal("Jimmy", contact.FirstName);
        }

        [Fact]
        public void CreateGetField()
        {
            var contact = new ContactField
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Jim",
                LastName = "Bob",
            };

            var type = typeof(ContactField);
            var firstField = type.GetTypeInfo().GetField("FirstName");
            Assert.NotNull(firstField);

            var firstDelegate = ExpressionFactory.CreateGet(firstField);
            Assert.NotNull(firstDelegate);

            var firstName = firstDelegate(contact) as string;
            Assert.Equal(contact.FirstName, firstName);

        }

        [Fact]
        public void CreateSetField()
        {
            var contact = new ContactField
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Jim",
                LastName = "Bob",
            };

            var type = typeof(ContactField);
            var firstProperty = type.GetTypeInfo().GetField("FirstName");
            Assert.NotNull(firstProperty);

            var firstDelegate = ExpressionFactory.CreateSet(firstProperty);
            Assert.NotNull(firstDelegate);

            firstDelegate(contact, "Jimmy");
            Assert.Equal("Jimmy", contact.FirstName);

        }
    }
}
