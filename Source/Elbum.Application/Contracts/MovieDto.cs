namespace Elbum.Application.Contracts
{
    public class MovieDto
    {
        public float? RatingKinopoisk { get; set; }
        public int? Year { get; set; }
        public string Id { get; set; }
        public string NameRus { get; set; }
        public string MobiLinkId { get; set; }
        public float? Rating { get; set; }
        public bool? Serial { get; set; }
        public string Cover { get; set; }
        public string NameId { get; set; }
    }
}