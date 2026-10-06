using MusicAlbumsManager.Api.Domain.Entities;
using MusicAlbumsManager.Api.Interfaces;
using System.Text.Json;

namespace MusicAlbumsManager.Api.Infrastructure.Deezer;

public sealed class DeezerMusicLibrarySource(HttpClient httpClient) : IMusicLibrarySource
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public async Task<ICollection<Album>> GetAlbumsAsync(
        string? name,
        string? artistName,
        CancellationToken cancellationToken = default)
    {
        var searchTerms = new List<string>();

        if (!string.IsNullOrWhiteSpace(name))
            searchTerms.Add($"album:\"{EscapeSearchTerm(name)}\"");

        if (!string.IsNullOrWhiteSpace(artistName))
            searchTerms.Add($"artist:\"{EscapeSearchTerm(artistName)}\"");

        if (searchTerms.Count == 0)
            return Array.Empty<Album>();        

        var query = Uri.EscapeDataString(string.Join(" ", searchTerms));
        var response = await httpClient.GetFromJsonAsync<DeezerSearchResponse>(
            $"search/album?q={query}", JsonOptions, cancellationToken);

        return response?.Data?
            .Select(result => new Album
            {
                Name = result.Title!,
                ArtistName = result.Artist!.Name!,
                Url = result.Link!,
                Cover = result.CoverXl ?? result.CoverBig ?? result.CoverMedium ?? result.Cover
            })
            .ToArray() ?? [];
    }

    //Adjust to Deezer search syntax: https://developers.deezer.com/api/search
    private static string EscapeSearchTerm(string value) =>
        value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("\"", "\\\"", StringComparison.Ordinal);

    private sealed class DeezerSearchResponse
    {
        public List<DeezerAlbumResult>? Data { get; init; }
    }

    private sealed class DeezerAlbumResult
    {
        public required string Title { get; init; }
        public required string Link { get; init; }

        public string? Cover { get; init; }
        public string? CoverMedium { get; init; }
        public string? CoverBig { get; init; }
        public string? CoverXl { get; init; }

        public required DeezerArtist? Artist { get; init; }
    }

    private sealed class DeezerArtist
    {
        public required string Name { get; init; }
    }
}