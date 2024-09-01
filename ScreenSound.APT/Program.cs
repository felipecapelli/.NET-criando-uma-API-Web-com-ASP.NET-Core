using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

//API = Application Programming Interface

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) => 
{
    //var dal = new DAL<Artista>(new ScreenSoundContext());
    return Results.Ok(dal.Listar());
});

app.MapGet("/Artistas/{nome}", ([FromServices] DAL < Artista > dal, string nome) =>
{
    //var dal = new DAL<Artista>(new ScreenSoundContext());
    var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if (artista is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(artista);

});

app.MapPost("/Artistas", ([FromServices] DAL < Artista > dal, [FromBody]Artista artista) =>
{
    //var dal = new DAL<Artista>(new ScreenSoundContext());
    dal.Adicionar(artista);
    return Results.Ok();
});

app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
{
    var artista = dal.RecuperarPor(a => a.Id == id);
    if (artista is null)
    {
        return Results.NotFound();
    }
    dal.Deletar(artista);
    return Results.NoContent();
});

app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, Artista artista) =>
{
    var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);
    if (artistaAAtualizar is null)
    {
        return Results.NotFound();
    }

    artistaAAtualizar.Nome = artista.Nome;
    artistaAAtualizar.Bio = artista.Bio;
    artistaAAtualizar.FotoPerfil = artista.FotoPerfil;

    dal.Atualizar(artistaAAtualizar);
    return Results.Ok();
});

app.Run();
