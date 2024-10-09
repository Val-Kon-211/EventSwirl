using EventSwirl.Application.Data.Mapping;
using EventSwirl.Application.Services;
using EventSwirl.Application.Services.Interfaces;
using EventSwirl.DataAccess;
using EventSwirl.DataAccess.Handlers;
using EventSwirl.DataAccess.Interfaces;
using EwentSwirl.RabbitMQ;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(AppMappingProfile));
        builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();

        // Add DbContext
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));

        // Migrator service
        builder.Services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DataContext).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<ICommandHandler, CreateEventCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, CreateUserCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, DeleteEventByIdCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, DeleteUserCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, GetAllEventsCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, GetEventByIdCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, GetEventsByUserIdCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, GetUserByIdCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, GetUserByLoginCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, UpdateEventCommandHandler>();
        builder.Services.AddScoped<ICommandHandler, UpdateUserCommandHandler>();

        builder.Services.AddScoped<IProducer, Producer>();

        builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();


        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ISecurityService, SecurityService>();

        builder.Services.AddScoped<CommandDispatcher>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();

            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            if (runner.HasMigrationsToApplyUp())
            {
                runner.ListMigrations();
                runner.MigrateUp();
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var dispatcher = scope.ServiceProvider.GetRequiredService<CommandDispatcher>();
            dispatcher.StartListening();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventSwirl API V1");
        });

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.Run();
    }
}