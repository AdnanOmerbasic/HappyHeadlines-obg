namespace Events
{
        public enum Continent
        {
            AF,
            AN,
            AS,
            EU,
            NA,
            AU,
            SA,
            GL
        }
        public class CreateArticleEvent
        {
            public required string Title { get; set; }
            public required string Content { get; set; }
            public required string AuthorName { get; set; }
            public Continent Continent { get; set; }
            public Dictionary<string, string> Headers { get; set; } = new();
    }
}
