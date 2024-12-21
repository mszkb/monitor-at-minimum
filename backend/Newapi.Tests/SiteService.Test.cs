using System.Collections.Generic;
using Mam.Models;
using Moq;
using Newapi.Repositories;
using Newapi.Service;
using Xunit;

public class SiteServiceTests
{
    private readonly Mock<ISiteRepository> _siteRepositoryMock;
    private readonly SiteService _siteService;

    public SiteServiceTests()
    {
        _siteRepositoryMock = new Mock<ISiteRepository>();
        _siteService = new SiteService(_siteRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllSites_ReturnsListOfSites()
    {
        // Arrange
        var sites = new List<Site> { new() { Id = 1, Url = "Site1" }, new() { Id = 2, Url = "Site2" } };
        _siteRepositoryMock.Setup(repo => repo.GetAllSitesAsync()).Returns(Task.FromResult<IEnumerable<Site>>(sites));

        // Act
        var result = await _siteService.GetAllSitesAsync();

        // Assert
        Assert.Equal(sites, result.ToList());
    }

    [Fact]
    public async Task GetSiteById_ReturnsSite()
    {
        // Arrange
        var site = new Site { Id = 1, Url = "Site1" };
        _siteRepositoryMock.Setup(repo => repo.GetSiteByIdAsync(1)).Returns(Task.FromResult(site));

        // Act
        var result = await _siteService.GetSiteByIdAsync(1);

        // Assert
        Assert.Equal(site, result);
    }

    [Fact]
    public async Task GetSiteById_ReturnsNull_WhenSiteNotFound()
    {
        // Arrange
        _ = _siteRepositoryMock.Setup(repo => repo.GetSiteByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<Site>(null));

        // Act
        var result = await _siteService.GetSiteByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddSite_CallsRepositoryAdd()
    {
        // Arrange
        var site = new Site { Id = 1, Url = "Site1" };

        // Act
        await _siteService.CreateSiteAsync(site);

        // Assert
        _siteRepositoryMock.Verify(repo => repo.AddSiteAsync(site), Times.Once);
    }

    [Fact]
    public async Task UpdateSite_CallsRepositoryUpdate()
    {
        // Arrange
        var site = new Site { Id = 1, Url = "Site1" };

        // Act
        await _siteService.UpdateSiteAsync(site);

        // Assert
        _siteRepositoryMock.Verify(repo => repo.UpdateSiteAsync(site), Times.Once);
    }

    [Fact]
    public async Task DeleteSite_CallsRepositoryDelete()
    {
        // Arrange
        var siteId = 1;

        // Act
        await _siteService.DeleteSiteAsync(siteId);

        // Assert
        _siteRepositoryMock.Verify(repo => repo.DeleteSiteAsync(siteId), Times.Once);
    }
}