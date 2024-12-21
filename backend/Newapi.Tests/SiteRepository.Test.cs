using Moq;
using Microsoft.EntityFrameworkCore;
using Newapi.Repositories;
using Mam.Models;

namespace Newapi.Tests
{
    public class SiteRepositoryTests
    {
        private Mock<ApplicationDbContext> GetMockedDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var mockContext = new Mock<ApplicationDbContext>(options);
            return mockContext;
        }

        [Fact]
        public async Task AddSite_ShouldAddSite()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockSet = new Mock<DbSet<Site>>();
            mockContext.Setup(m => m.Sites).Returns(mockSet.Object);

            var repository = new SiteRepository(mockContext.Object);
            var site = new Site { Id = 1, Url = "https://www.google.com", Status = "up", LastChecked = 1734726242437, Ping = 97, Type = "PING" };

            // Act
            await repository.AddSiteAsync(site);

            // Assert
            mockSet.Verify(m => m.Add(site), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task GetSite_ShouldReturnSite()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockSet = new Mock<DbSet<Site>>();
            var site = new Site { Id = 1, Url = "https://www.google.com", Status = "up", LastChecked = 1734726242437, Ping = 97, Type = "PING" };

            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(site);
            mockContext.Setup(m => m.Sites).Returns(mockSet.Object);

            var repository = new SiteRepository(mockContext.Object);

            // Act
            var result = await repository.GetSiteByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(site.Url, result.Url);
        }

        [Fact]
        public async Task DeleteSite_ShouldRemoveSite()
        {
            // Arrange
            var mockContext = GetMockedDbContext();
            var mockSet = new Mock<DbSet<Site>>();
            var site = new Site { Id = 1, Url = "https://www.google.com", Status = "up", LastChecked = 1734726242437, Ping = 97, Type = "PING" };

            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(site);
            mockSet.Setup(m => m.Remove(site)).Verifiable();
            mockContext.Setup(m => m.Sites).Returns(mockSet.Object);

            var repository = new SiteRepository(mockContext.Object);

            // Act
            await repository.DeleteSiteAsync(1);

            // Assert
            mockSet.Verify(m => m.Remove(site), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }
    }
}