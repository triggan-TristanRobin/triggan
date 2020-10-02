using NUnit.Framework;
using System.Collections.Generic;
using Model;
using System.Linq;
using DataAccessLayer.tests;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace triggan.DataAccessLayer.Tests
{
	[TestFixture]
	public class DefaultRepositoryTests
    {
		public class TestClass : Entity
        {

        }

        [Test]
		public void GetPostShouldCallRightMethod()
		{
			// Arrange
			var testObject = new TestClass();

			var context = new Mock<DbContext>();
			var dbSetMock = new Mock<DbSet<TestClass>>();

			context.Setup(x => x.Set<TestClass>()).Returns(dbSetMock.Object);
			dbSetMock.Setup(x => x.Find(It.IsAny<string>())).Returns(testObject);

			// Act
			var repository = new Repository<TestClass>(context.Object);
			repository.Get("");

			// Assert
			context.Verify(x => x.Set<TestClass>());
			dbSetMock.Verify(x => x.Find(It.IsAny<string>()));
		}
	}
}
