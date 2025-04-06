namespace AngularSaleAPI.Application.Features.Queries.Order.GetAllOrder
{
    public class GetAllOrderQueryResponse
    {
        public int TotalCount { get; set; }
        public object Orders { get; set; }
    }
}