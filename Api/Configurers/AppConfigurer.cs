namespace Api.Configurers;
internal class AppConfigurer
{
    private readonly WebApplication _app;

    private AppConfigurer(WebApplication app)
    {
        this._app = app;
    }

    internal static void Configure(WebApplication app)
    {
        new AppConfigurer(app).Configure();
    }

    private void Configure()
    {
        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }

        _app.UseHttpsRedirection();

        _app.UseAuthorization();

        _app.MapControllers();
    }
}