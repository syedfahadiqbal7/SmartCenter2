var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.AFFZ_Admin>("affz-admin");
builder.AddProject<Projects.AFFZ_Customer>("affz-customer");
builder.AddProject<Projects.AFFZ_API>("affz-api");
builder.AddProject<Projects.AFFZ_Provider>("affz-provider");
builder.AddProject<Projects.SCAPI_WebFront>("scapi-webfront");


/*
 builder.AddProject<Projects.AFFZ_Admin>(
    name: "apiservice",
    configure: static project =>
    {
        project.ExcludeLaunchProfile = true;
        project.ExcludeKestrelEndpoints = false;
    })
    .WithHttpsEndpoint();
 */
builder.Build().Run();
