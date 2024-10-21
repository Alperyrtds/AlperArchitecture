# AlperProject

AlperProject, MediatR, Entity Framework Core, ve Katmanlı Mimari prensiplerini kullanarak geliştirilmiş bir projedir. Bu proje, C# dilinde yazılmış olup, gelişmiş komut ve sorgu işleme yapısına sahiptir. Proje, güçlü bir altyapı ile veri tabanı işlemlerini yönetir ve kolayca genişletilebilir.

# Kullanılan Teknolojiler
- .NET 8
- MediatR - CQRS desenini uygulamak için
- Entity Framework Core - Veri tabanı işlemleri için
- Katmanlı Mimari - Ayrılmış ve ölçeklenebilir yazılım geliştirme için
- FluentValidation - Model doğrulama için
- ASP.NET Core Web API - API geliştirme için
- JWT (JSON Web Tokens) - Kullanıcı kimlik doğrulama için
- Redis - Cache Mekanizması için

# Katmanlar

Projede kullanılan katmanlar:
## Domain: Projenin iş kurallarını ve verilerini temsil eden en temel katmandır.

- Entities (varlıklar)
- Value Objects (değer nesneleri)

## Repositories : Depolama arayüzleri ve Cache mekanizmasını ele alır.
- EF Core implementasyonları

## Application: Uygulamanın iş mantığını ve kullanıcılara sunulan işlevleri yönetir.

- Commands (komutlar)
- Queries (sorgular)
- Validators (doğrulayıcılar)
- Handlers (işleyiciler)

## Infrastructure: Veritabanı erişimi ve diğer dış servislerle (örneğin e-posta gönderme, dosya sistemine yazma) etkileşimleri sağlar.

- Üçüncü parti servisler ( Mail, Telegram, Sms) service gibi.

## WebApi: API isteğini alan ve son kullanıcıya yanıt veren katmandır. Burada MediatR aracılığıyla iş akışları başlatılır.

- Controllers
- Filters
- Middleware

