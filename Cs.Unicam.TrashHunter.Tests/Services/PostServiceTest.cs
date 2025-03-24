using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using global::Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using global::Cs.Unicam.TrashHunter.Models.Entities;
using global::Cs.Unicam.TrashHunter.Models.Repositorys.Models;
using global::Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using global::Cs.Unicam.TrashHunter.Services.DTOs;
using global::Cs.Unicam.TrashHunter.Services.Services.Filters;
using global::Cs.Unicam.TrashHunter.Services.Services;
using Moq;
using Xunit;


namespace Cs.Unicam.TrashHunter.Tests.Services
{
    // Dummy implementation of IFilter that always returns true.
    public class DummyFilter : IFilter
    {
        public Expression<Func<T, bool>> GetFilter<T>() => x => true;
        public IFilter And(IFilter filter) => this;
        public IFilter Or(IFilter filter) => this;        
    }

    public class PostServiceTests
    {
        private readonly User _fakeUser = new User("TestName", "TestSurname", "TestEmail", "TestPassword", "TestSalt", Role.Admin, "TestCity");
        private readonly Mock<IRepository<Post>> _postRepositoryMock;
        private readonly Mock<IAttachmentService> _attachmentServiceMock;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _postRepositoryMock = new Mock<IRepository<Post>>();
            _attachmentServiceMock = new Mock<IAttachmentService>();
            _postService = new PostService(_postRepositoryMock.Object, _attachmentServiceMock.Object);
        }

        private Post CreateTestPost(int id = 1)
        {

            var post =  new Post("Test Title", "Test Description", "Test City", _fakeUser, new List<Attachment>());
            post.GetType().GetProperty("PostId")?.SetValue(post, id);
            return post;
        }

           

        private PostDTO CreateTestPostDTO(int id = 1)
        {
            var post = CreateTestPost(id);
            // Assuming PostDTO constructor takes Post and a list of AttachmentDTO (can be null)
            return new PostDTO(post, new List<AttachmentDTO>());
        }

        [Fact]
        public async Task CreatePostAsync_ShouldAddAndSavePost_AndReturnPostDTO()
        {
            // Arrange
            var postDTO = CreateTestPostDTO();
            var post = postDTO.ToEntity();
            _postRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            // Quando viene chiamato Add, non è necessario configurare un ritorno.
            // Simula che dopo il salvataggio Find restituisce il post inserito.
            _postRepositoryMock.Setup(repo => repo.Find(It.IsAny<object>())).ReturnsAsync(post);
            // Nessun attachments in questo test, quindi GetAttachment non viene invocato.

            // Act
            var result = await _postService.CreatePostAsync(postDTO);

            // Assert
            _postRepositoryMock.Verify(repo => repo.Add(It.IsAny<Post>()), Times.Once);
            _postRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            result.Should().NotBeNull();
            result.Title.Should().Be(postDTO.Title);
        }

        [Fact]
        public async Task DeletePostAsync_ShouldDeletePost_AndReturnPostDTO()
        {
            // Arrange
            var post = CreateTestPost();
            // Aggiungiamo una attachment fittizio
            post.Attachments.Add(new Attachment("file.txt", "desc", "path/to/file.txt", post.PostId));
            _postRepositoryMock.Setup(repo => repo.Find(It.IsAny<object>())).ReturnsAsync(post);
            _postRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            _attachmentServiceMock.Setup(service => service.DeleteRange(It.IsAny<string[]>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _postService.DeletePostAsync(post.PostId);

            // Assert
            _postRepositoryMock.Verify(repo => repo.Delete(It.Is<Post>(p => p.PostId == post.PostId)), Times.Once);
            _attachmentServiceMock.Verify(service => service.DeleteRange(It.Is<string[]>(files => files.Contains("file.txt"))), Times.Once);
            _postRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            result.Should().NotBeNull();
            result.PostId.Should().Be(post.PostId);
        }

        [Fact]
        public async Task GetPostAsync_ShouldReturnPostDTO_WhenPostExists()
        {
            // Arrange
            var post = CreateTestPost();
            _postRepositoryMock.Setup(repo => repo.Find(It.IsAny<object>())).ReturnsAsync(post);

            // Act
            var result = await _postService.GetPostAsync(post.PostId);

            // Assert
            result.Should().NotBeNull();
            result.PostId.Should().Be(post.PostId);
        }

        [Fact]
        public async Task UpdatePostAsync_ShouldUpdatePost_AndReturnUpdatedPostDTO()
        {
            // Arrange
            var postDTO = CreateTestPostDTO();
            var post = CreateTestPost();
            // Simula che il post esiste
            _postRepositoryMock.Setup(repo => repo.Exist(postDTO.PostId)).ReturnsAsync(true);
            _postRepositoryMock.Setup(repo => repo.Save()).Returns(Task.CompletedTask);
            _postRepositoryMock.Setup(repo => repo.Find(postDTO.PostId)).ReturnsAsync(post);

            // Act
            var result = await _postService.UpdatePostAsync(postDTO);

            // Assert
            _postRepositoryMock.Verify(repo => repo.Update(It.Is<Post>(p => p.PostId == postDTO.PostId)), Times.Once);
            _postRepositoryMock.Verify(repo => repo.Save(), Times.Once);
            result.Should().NotBeNull();
            result.PostId.Should().Be(postDTO.PostId);
        }

        [Fact]
        public async Task GetPostsAsync_ShouldReturnPagedResults()
        {
            // Arrange
            var posts = new List<Post>
            {
                CreateTestPost(1),
                CreateTestPost(2)
            };

            var dummyPagingResult = new PagingResult<Post>
            {
                Items = posts,
                Page = 0,
                PageSize = 10,
                TotalItems = posts.Count,
                TotalPages = 1
            };

            _postRepositoryMock.Setup(repo => repo.GetFiltered(It.IsAny<IFilter>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(dummyPagingResult);

            var postFilters = new PostFilters("Test", null, null, null, null);
            Func<IFilter> filterFunc = () => new DummyFilter();
            var filtersField = typeof(PostFilters).GetField("_title", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            // Act
            var result = await _postService.GetPostsAsync(postFilters, 1, 10);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(posts.Count);
        }
    }
}

