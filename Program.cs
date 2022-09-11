using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movie.Database.ApplecationContext;
using Movie.Helper;
using Movie.Mapper;
using Movie.Repository.InterFace;
using Movie.Repository.Repos;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ApplecationDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("MoviesConnection")));

builder.Services.AddCors();
builder.Services.AddScoped<IGerneInterface, GerneRepo>();
builder.Services.AddScoped<IMoviesInterface, MoviesRepo>();
builder.Services.AddScoped<IAuthInterface, IAuthRepo>();
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplecationDbContext>();

builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
               .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidIssuer = builder.Configuration["JWT:Issuer"],
                       ValidAudience = builder.Configuration["JWT:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                   };
               });
var app = builder.Build();

// Configure the HTTP request pipeline.


    app.UseSwagger();
    app.UseSwaggerUI();




app.UseHttpsRedirection();

app.UseCors(options => options
       .AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
