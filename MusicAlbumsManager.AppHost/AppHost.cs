var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("music-library-postgres-server")
    .WithHostPort(5432)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("music-library-db");

builder.AddProject<Projects.MusicAlbumsManager_Api>("musicalbumsmanager-api")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();
