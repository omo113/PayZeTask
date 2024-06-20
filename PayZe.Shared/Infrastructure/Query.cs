namespace PayZe.Shared.Infrastructure
{
    public record Query<T> : Query
    {
        public Query()
        {
            Params = Activator.CreateInstance<T>();
        }

        public T Params { get; init; }
    }

    public record Query
    {
        public string? Search { get; set; }
        public required int PageIndex { get; init; }
        public required int PageSize { get; init; }
    }
}