using FluentValidation;

namespace MusicAlbumsManager.Api.Features.Albums;


    public class AddAlbumsValidator : AbstractValidator<AddAlbums.Request>
    {
        public AddAlbumsValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("User name is required");

            RuleFor(x => x.Albums)
                .NotNull()
                .WithMessage("Albums are required");
        }
    }

public class RemoveAlbumsValidator : AbstractValidator<RemoveAlbums.Request>
{
    public RemoveAlbumsValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("User name is required");
        RuleFor(x => x.AlbumNames)
            .NotNull()
            .WithMessage("At least one album name is required")
            .Must(albumNames => albumNames != null && albumNames.Count > 0)
            .WithMessage("At least one valid album name is required.");
    }
}
public class SearchAlbumsValidator : AbstractValidator<SearchAlbums.Request>
{
    public SearchAlbumsValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.ArtistName))
            .WithMessage("Either album name or artist name must be provided.");

        RuleFor(x => x.ArtistName)
            .NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Either album name or artist name must be provided.");
    }
}

