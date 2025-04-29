namespace Basket.Application.Interfaces;

public record ProductDto(int Id, string Name, decimal Price, int Size, int Stars);

public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);

public interface IProductClient
{
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ProductDto>> GetTopAsync(int top, CancellationToken ct = default);
    Task<IReadOnlyList<ProductDto>> GetCheapestAsync(int count, CancellationToken ct = default);
    Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
}
