using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Cs.Unicam.TrashHunter.Services.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cs.Unicam.TrashHunter.Tests.Services
{
    public class CompanyServiceTests
    {
        private readonly Mock<IRepository<Company>> _companyRepositoryMock;
        private readonly Mock<IAttachmentService> _attachmentServiceMock;
        private readonly CompanyService _companyService;

        public CompanyServiceTests()
        {
            _companyRepositoryMock = new Mock<IRepository<Company>>();
            _attachmentServiceMock = new Mock<IAttachmentService>();
            _companyService = new CompanyService(_companyRepositoryMock.Object, _attachmentServiceMock.Object);
        }

        [Fact]
        public async Task AddCompany_ShouldAddCompanyAndSave()
        {
            // Arrange
            var companyDTO = new CompanyDTO
            {
                CompanyCode = "COMP123",
                Name = "Test Company",
                Description = "Test Description",
                Logo = new AttachmentDTO(new byte[] { 1, 2, 3, 4 }, "logo.png", "Logo", "aa/a", null, "COMP123", null),
                AdviceImage = new AttachmentDTO(new byte[] { 5, 6, 7, 8 }, "advice.png", "Advice Image", "aa", null, "COMP123", null)
            };

            _attachmentServiceMock.Setup(service => service.Attach(companyDTO.Logo, companyDTO.CompanyCode, true))
                .ReturnsAsync(companyDTO.Logo);
            _attachmentServiceMock.Setup(service => service.Attach(companyDTO.AdviceImage, companyDTO.CompanyCode, false))
                .ReturnsAsync(companyDTO.AdviceImage);

            // Act
            await _companyService.AddCompany(companyDTO);

            // Assert
            _companyRepositoryMock.Verify(repo => repo.Add(It.IsAny<Company>()), Times.Once);
            _companyRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteCompany_ShouldDeleteCompanyAndAttachments()
        {
            // Arrange
            var company = new Company("COMP123", "Test Company", "Test Description", new Attachment("logo.png", "Logo", "path/to/logo.png", "COMP123", true), new Attachment("advice.png", "Advice Image", "path/to/advice.png", "COMP123", false));
            _companyRepositoryMock.Setup(repo => repo.Find("COMP123")).ReturnsAsync(company);

            // Act
            var result = await _companyService.DeleteCompany("COMP123");

            // Assert
            result.Should().BeTrue();
            _attachmentServiceMock.Verify(service => service.DeleteAttachment(company.Logo.FileName), Times.Once);
            _attachmentServiceMock.Verify(service => service.DeleteAttachment(company.AdviceImage.FileName), Times.Once);
            _companyRepositoryMock.Verify(repo => repo.Delete(company), Times.Once);
            _companyRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task EditCompany_ShouldUpdateCompanyAndSave()
        {
            // Arrange
            var companyDTO = new CompanyDTO
            {
                CompanyCode = "COMP123",
                Name = "Updated Company",
                Description = "Updated Description",
                Logo = new AttachmentDTO(new byte[] { 1, 2, 3, 4 }, "logo.png", "Logo", "a", null, "COMP123", null),
                AdviceImage = new AttachmentDTO(new byte[] { 5, 6, 7, 8 }, "advice.png", "Advice Image", "a", null, "COMP123", null)
            };

            var company = new Company("COMP123", "Test Company", "Test Description", new Attachment("logo.png", "Logo", "path/to/logo.png", "COMP123", true), new Attachment("advice.png", "Advice Image", "path/to/advice.png", "COMP123", false));
            _companyRepositoryMock.Setup(repo => repo.Find("COMP123")).ReturnsAsync(company);

            _attachmentServiceMock.Setup(service => service.Attach(companyDTO.Logo, companyDTO.CompanyCode, true))
                .ReturnsAsync(companyDTO.Logo);
            _attachmentServiceMock.Setup(service => service.Attach(companyDTO.AdviceImage, companyDTO.CompanyCode, false))
                .ReturnsAsync(companyDTO.AdviceImage);

            // Act
            var result = await _companyService.EditCompany(companyDTO);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(companyDTO.Name);
            result.Description.Should().Be(companyDTO.Description);
            _companyRepositoryMock.Verify(repo => repo.Update(It.IsAny<Company>()), Times.Once);
            _companyRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task GetCompanies_ShouldReturnCompanyDTOs()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company("COMP123", "Test Company", "Test Description", new Attachment("logo.png", "Logo", "path/to/logo.png", "COMP123", true), new Attachment("advice.png", "Advice Image", "path/to/advice.png", "COMP123", false))
            };
            _companyRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(companies);

            _attachmentServiceMock.Setup(service => service.GetAttachment(It.IsAny<string>()))
                .ReturnsAsync(new AttachmentDTO(new byte[] { 1, 2, 3, 4 }, "logo.png", "Logo", "path/to/logo.png", null, "COMP123", null));

            // Act
            var result = await _companyService.GetCompanies();

            // Assert
            result.Should().HaveCount(1);
            result.First().CompanyCode.Should().Be("COMP123");
        }

        [Fact]
        public async Task GetCompany_ShouldReturnCompanyDTO()
        {
            // Arrange
            var company = new Company("COMP123", "Test Company", "Test Description", new Attachment("logo.png", "Logo", "path/to/logo.png", "COMP123", true), new Attachment("advice.png", "Advice Image", "path/to/advice.png", "COMP123", false));
            _companyRepositoryMock.Setup(repo => repo.Find("COMP123")).ReturnsAsync(company);

            _attachmentServiceMock.Setup(service => service.GetAttachment(company.Logo.FileName))
                .ReturnsAsync(new AttachmentDTO(new byte[] { 1, 2, 3, 4 }, "logo.png", "Logo", "path/to/logo.png", null, "COMP123", null));
            _attachmentServiceMock.Setup(service => service.GetAttachment(company.AdviceImage.FileName))
                .ReturnsAsync(new AttachmentDTO(new byte[] { 5, 6, 7, 8 }, "advice.png", "Advice Image", "path/to/advice.png", null, "COMP123", null));

            // Act
            var result = await _companyService.GetCompany("COMP123");

            // Assert
            result.Should().NotBeNull();
            result.CompanyCode.Should().Be("COMP123");
        }
    }
}

