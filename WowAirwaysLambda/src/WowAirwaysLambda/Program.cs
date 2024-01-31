using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using WowAirwaysLambda.API.Models;
using WowAirwaysLambda.API.Repository;
using WowAirwaysLambda.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//enable CORS
builder.Services.AddCors(p=> p.AddPolicy("CorsPolicy"
    , build => { 
        build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }
    ));

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<DynamoDbRepository>();
builder.Services.AddSingleton<PdfService>();


var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

static IResult Ping()
{
    return Results.Ok("Success");
}

static async Task<IResult> AddAttendee([FromBody] Attendee attendee
    , DynamoDbRepository dynamoDbRepository)
{

    attendee.BookingReference = Guid.NewGuid().ToString().Substring(0, 17).ToUpper();
    attendee.FlightNo = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
    attendee.SeatNo = "0";

    var response = await dynamoDbRepository.AddItem(attendee);

    return Results.Created($"/attendees/{attendee.Id}", attendee);
}

static async Task<IResult> GetAttendees(DynamoDbRepository dynamoDbRepository)
{
    var response = await dynamoDbRepository.GetItems();

    return Results.Ok(response);
}

static async Task<IResult> CreateItineraryFile(PdfService pdfService)
{
    var bookingReference = Guid.NewGuid().ToString().Substring(0,17).ToUpper();
    var flightNo = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();

    var bytes = pdfService.CreateItineraryFile(bookingReference, flightNo, "GINO", "INGRESO");

    return Results.File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Edited.pdf");
}

app.MapGet("/", Ping);
app.MapGet("/ping", Ping);
app.MapPost("/attendees/", AddAttendee);
app.MapGet("/attendees/", GetAttendees);
app.MapGet("/email/", CreateItineraryFile);

app.Run();
