using RestaurantReviewProgram.Controllers;
using RestaurantReviewProgram.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    var filePath = Path.Combine(AppContext.BaseDirectory, "RestaurantReviewDocumentation.xml");
    options.IncludeXmlComments(filePath);
});
// Add singleton- Easy way
builder.Services.AddSingleton(typeof(RestaurantList));

// Add HTTPClient
builder.Services.AddHttpClient<RestaurantCreator>(options => options.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/geocode/json"));

// Add Secret
builder.Services.Configure<Secret>(builder.Configuration.GetSection(Secret.Section));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();