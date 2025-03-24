using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Models.Repositorys.Models;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Cs.Unicam.TrashHunter.Services.Services;
using Cs.Unicam.TrashHunter.Services.Services.Filters;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cs.Unicam.TrashHunter.Tests.Services
{
    public class CuponServicesTests
    {
        private readonly Attachment _attachment;
        private readonly Attachment _logo;
        private readonly Mock<IRepository<Cupon>> _cuponRepositoryMock;
        private readonly Mock<IRepository<Company>> _companyRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly CuponServices _cuponServices;

        public CuponServicesTests()
        {
            _cuponRepositoryMock = new Mock<IRepository<Cupon>>();
            _companyRepositoryMock = new Mock<IRepository<Company>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _cuponServices = new CuponServices(_cuponRepositoryMock.Object, _companyRepositoryMock.Object, _userRepositoryMock.Object);
            _attachment = new Attachment("test.png", "Test file", "testPath/test.png", "COMP123", false);
            _logo = new Attachment("logo.png", "Test logo", "testPath/logo.png", "COMP123", true);

        }

        [Fact]
        public async Task AddCupon_ShouldAddCuponAndSave()
        {
            // Arrange
            var companyCode = "COMP123";
            var userCode = "USER123";
            var expirationDate = DateTime.UtcNow.AddDays(30);

            var company = new Company(companyCode, "Test Company", "Description", _logo, _attachment);
            var user = new User("John", "Doe", "john.doe@example.com", "password", "salt", Role.User, "City");

            _companyRepositoryMock.Setup(repo => repo.Find(companyCode)).ReturnsAsync(company);
            _userRepositoryMock.Setup(repo => repo.Find(userCode)).ReturnsAsync(user);

            // Act
            var result = await _cuponServices.AddCupon(companyCode, userCode, expirationDate);

            // Assert
            result.Should().BeTrue();
            _cuponRepositoryMock.Verify(repo => repo.Add(It.IsAny<Cupon>()), Times.Once);
            _cuponRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteCupon_ShouldDeleteCuponAndSave()
        {
            // Arrange
            var cuponCode = Guid.NewGuid();
            var cupon = new Cupon("COMP123", "USER123", DateTime.UtcNow.AddDays(30), DateTime.UtcNow, false);

            _cuponRepositoryMock.Setup(repo => repo.Find(cuponCode)).ReturnsAsync(cupon);

            // Act
            var result = await _cuponServices.DeleteCupon(cuponCode);

            // Assert
            result.Should().BeTrue();
            _cuponRepositoryMock.Verify(repo => repo.Delete(cupon), Times.Once);
            _cuponRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task GetCupons_ShouldReturnCuponDTOs()
        {
            // Arrange
            var userCode = "USER123";
            var cupons = new List<Cupon>
            {
                new Cupon("COMP123", userCode, DateTime.UtcNow.AddDays(30), DateTime.UtcNow, false)
            };

            var pagingResult = new PagingResult<Cupon>
            {
                Items = cupons,
                Page = 0,
                PageSize = 10,
                TotalItems = cupons.Count,
                TotalPages = 1
            };

            _cuponRepositoryMock.Setup(repo => repo.GetFiltered(It.IsAny<IFilter>(), 0, int.MaxValue)).ReturnsAsync(pagingResult);

            // Act
            var result = await _cuponServices.GetCupons(userCode);

            // Assert
            result.Should().HaveCount(1);
            result.First().UserCode.Should().Be(userCode);
        }

        [Fact]
        public async Task GetCuponsForCompany_ShouldReturnCuponDTOs()
        {
            // Arrange
            var companyCode = "COMP123";
            var cupons = new List<Cupon>
            {
                new Cupon(companyCode, "USER123", DateTime.UtcNow.AddDays(30), DateTime.UtcNow, false)
            };

            var pagingResult = new PagingResult<Cupon>
            {
                Items = cupons,
                Page = 0,
                PageSize = 10,
                TotalItems = cupons.Count,
                TotalPages = 1
            };

            _cuponRepositoryMock.Setup(repo => repo.GetFiltered(It.IsAny<IFilter>(), 0, int.MaxValue)).ReturnsAsync(pagingResult);

            // Act
            var result = await _cuponServices.GetCuponsForCompany(companyCode);

            // Assert
            result.Should().HaveCount(1);
            result.First().CompanyCode.Should().Be(companyCode);
        }

        [Fact]
        public async Task GetCupon_ShouldReturnCuponDTO()
        {
            // Arrange
            var cuponCode = Guid.NewGuid();
            var cupon = new Cupon("COMP123", "USER123", DateTime.UtcNow.AddDays(30), DateTime.UtcNow, false);

            _cuponRepositoryMock.Setup(repo => repo.Find(cuponCode)).ReturnsAsync(cupon);

            // Act
            var result = await _cuponServices.GetCupon(cuponCode);

            // Assert
            result.Should().NotBeNull();
            result.CompanyCode.Should().Be("COMP123");
        }

        [Fact]
        public async Task UseCupon_ShouldMarkCuponAsUsedAndSave()
        {
            // Arrange
            var cuponCode = Guid.NewGuid();
            var cupon = new Cupon("COMP123", "USER123", DateTime.UtcNow.AddDays(30), DateTime.UtcNow, false);

            _cuponRepositoryMock.Setup(repo => repo.Find(cuponCode)).ReturnsAsync(cupon);

            // Act
            var result = await _cuponServices.UseCupon(cuponCode);

            // Assert
            result.Should().BeTrue();
            cupon.IsUsed.Should().BeTrue();
            _cuponRepositoryMock.Verify(repo => repo.Update(cupon), Times.Once);
            _cuponRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
    }
}
