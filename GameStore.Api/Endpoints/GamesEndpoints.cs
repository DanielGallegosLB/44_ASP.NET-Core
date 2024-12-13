using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GamesDto> games = [
        new (
        1,
        "The Legend of Zelda: Breath of the Wild",
        "Action-adventure",
        59.99m,
        new DateOnly(2017, 3, 3)
    ),
    new (
        2,
        "Super Mario Odyssey",
        "Platformer",
        59.99m,
        new DateOnly(2017, 10, 27)
    ),
    new (
        3,
        "Mario Kart 8 Deluxe",
        "Racing",
        59.99m,
        new DateOnly(2017, 4, 28)
    )];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
        .WithParameterValidation();

        // GET
        group.MapGet("/", () => games);

        // GET by ID
        group.MapGet("/{id}", (int id) => games.FirstOrDefault(g => g.Id == id) is GamesDto game ? Results.Ok(game) : Results.NotFound())
        .WithName(GetGameEndpointName);

        // POST Create Game 
        group.MapPost("/", (CreateGameDto newGame) =>
        {

            var game = new GamesDto(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = games[index] with
            {
                Name = updatedGame.Name,
                Genre = updatedGame.Genre,
                Price = updatedGame.Price,
                ReleaseDate = updatedGame.ReleaseDate
            };

            return Results.Ok(games[index]);
        });
        // DELETE /games/1

        group.MapDelete("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }

            games.Remove(game);
            return Results.NoContent();
        });

        return group;

    }
};
