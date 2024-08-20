using Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder
    .AddProject<Projects.AspireStarter_ApiService>("apiservice")
    .WithDaprSidecar();

var senderService = builder
    .AddProject<Projects.AspireStarter_Sender>("sender")
    .WithDaprSidecar();

var redis = builder.AddRedis("cache");

builder.AddProject<Projects.AspireStarter_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(senderService)
    .WithReference(redis)
    .WithDaprSidecar(new DaprSidecarOptions()
    {
        DaprGrpcPort = 50003,
        DaprHttpPort = 3500,
        MetricsPort = 9093
    });

builder.Build().Run();
