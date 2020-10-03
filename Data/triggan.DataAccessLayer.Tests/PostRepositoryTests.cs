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
	public class PostRepositoryTests
	{
		[Test]
		public void GetPostWithSlugShouldCallRightMethod()
		{
			// Arrange
			var testObject = new Post();

			var context = new Mock<TrigganDBContext>();
			var dbSetMock = new Mock<DbSet<Post>>();

			context.Setup(x => x.Set<Post>()).Returns(dbSetMock.Object);
			//dbSetMock.Setup(x => x.FirstOrDefault(It.IsAny<Func<Post, bool>>)).Returns(testObject);

			// Act
			var repository = new PostRepository(context.Object);
			repository.Get("slug");

			// Assert
			context.Verify(x => x.Set<Post>());
			dbSetMock.Verify(x => x.Find(It.IsAny<string>()));
		}
	}
}
