using Application.Dto.Post;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Services
{
    public class PostServiceTests
    {
        [Fact]
        public async Task add_post_async_should_invoke_add_async_on_post_repository()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PostService>>();

            PostService postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

            var postDto = new CreatePostDto()
            {
                Title = "Title1",
                Content = "Content 1"
            };

            mapperMock.Setup(x => x.Map<Post>(postDto)).Returns(new Post()
            {
                Title = postDto.Title,
                Content = postDto.Content
            });

            //Act
            await postService.AddNewPostAsync(postDto, "14fcc71f-76f4-4f5b-ae11-6ee74b3fe493");

            //Assert
            postRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task when_invoking_get_post_async_it_should_invoke_get_async_on_post_repository()
        {
            //Arrange
            var postRepositoryMock = new Mock<IPostRepository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PostService>>();

            PostService postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

            var post = new Post(1, "Title", "Content 1");
            var postDto = new PostDto()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };

            mapperMock.Setup(x => x.Map<Post>(postDto)).Returns(post);
            postRepositoryMock.Setup(x => x.GetByIdAsync(post.Id)).ReturnsAsync(post);

            //Act
            var existingPostDto = await postService.GetPostByIdAsync(post.Id);

            //Assert
            postRepositoryMock.Verify(x => x.GetByIdAsync(post.Id), Times.Once);
            postDto.Should().NotBeNull();
            postDto.Title.Should().NotBeNull();
            postDto.Title.Should().BeEquivalentTo(post.Title);
            postDto.Content.Should().NotBeNull();
            postDto.Content.Should().BeEquivalentTo(post.Content);
        }
    }
}