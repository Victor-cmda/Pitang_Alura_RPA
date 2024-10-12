using IoC;
using Infrastructure.Data;  // Certifique-se de que o namespace do AppDbContext está correto
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Adicionar controle de dependências e JSON
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

// Registrar os serviços usando a injeção de dependências
DependencyInjection.RegisterServices(builder.Services);

// Configurar o banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pitang RPA API", Version = "v1" });
});

var app = builder.Build();

// Configurar criação do banco de dados e aplicação de migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Cria o banco de dados se não existir
    dbContext.Database.EnsureCreated();
    
    // Aplica as migrations pendentes
    dbContext.Database.Migrate();
}

// Usar CORS
app.UseCors("AllowAll");

// Verificar se está em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Usar HTTPS
app.UseHttpsRedirection();

// Configurar autorização
app.UseAuthorization();

// Mapear os controladores
app.MapControllers();

// Usar Swagger e Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pitang RPA API V1");
});

// Executar a aplicação
app.Run();
