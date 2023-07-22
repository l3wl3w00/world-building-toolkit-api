using Api.Configurers;

var builder = WebApplication.CreateBuilder(args);
BuilderConfigurer.Configure(builder);
var app = builder.Build();
AppConfigurer.Configure(app);
app.Run();
