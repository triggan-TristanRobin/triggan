using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Tests
{
	[TestFixture]
	public class RepositoryTests
	{
		class TestClass : Entity { }

		[Test]
		public void GetPostWithSlugShouldCallRightMethod()
		{
			// Arrange
			var testObject = new TestClass();

			var context = new Mock<TrigganContext>();
			var dbSetMock = new Mock<DbSet<TestClass>>();

			context.Setup(x => x.Set<TestClass>()).Returns(dbSetMock.Object);
			dbSetMock.Setup(x => x.FirstOrDefault()).Verifiable();

			// Act
			var repository = new Repository<TestClass>(context.Object);
			repository.Get("slug");

			// Assert
			context.Verify(x => x.Set<TestClass>());
			dbSetMock.Verify();
		}
	}
}
