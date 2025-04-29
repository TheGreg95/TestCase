using Microsoft.Extensions.Caching.Memory;
using Basket.Application.Interfaces;

namespace Basket.Infrastructure;

public class ProductClient : IProductClient
{
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;

    public ProductClient(HttpClient http, IMemoryCache cache)
        => (_http, _cache) = (http, cache);

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<ProductDto>($"products/{id}", ct);

    public async Task<IReadOnlyList<ProductDto>> GetTopAsync(int top, CancellationToken ct = default)
        => (await GetAll(ct)).OrderBy(p => p.Stars).Take(top).ToList();

    public async Task<IReadOnlyList<ProductDto>> GetCheapestAsync(int n, CancellationToken ct = default)
        => (await GetAll(ct)).OrderBy(p => p.Price).Take(n).ToList();

    public async Task<PagedResult<ProductDto>> GetPagedAsync(int page, int size, CancellationToken ct = default)
    {
        var all = await GetAll(ct);
        var slice = all.OrderBy(p => p.Price)
                       .Skip((page - 1) * size)
                       .Take(size)
                       .ToList();
        return new PagedResult<ProductDto>(slice, all.Count, page, size);
    }

    private Task<List<ProductDto>> GetAll(CancellationToken ct) =>
        _cache.GetOrCreateAsync("allProducts",
            _ => _http.GetFromJsonAsync<List<ProductDto>>("products", ct))!;
}
