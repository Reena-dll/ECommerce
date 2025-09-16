# Kurulum ve �al��t�rma Y�nergeleri

## 1. Veritaban� G�ncellemeleri:
- Migration dosyas�n�n bulundu�u dizinde, `Update-Database` komutunu �al��t�rarak veritaban�n�z� g�ncelleyin. Bu i�lem veritaban� yap�s�n� olu�turacakt�r.
- Daha sonra, iletilen SQL scriptini kullanarak gerekli verileri ekleyin. Verilerin sisteme do�ru bir �ekilde y�klenmesi, uygulaman�n d�zg�n �al��abilmesi i�in kritik �neme sahiptir. �zellikle, a�a��daki tablolarda yer alan veriler hayati �nem ta��maktad�r:
  - `User`
  - `Role`
  - `Permission`
  - `UserRoles`
  - `RolePermission`

## 2. Proje Yap�land�rmas�:
- `ECommerce.API` projesini ba�lang�� projesi olarak ayarlay�n.
- Proje, 3 farkl� �ekilde �al��t�r�labilir:
  - **HTTP**
  - **HTTPS**
  - **IIS Express**
  
  �htiya�lar�n�za g�re bu se�eneklerden biri ile �al��t�rabilirsiniz.

## 3. Uygulaman�n Ba�lat�lmas�:
- Uygulama ba�lat�ld���nda, **SwaggerLogin** ekran� ile kar��la�acaks�n�z.
- Sisteme giri� yapmak i�in, yaln�zca **Creator** rol�ne sahip kullan�c�lar�n kullan�c� ad� ve �ifre bilgileri ge�erlidir. Di�er kullan�c�lar�n giri� bilgileri ge�ersiz say�lacakt�r. Arka planda, servis veritaban�na yap�lan isteklerle kullan�c� do�rulamas� yap�lmaktad�r.

### �rnek Swagger Giri� Bilgileri:
- Kullan�c� Ad�: `oguzhan.sadikoglu@ecommerce.com`
- �ifre: `Oguzhan.1907`

## 4. Token Al�m� ve Servis Eri�imi:
- Ba�ar�l� bir giri� i�lemi sonras�nda, `User/Login` endpoint�i arac�l���yla bir token al�nacakt�r. Bu token ile di�er servislere istekler yap�labilir.
- **S�perAdmin** rol�ne sahip kullan�c�lar, t�m servislerle etkile�ime ge�ebilecek �ekilde t�m yetkilere sahiptir. 
- **Creator** rol�ne sahip kullan�c�lar ise, rol tan�mlar�nda herhangi bir yetki verilmemi� olsa dahi t�m servislere eri�im sa�layabilecektir. 
  - Uygulama, kod taraf�nda bu mant�kla yap�land�r�lm��t�r.

 Alternatif Giri� JSON Format�:
{
  "email": "info@ecommerce.com",
  "password": "Oguzhan.1907"
}


## 5. ApiLog ve Entity Kay�tlar�:
- T�m API istekleri (GET, POST vb.) ve bunlar�n response/request body'leri, kullan�c� bilgileri (guest dahil), taray�c� bilgileri gibi detaylar **ApiLogs** tablosunda tutulmaktad�r.
- Ayr�ca, uygulama �zerinden herhangi bir entity (create, update, delete) i�lemi ger�ekle�tirildi�inde, ilgili i�lem **Logs** tablosuna kaydedilecektir. 
  - Bu tabloda entity'nin eski ve yeni halleri aras�nda kar��la�t�rma yap�labilir.