using Spaghetti.Core.DataTransferObject;
using Spaghetti.Domain.DataTransferObject;
using Spaghetti.ElasticSearch.ElasicSearch;
using Spaghetti.Core.DI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DI

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/IndexDocument", (DocumentIndexDto instance) =>
{
    var service = new ElasticSerachService();
    service.InsertIndex(instance);

});

app.MapPost("/IndexDocuments", (DocumentsIndexDto instance) =>
{
    var service = new ElasticSerachService();
    service.InsertManyIndex(instance);

});

app.MapGet("/SearchIndex", (string keyword) =>
{
    var service = new ElasticSerachService();
    return service.ESSearch(keyword);

});



app.Run();
