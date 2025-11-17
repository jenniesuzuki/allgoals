using AllGoals.Application.DTOs;
using AllGoals.Application.Services;
using AllGoals.Domain.Entities;
using AllGoals.Domain.Interfaces;
using Moq;

namespace AllGoals.Tests.Tests;

public class StoreItemTests
{
    private readonly Mock<IStoreItemRepository> _mockRepo;
    private readonly StoreItemService _service;

    public StoreItemTests()
    {
        _mockRepo = new Mock<IStoreItemRepository>();
        _service = new StoreItemService(_mockRepo.Object); 
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnDtoWithIdAndLinks()
    {
        var requestDto = new StoreItemDtoRequest
        {
            Nome = "Item Teste",
            Descricao = "Um item de teste",
            Valor = 100
        };

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<StoreItem>()))
            .Callback<StoreItem>(entity => 
            {
                typeof(StoreItem).GetProperty("Id")?.SetValue(entity, 42); 
            });

        var result = await _service.CreateAsync(requestDto);

        Assert.NotNull(result);
        Assert.Equal(42, result.Id);
        Assert.Equal("Item Teste", result.Nome);
        Assert.Equal(100, result.Valor);

        Assert.NotEmpty(result.Links); 
        Assert.Equal(3, result.Links.Count);
        Assert.Contains(result.Links, l => l.Rel == "delete");
        Assert.Contains(result.Links, l => l.Href == "/api/storeitems/42");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnItemDto_WhenItemExists()
    {
        var storeItemEntity = new StoreItem("Item Falso", "Desc", 10);
        typeof(StoreItem).GetProperty("Id")?.SetValue(storeItemEntity, 7);

        _mockRepo.Setup(r => r.GetByIdAsync(7))
            .ReturnsAsync(storeItemEntity);

        var result = await _service.GetByIdAsync(7);

        Assert.NotNull(result);
        Assert.Equal(7, result.Id);
        Assert.Equal("Item Falso", result.Nome);

        Assert.Equal(3, result.Links.Count);
        Assert.Equal("self", result.Links[0].Rel);
    }
}