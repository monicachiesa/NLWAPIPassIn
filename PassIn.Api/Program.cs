using PassIn.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter))); //adiciona o filtro de excessãp
//não importa aonde no projeto ocorrer a excessão, vai vir automaticamente pra cá

//coloca as urls das chamadas em lowercase no Swagger
//precisa vir antes do build, linha 15
builder.Services.AddRouting(options => options.LowercaseUrls = true);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
