namespace Basket.Domain;


public record BasketItem(int ProductId, int Quantity, decimal UnitPrice) : IBasketItem
{
    public decimal SubTotal => UnitPrice * Quantity;
    public void Increase(int by) => Quantity += by;
    public void SetQty(int qty) => Quantity = qty;
}