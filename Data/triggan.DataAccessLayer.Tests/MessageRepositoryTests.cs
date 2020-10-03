using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Tests
{
	[TestFixture]
	public class MessageRepositoryTests
	{
		[Test]
		public void GetMessageShouldCallRightMethod()
		{
			// Arrange
			var testObject = new Message();
			var testList = new List<Message>() { testObject };

			var context = new Mock<TrigganDBContext>();
			var dbSetMock = new Mock<DbSet<Message>>();

			context.Setup(x => x.Set<Message>()).Returns(dbSetMock.Object);
			dbSetMock.As<IQueryable<Message>>().Setup(x => x.Provider).Returns(testList.AsQueryable().Provider);
			dbSetMock.As<IQueryable<Message>>().Setup(x => x.Expression).Returns(testList.AsQueryable().Expression);
			dbSetMock.As<IQueryable<Message>>().Setup(x => x.ElementType).Returns(testList.AsQueryable().ElementType);
			dbSetMock.As<IQueryable<Message>>().Setup(x => x.GetEnumerator()).Returns(testList.AsQueryable().GetEnumerator());

			// Act
			var repository = new MessageRepository(context.Object);
			var result = repository.Get();

			// Assert
			Assert.AreEqual(testList, result.ToList());
		}

		[Test]
		public void InsertMessageShouldCallRightMethod()
		{
			// Arrange
			var testObject = new Message();

			var context = new Mock<TrigganDBContext>();
			var dbSetMock = new Mock<DbSet<Message>>();

			context.Setup(x => x.Set<Message>()).Returns(dbSetMock.Object);
			dbSetMock.Setup(x => x.Add(It.IsAny<Message>())).Verifiable();

			// Act
			var repository = new MessageRepository(context.Object);
			repository.Insert(testObject);

			// Assert
			context.Verify(x => x.Set<Message>());
			dbSetMock.Verify(x => x.Add(It.IsAny<Message>()));
		}
	}
}
