using AllGoals.Application.DTOs;
using AllGoals.Application.Services;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;
using AllGoals.Domain.ValueObjects;
using Moq;
using Xunit;

namespace AllGoals.Tests.Tests;

public class UserTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly UserService _service;
    private User _capturedUser;

    public UserTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _service = new UserService(_mockRepo.Object);
        _capturedUser = null; 
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUserDtoWithLinks_WhenUserExists()
    {
        var emailVo = new Email("jenni@example.com");
        var userEntity = new User("Jenni Teste", emailVo);

        typeof(User).GetProperty("Id")?.SetValue(userEntity, 10);

        _mockRepo.Setup(r => r.GetByIdAsync(10))
            .ReturnsAsync(userEntity);

        var result = await _service.GetByIdAsync(10);

        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal("Jenni Teste", result.Nome);
        Assert.Equal("jenni@example.com", result.Email);
        Assert.False(result.IsAdmin);

        Assert.NotNull(result.Links);
        Assert.Equal(3, result.Links.Count);
        Assert.Contains(result.Links, l => l.Rel == "update");
        Assert.DoesNotContain(result.Links, l => l.Rel == "promote");
    }

    [Fact]
    public async Task CreateAsync_ShouldCallRepository_AndReturnDtoWithCorrectEmail()
    {
        // 1. Arrange
        var requestDto = new UserDtoRequest
        {
            Nome = "Novo Usuário",
            Email = "novo@email.com"
        };

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(entity =>
            {
                _capturedUser = entity;
                typeof(User).GetProperty("Id")?.SetValue(entity, 5);
            });

        var result = await _service.CreateAsync(requestDto);

        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
        Assert.Equal("Novo Usuário", result.Nome);

        Assert.NotNull(_capturedUser);
        Assert.Equal("Novo Usuário", _capturedUser.Nome);
        Assert.Equal("novo@email.com", _capturedUser.Email.Value);
        Assert.False(_capturedUser.IsAdmin);
    }
}