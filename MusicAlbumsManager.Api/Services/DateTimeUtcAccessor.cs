using MusicAlbumsManager.Api.Interfaces;

namespace MusicAlbumsManager.Api.Services;

public class DateTimeUtcAccessor : IDateTimeAccessor
{
    public DateTime Now => DateTime.UtcNow;
}
