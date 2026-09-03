using Geekhub.Backend.Domain.Adapters;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Trending;
using TMDbLib.Objects.TvShows;

namespace Geekhub.Backend.WebApi.Routes;

public static class MediaController
{
    private record MediaControllerServices(
    [FromServices] IMapper Mapper,
    [FromServices] TMDbClient TmdbClient,
    [FromServices] IRedisAdapter RedisAdapter);

    private static readonly TimeSpan Duration7Days = TimeSpan.FromDays(7);
    private static readonly TimeSpan Duration1Hour = TimeSpan.FromHours(1);

    public static void MapMediaController(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/media")
            .WithTags("Media")
            .WithDescription("Operações relacionadas às mídias do TMDB");

        group.MapGet("/movie/{term}/search", async Task<IResult> (string term, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .TmdbClient
                .SearchMovieAsync(term));
        })
            .WithName("GetMovieByTitle")
            .WithSummary("Obtém um filme pelo título")
            .WithDescription("Retorna os dados de um filme correspondente ao termo informado.")
            .Produces<SearchContainer<SearchMovie>?>(StatusCodes.Status200OK);

        group.MapGet("/movie/{id:int}/id", async Task<IResult> (int id, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"movie:{id}", () => services
                    .TmdbClient
                    .GetMovieAsync(id), Duration7Days));
        })
            .WithName("GetMovieById")
            .WithSummary("Obtém um filme pelo ID")
            .WithDescription("Retorna os dados do filme correspondente ao Id informado.")
            .Produces<Movie?>(StatusCodes.Status200OK);

        group.MapGet("/tv/{term}/search", async Task<IResult> (string term, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .TmdbClient
                .SearchTvShowAsync(term));
        })
            .WithName("GetTVShowByTitle")
            .WithSummary("Obtém uma série de TV pelo título")
            .WithDescription("Retorna os dados da série de TV correspondente ao termo informado.")
            .Produces<SearchContainer<SearchTv>?>(StatusCodes.Status200OK);

        group.MapGet("/tv/{id}/id", async Task<IResult> (int id, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"tv:{id}", () => services
                    .TmdbClient
                    .GetTvShowAsync(id), Duration7Days));
        })
            .WithName("GetTVShowById")
            .WithSummary("Obtém uma série de TV pelo ID")
            .WithDescription("Retorna os dados da série de TV correspondente ao Id informado.")
            .Produces<TvShow?>(StatusCodes.Status200OK);

        group.MapGet("/tv/{id}/season/{seasonNumber}/episode/{episodeNumber}", async Task<IResult> (int id, int seasonNumber, int episodeNumber, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"tv:{id}:{seasonNumber}:{episodeNumber}", () => services
                    .TmdbClient
                    .GetTvEpisodeAsync(id, seasonNumber, episodeNumber), Duration7Days));
        })
            .WithName("GetTVEpisodeById")
            .WithSummary("Obtém um episódio de uma série de TV pelo ID")
            .WithDescription("Retorna os dados do episódio correspondente ao Id da série, número da temporada e número do episódio informados.")
            .Produces<TvEpisode?>(StatusCodes.Status200OK);

        group.MapGet("/tv/{id}/season/{seasonNumber}", async Task<IResult> (int id, int seasonNumber, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"tv:{id}:{seasonNumber}", () => services
                    .TmdbClient
                    .GetTvSeasonAsync(id, seasonNumber), Duration7Days));
        })
            .WithName("GetTVSeasonById")
            .WithSummary("Obtém uma temporada de uma série de TV pelo ID")
            .WithDescription("Retorna os dados da temporada correspondente ao Id da série e número da temporada informados.")
            .Produces<TvSeason?>(StatusCodes.Status200OK);

        group.MapGet("/person/{term}/search", async Task<IResult> (string term, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .TmdbClient
                .SearchPersonAsync(term));
        })
            .WithName("GetPersonByName")
            .WithSummary("Obtém uma pessoa pelo nome")
            .WithDescription("Retorna os dados de uma pessoa correspondente ao termo informado.")
            .Produces<SearchContainer<TvShow>?>();

        group.MapGet("/person/{id}/id", async Task<IResult> (int id, [AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"person:{id}", () => services
                    .TmdbClient
                    .GetPersonAsync(id), Duration7Days));
        })
            .WithName("GetPersonById")
            .WithSummary("Obtém uma pessoa pelo ID")
            .WithDescription("Retorna os dados da pessoa correspondente ao Id informado.")
            .Produces<Person?>();

        group.MapGet("/trends/movies", async Task<IResult> ([AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"trending:movies", () => services
                    .TmdbClient
                    .GetTrendingMoviesAsync(TimeWindow.Day), Duration1Hour));
        })
            .WithName("GetTrendingMovies")
            .WithSummary("Obtém os filmes em destaque")
            .WithDescription("Retorna os dados dos filmes em destaque no momento.")
            .Produces<SearchContainer<SearchMovie>>();

        group.MapGet("/trends/tv", async Task<IResult> ([AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"trending:tv", () => services
                    .TmdbClient
                    .GetTrendingTvAsync(TimeWindow.Day), Duration1Hour));
        })
            .WithName("GetTrendingTVShows")
            .WithSummary("Obtém as séries de TV em destaque")
            .WithDescription("Retorna os dados das séries de TV em destaque no momento.")
            .Produces<SearchContainer<SearchTv>>();

        group.MapGet("/now/playing", async Task<IResult> ([AsParameters] MediaControllerServices services) =>
        {
            return Results.Ok(await services
                .RedisAdapter
                .GetOrCreateAsync($"now:playing", () => services
                    .TmdbClient
                    .GetMovieNowPlayingListAsync(), Duration1Hour));
        })
            .WithName("GetNowPlayingMovies")
            .WithSummary("Obtém os filmes em cartaz")
            .WithDescription("Retorna os dados dos filmes que estão em cartaz no momento.")
            .Produces<SearchContainerWithDates<SearchMovie>>();
    }
}
