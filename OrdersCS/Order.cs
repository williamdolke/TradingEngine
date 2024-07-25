namespace TradingEngineServer.Orders
{
    public class Order : IOrderCore
    {
        // You can have a negative price but not a negative quantity
        public Order(IOrderCore orderCore, long price, uint quantity, bool isBuySide)
        {
            // PROPERTIES //
            Price = price;
            IsBuySide = isBuySide;
            InitialQuantity = quantity;
            CurrentQuantity = quantity;

            // FIELDS // 
            _orderCore = orderCore;
        }

        public Order(ModifyOrder modifyOrder) : this(modifyOrder, modifyOrder.Price, modifyOrder.Quantity, modifyOrder.IsBuySide)
        {

        }

        // PROPERTIES //
        public long Price { get; private set; }
        public uint InitialQuantity { get; private set; }
        public uint CurrentQuantity { get; private set; }
        public bool IsBuySide { get; private set; }
        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;

        // METHODS //
        public void increaseQuantity(uint quantityDelta)
        {
            CurrentQuantity += quantityDelta;
        }

        public CancelOrder ToCancelOrder()
        {
            return new CancelOrder(this);
        }

            public void decreaseQuantity(uint quantityDelta)
        {
            if (quantityDelta > CurrentQuantity)
                throw new InvalidOperationException($"Quantity Delta > Current Quantity for OrderId={OrderId}");

            CurrentQuantity -= quantityDelta;
        }

        // FIELDS //
        private readonly IOrderCore _orderCore;
    }
}
