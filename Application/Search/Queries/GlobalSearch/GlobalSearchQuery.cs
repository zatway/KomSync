using Application.DTO.Search;
using MediatR;

namespace Application.Search.Queries.GlobalSearch;

public record GlobalSearchQuery(string Q, int Take = 40) : IRequest<IReadOnlyList<SearchHitDto>>;
