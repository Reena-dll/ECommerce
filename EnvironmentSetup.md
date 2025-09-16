# Kurulum ve Çalýþtýrma Yönergeleri

## 1. Veritabaný Güncellemeleri:
- Migration dosyasýnýn bulunduðu dizinde, `Update-Database` komutunu çalýþtýrarak veritabanýnýzý güncelleyin. Bu iþlem veritabaný yapýsýný oluþturacaktýr.
- Daha sonra, iletilen SQL scriptini kullanarak gerekli verileri ekleyin. Verilerin sisteme doðru bir þekilde yüklenmesi, uygulamanýn düzgün çalýþabilmesi için kritik öneme sahiptir. Özellikle, aþaðýdaki tablolarda yer alan veriler hayati önem taþýmaktadýr:
  - `User`
  - `Role`
  - `Permission`
  - `UserRoles`
  - `RolePermission`

## 2. Proje Yapýlandýrmasý:
- `ECommerce.API` projesini baþlangýç projesi olarak ayarlayýn.
- Proje, 3 farklý þekilde çalýþtýrýlabilir:
  - **HTTP**
  - **HTTPS**
  - **IIS Express**
  
  Ýhtiyaçlarýnýza göre bu seçeneklerden biri ile çalýþtýrabilirsiniz.

## 3. Uygulamanýn Baþlatýlmasý:
- Uygulama baþlatýldýðýnda, **SwaggerLogin** ekraný ile karþýlaþacaksýnýz.
- Sisteme giriþ yapmak için, yalnýzca **Creator** rolüne sahip kullanýcýlarýn kullanýcý adý ve þifre bilgileri geçerlidir. Diðer kullanýcýlarýn giriþ bilgileri geçersiz sayýlacaktýr. Arka planda, servis veritabanýna yapýlan isteklerle kullanýcý doðrulamasý yapýlmaktadýr.

### Örnek Swagger Giriþ Bilgileri:
- Kullanýcý Adý: `oguzhan.sadikoglu@ecommerce.com`
- Þifre: `Oguzhan.1907`

## 4. Token Alýmý ve Servis Eriþimi:
- Baþarýlý bir giriþ iþlemi sonrasýnda, `User/Login` endpoint’i aracýlýðýyla bir token alýnacaktýr. Bu token ile diðer servislere istekler yapýlabilir.
- **SüperAdmin** rolüne sahip kullanýcýlar, tüm servislerle etkileþime geçebilecek þekilde tüm yetkilere sahiptir. 
- **Creator** rolüne sahip kullanýcýlar ise, rol tanýmlarýnda herhangi bir yetki verilmemiþ olsa dahi tüm servislere eriþim saðlayabilecektir. 
  - Uygulama, kod tarafýnda bu mantýkla yapýlandýrýlmýþtýr.

 Alternatif Giriþ JSON Formatý:
{
  "email": "info@ecommerce.com",
  "password": "Oguzhan.1907"
}


## 5. ApiLog ve Entity Kayýtlarý:
- Tüm API istekleri (GET, POST vb.) ve bunlarýn response/request body'leri, kullanýcý bilgileri (guest dahil), tarayýcý bilgileri gibi detaylar **ApiLogs** tablosunda tutulmaktadýr.
- Ayrýca, uygulama üzerinden herhangi bir entity (create, update, delete) iþlemi gerçekleþtirildiðinde, ilgili iþlem **Logs** tablosuna kaydedilecektir. 
  - Bu tabloda entity'nin eski ve yeni halleri arasýnda karþýlaþtýrma yapýlabilir.