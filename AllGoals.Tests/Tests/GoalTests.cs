using AllGoals.Application.DTOs;
using AllGoals.Application.Services;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;
using Moq;
using Xunit;

namespace AllGoals.Tests.Tests;

public class GoalTests
{
    private readonly Mock<IGoalRepository> _mockRepo;
    private readonly GoalService _service;

    public GoalTests()
    {
        _mockRepo = new Mock<IGoalRepository>();
        _service = new GoalService(_mockRepo.Object); // Injeta o mock
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnGoalDtoWithIdAndLinks_WhenCalled()
    {
        var requestDto = new GoalDtoRequest
        {
            Titulo = "Meu Teste de Unidade",
            Descricao = "Testando o CreateAsync",
            Xp = 100,
            Moedas = 50
        };

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Goal>()))
            .Callback<Goal>(entity => 
            {
                var propertyInfo = typeof(Goal).GetProperty("Id");
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(entity, 1);
                }
            });

        var result = await _service.CreateAsync(requestDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Meu Teste de Unidade", result.Titulo);
        Assert.Equal(100, result.Xp);

        Assert.NotEmpty(result.Links); 
        Assert.Equal("self", result.Links[0].Rel);
        Assert.Equal("/api/goals/1", result.Links[0].Href);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenGoalDoesNotExist()
    {

        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Goal)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }
}