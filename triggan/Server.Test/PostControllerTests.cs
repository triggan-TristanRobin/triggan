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
        readonly Mock<ISlugRepository<Post>> repositoryMock;

        public PostControllerTests()
        {
            repositoryMock = new Mock<ISlugRepository<Post>>();
            controller = new PostController(repositoryMock.Object);
        }

        [Test]
        public void GetShouldReturnAll()
        {
            //Arrange
            repositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Post> { new Post(), new Post(), new Post() });

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Post>), result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void GetWithSlugShouldReturnSingleRelatedPost()
        {
            //Arrange
            var expected = new Post()
            {
                Title = "Fake",
                Slug = "Slug",
            };
            repositoryMock.Setup(repo => repo.Get("Slug")).Returns(expected);

            // Act
            var result = controller.Get("Slug");

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}