using Application.Services;
using AutoFixture.Xunit2;
using Domain.DTOs;
using Domain.DTOs.Requests;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace HopShop.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IJsonPlaceholderClient> _jsonPlaceholderClientMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _jsonPlaceholderClientMock = new Mock<IJsonPlaceholderClient>();
            _userService = new UserService(_jsonPlaceholderClientMock.Object);
        }

        [Theory]
        [AutoData]
        public async Task GetAllUsers_WithUsersInExternalClient_ReturnsListOfDTOs(List<GetUser> users)
        {
            //ARRANGE 
            _jsonPlaceholderClientMock.Setup(i => i.GetUsers()).ReturnsAsync(users);

            //ACT
            var result = await _userService.GetUsers();

            //ASSERT
            result.Should().NotBeNull();
            result.Should().HaveCount(users.Count);
        }

        [Fact]
        public async Task GetAllUsers_WithNoUsersInExternalClient_ReturnsEmptyList()
        {
            //ARRANGE
            _jsonPlaceholderClientMock.Setup(i => i.GetUsers()).ReturnsAsync(new List<GetUser>());

            //ACT
            var result = await _userService.GetUsers();

            //ASSERT
            result.Should().BeEmpty();
            result.Should().NotBeNull();
        }

        [Theory]
        [AutoData]
        public async Task GetUserById_GivenValidId_ReturnsDTO(int id, GetUser data, string errorMessage)
        {
            //ARRANGE
            JsonPlaceholderResult<GetUser> success = new JsonPlaceholderResult<GetUser>
            {
                Data = data,
                IsSuccessful = true,
                ErrorMessage = errorMessage
            };
            _jsonPlaceholderClientMock.Setup(i => i.GetUserById(id)).ReturnsAsync(success);

            //ACT
            var result = await _userService.GetUserById(id);

            //ASSERT
            result.Should().Be(success.Data);
        }

        [Theory]
        [AutoData]
        public async Task GetUserById_GivenInvalidId_ThrowsUserNotFoundException(int id)
        {
            //ARRANGE
            _jsonPlaceholderClientMock.Setup(i => i.GetUserById(id)).ReturnsAsync(new JsonPlaceholderResult<GetUser> { IsSuccessful = false});

            //ACT
            var result = async () => await _userService.GetUserById(id);

            //ASSERT
            await result.Should().ThrowAsync<UserNotFoundException>();
        }
    }
}
