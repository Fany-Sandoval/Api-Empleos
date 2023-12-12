using Firebase.Auth.Objects;

using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FireSharp.Interfaces;
using Google.Apis.Auth.OAuth2;

using ResenaApi.Modelos;
using ResenaApi.Services;
using ResenaApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
{
    AuthSecret = "WxrfWdNQtAMzLKnyV9QB1VnzR340gCPeGcU2Tywe",
    BasePath = "https://maui-6f1ac-default-rtdb.firebaseio.com/"
};

IFirebaseClient firebaseClient = new FireSharp.FirebaseClient(config);


var credentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clave.json");
var credentials = GoogleCredential.FromFile(credentialsPath);
var apps = FirebaseApp.DefaultInstance;
if (apps == null)
{
    apps = FirebaseApp.Create(new AppOptions
    {
        Credential = credentials
    });
}

builder.Services.AddSingleton<Firebase.Auth.Provider.FirebaseAuthProvider>(_ =>
{
    var config = new Firebase.Auth.Config.FirebaseConfig("AIzaSyDKPXC3kGaBXBXHw5mgdRpXd1KyrAeeYMU");
    return new Firebase.Auth.Provider.FirebaseAuthProvider(config);
});



// Registrar el servicio FirebaseAuth
builder.Services.AddSingleton<FirebaseAdmin.Auth.FirebaseAuth>(_ => FirebaseAdmin.Auth.FirebaseAuth.GetAuth(apps));

// Add services to the container.
builder.Services.AddSingleton(firebaseClient);


builder.Services.AddScoped<IServiceAuth,ServiceAuth>();

builder.Services.AddScoped<IServicePersonas,ServicePersonas>();
builder.Services.AddScoped<IServiceEmpleo, ServiceEmpleos>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
