# Kurulum ve Çalıştırma Yönergeleri

## 1. Veritabanı Güncellemeleri:
- Migration dosyasının bulunduğu dizinde, `Update-Database` komutunu çalıştırarak veritabanınızı güncelleyin. Bu işlem veritabanı yapısını oluşturacaktır.
- Daha sonra, iletilen SQL scriptini kullanarak gerekli verileri ekleyin. Verilerin sisteme doğru bir şekilde yüklenmesi, uygulamanın düzgün çalışabilmesi için kritik öneme sahiptir. Özellikle, aşağıdaki tablolarda yer alan veriler hayati önem taşımaktadır:
  - `User`
  - `Role`
  - `Permission`
  - `UserRoles`
  - `RolePermission`

## 2. Proje Yapılandırması:
- `ECommerce.API` projesini başlangıç projesi olarak ayarlayın.
- Proje, 3 farklı şekilde çalıştırılabilir:
  - **HTTP**
  - **HTTPS**
  - **IIS Express**
  
  İhtiyaçlarınıza göre bu seçeneklerden biri ile çalıştırabilirsiniz.

## 3. Uygulamanın Başlatılması:
- Uygulama başlatıldığında, **SwaggerLogin** ekranı ile karşılaşacaksınız.
- Sisteme giriş yapmak için, yalnızca **Creator** rolüne sahip kullanıcıların kullanıcı adı ve şifre bilgileri geçerlidir. Diğer kullanıcıların giriş bilgileri geçersiz sayılacaktır. Arka planda, servis veritabanına yapılan isteklerle kullanıcı doğrulaması yapılmaktadır.

### Örnek Swagger Giriş Bilgileri:
- Kullanıcı Adı: `oguzhan.sadikoglu@ecommerce.com`
- Şifre: `Oguzhan.1907`

## 4. Token Alımı ve Servis Erişimi:
- Başarılı bir giriş işlemi sonrasında, `User/Login` endpoint’i aracılığıyla bir token alınacaktır. Bu token ile diğer servislere istekler yapılabilir.
- **SüperAdmin** rolüne sahip kullanıcılar, tüm servislerle etkileşime geçebilecek şekilde tüm yetkilere sahiptir. 
- **Creator** rolüne sahip kullanıcılar ise, rol tanımlarında herhangi bir yetki verilmemiş olsa dahi tüm servislere erişim sağlayabilecektir. 
  - Uygulama, kod tarafında bu mantıkla yapılandırılmıştır.

 Alternatif Giriş JSON Formatı:
{
  "email": "info@ecommerce.com",
  "password": "Oguzhan.1907"
}


## 5. ApiLog ve Entity Kayıtları:
- Tüm API istekleri (GET, POST vb.) ve bunların response/request body'leri, kullanıcı bilgileri (guest dahil), tarayıcı bilgileri gibi detaylar **ApiLogs** tablosunda tutulmaktadır.
- Ayrıca, uygulama üzerinden herhangi bir entity (create, update, delete) işlemi gerçekleştirildiğinde, ilgili işlem **Logs** tablosuna kaydedilecektir. 
  - Bu tabloda entity'nin eski ve yeni halleri arasında karşılaştırma yapılabilir.
