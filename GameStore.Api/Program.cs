using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GamesDto> games = [
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

// GET
app.MapGet("/games", () => games);

// GET by ID
app.MapGet("/games/{id}", (int id) => games.FirstOrDefault(g => g.Id == id) is GamesDto game ? Results.Ok(game) : Results.NotFound())
.WithName(GetGameEndpointName);

// POST Create Game 
app.MapPost("/games", (CreateGameDto game) => {
    var newGame = new GamesDto(games.Count + 1, game.Name, game.Genre, game.Price, game.ReleaseDate);
    games.Add(newGame);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = newGame.Id }, newGame);
});

// PUT /games
app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) =>
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

    
    return Results.NoContent();
});

// DELETE /games/1

app.MapDelete("/games/{id}", (int id) =>
{
    var game = games.FirstOrDefault(g => g.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }

    games.Remove(game);
    return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.Run();
