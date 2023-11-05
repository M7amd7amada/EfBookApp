var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.WithOrigins("AnyOrigin");
        config.AllowAnyHeader();
        config.AllowAnyMethod();
    });
    options.AddPolicy("AnyOrigin", config =>
    {
        config.AllowAnyOrigin();
        config.AllowAnyHeader();
        config.AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();

app.MapGet("/error", [ResponseCache(NoStore = true)] () =>
    Results.Problem()).RequireCors("AnyOrigin");
app.MapGet("/error/test", [ResponseCache(NoStore = true)] () =>
    { throw new Exception(); });

app.MapControllers();

app.Run();