using NUnit.Framework;
using System.Collections.Generic;
using Model;
using System.Linq;
using triggan.Interfaces;
using triggan.Server.Controllers;
using Moq;

namespace triggan.Server.test
{
    [TestFixture]
    public class PostControllerTests
    {
        readonly PostController controller;
        readonly Mock<IRepository<Post>> repositoryMock;

        public PostControllerTests()
        {
            repositoryMock = new Mock<IRepository<Post>>();
            controller = new PostController(repositoryMock.Object);
        }

        [Test]
        public void GetShouldReturnAll()
        {
            //Arrange
            repositoryMock.Setup(repo => repo.Get()).Returns(new List<Post> { new Post(), new Post(), new Post() });

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Post>), result);
            Assert.AreEqual(3, result.Count());
        }
    }
}