using System;

namespace Aeve.Contracts
{
    public class CreateMovieRequest
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal? Rating { get; set; }
        public int? Year { get; set; }
        public string Url { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}