using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.DB;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Cs.Unicam.TrashHunter.Services.Options;
using Cs.Unicam.TrashHunter.Services.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cs.Unicam.TrashHunter.Tests.Services
{
    public class AttachmentServiceTests
    {
        private readonly Mock<IRepository<Attachment>> _attachmentRepositoryMock;
        private readonly Mock<IRepository<Post>> _postRepositoryMock;
        private readonly AttachmentOptions _options;
        private readonly AttachmentService _attachmentService;

        public AttachmentServiceTests()
        {
            _attachmentRepositoryMock = new Mock<IRepository<Attachment>>();
            _postRepositoryMock = new Mock<IRepository<Post>>();
            _options = new AttachmentOptions("Services/Attachments", 10485760, new FileType[] { });
            _attachmentService = new AttachmentService(_attachmentRepositoryMock.Object, _postRepositoryMock.Object, _options);
        }

        [Fact]
        public async Task Attach_ShouldSaveFileAndReturnAttachmentDTO()
        {
            // Arrange
            var attachmentDTO = new AttachmentDTO(new byte[] { 1,2,3,4}, "file.txt", "description", null, 1, null, null);
            var postId = 1;

            // Act
            var result = await _attachmentService.Attach(attachmentDTO, postId);

            // Assert
            result.FileName.Should().Be(attachmentDTO.FileName);
            result.Description.Should().Be(attachmentDTO.Description);
            result.Path.Should().NotBeNull().And.Be(Path.Combine(_options.RootPath, attachmentDTO.FileName));
            result.File.Should().BeEquivalentTo(attachmentDTO.File);
        }

        [Fact]
        public async Task DeleteAttachment_ShouldDeleteFileAndAttachment()
        {
            // Arrange
            var fileName = "test.txt";
            var attachment = new Attachment(fileName, "Test file", "testPath/test.txt", 1);
            _attachmentRepositoryMock.Setup(repo => repo.Find(fileName)).ReturnsAsync(attachment);

            // Act
            await _attachmentService.DeleteAttachment(fileName);

            // Assert
            _attachmentRepositoryMock.Verify(repo => repo.Delete(attachment), Times.Once);
            _attachmentRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            File.Exists(attachment.Path).Should().BeFalse();
        }

        [Fact]
        public async Task GetAttachment_ShouldReturnAttachmentDTO()
        {
            // Arrange
            var fileName = "test.txt";
            await File.WriteAllBytesAsync(Path.Combine(_options.RootPath, fileName), [1, 2, 3, 4]);
            var attachment = new Attachment(fileName, "Test file", Path.Combine(_options.RootPath, fileName), 1);
            _attachmentRepositoryMock.Setup(repo => repo.Find(fileName)).ReturnsAsync(attachment);

            // Act
            var result = await _attachmentService.GetAttachment(fileName);

            // Assert
            result.FileName.Should().Be(fileName);
            result.Description.Should().Be(attachment.Description);
            result.Path.Should().Be(attachment.Path);
        }

        [Fact]
        public async Task GetLazyAttachments_ShouldReturnLazyAttachmentDTOs()
        {
            // Arrange
            var postId = 1;
            var post = new Post("Fake", "post", "a", "1", new List<Attachment>
            {
                new Attachment("file.txt", "test file 1", "a", 1),
                new Attachment("file.pdf", "test file 2", "a", 1)
            });
            _postRepositoryMock.Setup(repo => repo.Find(postId)).ReturnsAsync(post);

            // Act
            var result = await _attachmentService.GetLazyAttachments(postId);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateAttachment_ShouldUpdateDescription()
        {
            // Arrange
            var fileName = "test.txt";
            var newDescription = "Updated description";
            var attachment = new Attachment(fileName, "Test file", Path.Combine(_options.RootPath, fileName), 1);
            _attachmentRepositoryMock.Setup(repo => repo.Find(fileName)).ReturnsAsync(attachment);

            // Act
            var result = await _attachmentService.UpdateAttachment(fileName, newDescription);

            // Assert
            result.Description.Should().Be(newDescription);
            _attachmentRepositoryMock.Verify(repo => repo.Update(attachment), Times.Once);
            _attachmentRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
    }
}