## Installation
## Gereksinimler
- .NET 8 SDK
- SQL Server veya uyumlu bir veri tabanı (örneğin MSSQL, PostgreSQL)
- Visual Studio veya Visual Studio Code (C# desteği ile)

### Adımlar

1. Projeyi klonlayın:

```bash
git clone https://github.com/Alperyrtds/AlperProject.git
cd AlperProject
```
2. Gerekli bağımlılıkları yükleyin:

```bash
dotnet restore
```
3. Veritabanı ayarlarını yapılandırın. appsettings.json dosyasındaki bağlantı dizesini (connection string) kendi veritabanınıza göre güncelleyin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

4. Veritabanı migrasyonlarını uygulayın:
```bash
dotnet ef database update
```
5. Uygulamayı çalıştırın:
```bash
dotnet run
```

## Kullanım
Uygulamayı çalıştırdıktan sonra, Postman veya benzeri bir API istemcisi ile aşağıdaki uç noktalara istek gönderebilirsiniz:
### Örnek API Uç Noktaları
- Kullanıcı oluşturma (POST):

POST /api/users/create

```json
Body:
{
    "email": "user@example.com",
    "name": "John",
    "surname": "Doe",
    "password": "YourSecurePassword",
    "phoneNumber": "1234567890"
}
```

- Kullanıcı create (POST):

POST /api/auth/login
```json
Body:
{
    "email": "user@example.com",
    "password": "YourSecurePassword"
}
```
## GET /getUserByEmail
- Retrieves a user by their email address.
## Request Body:
POST /api/auth/login
```json
{
  "eposta": "alper.yurtdas06@gmail.com"
}
```
## Get api/Sistem/cleanRedisKeys
```json
- Rediste biriken Keyleri silmek için kullanılır

```

# MediatR Kullanımı

Projede MediatR, CQRS desenini uygulamak için kullanılmaktadır. Komutlar (Commands) ve Sorgular (Queries) Application katmanında tanımlanır ve işleyicileri (Handlers) tarafından işlenir.

Örneğin:
- CreateUserCmd komutu bir kullanıcı oluşturmak için kullanılır. Bu komut, CreateUserHandler sınıfı tarafından işlenir ve veritabanına ekleme işlemi yapılır.


# Using Redis
To utilize Redis for caching, ensure that your Redis server is running and configured in your application. Implement caching strategies to optimize data retrieval for frequently accessed data.

# Validasyon
Kullanıcı girdileri FluentValidation ile doğrulanmaktadır. Örneğin, kullanıcı oluşturma işlemi sırasında CreateUserVld sınıfı, e-posta ve şifre gibi alanların doğruluğunu kontrol eder.

# Katkıda Bulunma

Katkıda bulunmak isterseniz, bir "fork" yapın ve ardından pull request gönderin. Yeni özellikler veya hata düzeltmeleri her zaman memnuniyetle karşılanır.

- Projeyi fork edin
- Yeni bir dal (feature/your-feature) oluşturun
- Değişikliklerinizi commit edin
- Dallara (feature/your-feature) gönderin
- Bir Pull Request açın

# Lisans
Bu proje MIT Lisansı ile lisanslanmıştır. Daha fazla bilgi için LICENSE dosyasına göz atın.

[MIT](https://github.com/Alperyrtds/-lper/tree/master?tab=MIT-1-ov-file)


--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

# AlperProject

AlperProject is a project developed using MediatR, Entity Framework Core, and Layered Architecture principles. This project is written in C# and features an advanced command and query processing structure. It manages database operations with a robust infrastructure and is easily extensible.

# Technologies Used
- .NET 8
- MediatR - for implementing the CQRS pattern
- Entity Framework Core - for database operations
- Layered Architecture - for modular and scalable software development
- FluentValidation - for model validation
- ASP.NET Core Web API - for API development
- JWT (JSON Web Tokens) - for user authentication
- Redis - for caching mechanism

# Katmanlar

The layers used in the project are:
## Domain: The core layer representing business rules and data.

- Entities (models)
- Value Objects (value types)
  
## Repositories: Manages storage interfaces and the caching mechanism.
- EF Core implementations

## Application: Manages business logic and functionalities provided to users.

- Commands 
- Queries 
- Validators 
- Handlers 

## Infrastructure: Facilitates database access and interactions with external services (e.g., email sending, file writing).

- Third-party services (Mail, Telegram, SMS)

## WebApi: The layer that handles API requests and responds to the end user. Workflow is initiated here via MediatR.

- Controllers
- Filters
- Middleware

# Installation

## Requirements
- .NET 8 SDK
- SQL Server or a compatible database (e.g., MSSQL, PostgreSQL)
- Visual Studio or Visual Studio Code (with C# support)

### Steps

1. Clone the project:

```bash
git clone https://github.com/Alperyrtds/AlperProject.git
cd AlperProject
```
2. Install the necessary dependencies:

```bash
dotnet restore
```
3. Configure the database settings. Update the connection string in the appsettings.json file to match your database:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

4. Apply database migrations:
```bash
dotnet ef database update
```
5. Run the application:
```bash
dotnet run
```

# Usage
After running the application, you can send requests to the following endpoints using Postman or a similar API client:

### Örnek API Uç Noktaları
- Create user (POST):

POST /api/users/create

```json
Body:
{
    "email": "user@example.com",
    "name": "John",
    "surname": "Doe",
    "password": "YourSecurePassword",
    "phoneNumber": "1234567890"
}
```

- Login (POST):

POST /api/auth/login
```json
Body:
{
    "email": "user@example.com",
    "password": "YourSecurePassword"
}
```
## GET /getUserByEmail
- Retrieves a user by their email address.
## Request Body:
POST /api/auth/login
```json
{
  "eposta": "alper.yurtdas06@gmail.com"
}
```
## Get api/Sistem/cleanRedisKeys
```json
- Used to delete keys accumulated in Redis.

```

# Using MediatR

MediatR is used in the project to implement the CQRS pattern. Commands and Queries are defined in the Application layer and processed by Handlers.

For example:
- The CreateUserCmd command is used to create a user. This command is processed by the CreateUserHandler class, which handles the addition to the database.


# Using Redis
To utilize Redis for caching, ensure that your Redis server is running and configured in your application. Implement caching strategies to optimize data retrieval for frequently accessed data.

# Validation
User inputs are validated using FluentValidation. For instance, during the user creation process, the CreateUserVld class checks the validity of fields such as email and password.

# Contributing

If you wish to contribute, please fork the project and submit a pull request. New features or bug fixes are always welcome.

- Fork the project
- Create a new branch (feature/your-feature)
- Commit your changes
- Push to the branch (feature/your-feature)
- Open a Pull Request

# License
This project is licensed under the MIT License. For more information, please refer to the LICENSE file.

[MIT](https://github.com/Alperyrtds/-lper/tree/master?tab=MIT-1-ov-file)
