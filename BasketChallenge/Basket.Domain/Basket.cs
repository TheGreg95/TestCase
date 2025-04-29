using System.Collections.Generic;
using System.Linq;

namespace Basket.Domain;

public class Basket
{
    private readonly List<BasketItem> _items = new();
    public Guid Id { get; } = Guid.NewGuid();
    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

    public void Add(int productId, int qty, decimal unitPrice)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        var line = _items.FirstOrDefault(i => i.ProductId == productId);
        if (line is null)
            _items.Add(new BasketItem(productId, qty, unitPrice));
        else
            line.Increase(qty);
    }

    public void Remove(int productId) =>
        _items.RemoveAll(i => i.ProductId == productId);

    public void ChangeQty(int productId, int newQty)
    {
        var line = _items.FirstOrDefault(i => i.ProductId == productId)
                   ?? throw new InvalidOperationException("Line not found");
        if (newQty <= 0) Remove(productId);
        else line.SetQty(newQty);
    }

    public decimal Total() => _items.Sum(i => i.SubTotal);
}
