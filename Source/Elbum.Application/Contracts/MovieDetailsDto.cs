using System;

namespace Elbum.Application.Contracts
{
    public class MovieDetailsConainerDto
    {
        public MovieDetailsDto Movie { get; set; }
    }

    public class MovieDetailsDto
    {
        public string Description { get; set; }
        public DateTime MobiLinkDate { get; set; }
    }
}