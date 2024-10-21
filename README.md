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

