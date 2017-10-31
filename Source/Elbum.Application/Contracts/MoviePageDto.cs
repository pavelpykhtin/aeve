using System.Collections.Generic;

namespace Elbum.Application.Contracts
{
    public class MoviePageDto
    {
        public List<MovieDto> Items { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}