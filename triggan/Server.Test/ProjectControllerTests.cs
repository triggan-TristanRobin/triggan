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
    public class ProjectControllerTests
    {
        readonly ProjectController controller;
        readonly Mock<ISlugRepository<Project>> repositoryMock;

        public ProjectControllerTests()
        {
            repositoryMock = new Mock<ISlugRepository<Project>>();
            controller = new ProjectController(repositoryMock.Object);
        }

        [Test]
        public void GetShouldReturnAll()
        {
            //Arrange
            repositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Project> { new Project(), new Project(), new Project() });

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsInstanceOf(typeof(IEnumerable<Project>), result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void GetWithSlugShouldReturnSingleRelatedProject()
        {
            //Arrange
            var expected = new Project()
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