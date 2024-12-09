namespace GameStore.Api.Dtos;

public record class GamesDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate);

