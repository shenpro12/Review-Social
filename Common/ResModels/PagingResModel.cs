namespace review.Common.ResModels
{
    public class PagingResModel
    {
        public decimal TotalPage { get; set; }

        public decimal TotalData { get; set; }

        public int CurrentPage { get; set; }

        public object? Data { get; set; }
    }
}
