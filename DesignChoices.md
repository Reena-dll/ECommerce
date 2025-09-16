# Tasar�m Kararlar�

## Giri�
Bu mimari, end�stri standartlar�na dayal� temiz, s�rd�r�lebilir ve mod�ler bir yap� sa�lamak amac�yla tasarlanm��t�r.
Tasar�m�n ana hedefi, �l�eklenebilirlik, test edilebilirlik ve bak�m kolayl��� sa�lamakt�r.
A�a��da, bu hedeflere ula�mak i�in al�nan temel mimari kararlar a��klanm��t�r.

## Katmanl� Mimari

Mimari, her biri belirli bir g�revi �stlenen d�rt ana katmana ayr�lm��t�r:

1. **ECommerce.API (API Katman�)**:  
   Bu katman, t�m d�� etkile�imleri (istemci API �a�r�lar�) y�netir. `Controllers` dizini, belirli alanlarla (�rne�in kategori, �r�n, kullan�c�) ilgili i�lemleri y�neten �e�itli controller'lar� i�erir. Ayr�ca, hata y�netimi ve izin kontrolleri de bu katmanda yap�l�r.

2. **ECommerce.Application (Uygulama Katman�)**:  
   Bu katman, i� mant���n� ve uygulama seviyesindeki servisleri i�erir. `DTO` ve `Services` dizinlerinde, verilerin ta��nmas� ve i�lenmesinden sorumlu s�n�flar yer al�r. Domain'e ait do�rulama i�lemleri `Validations` klas�r�nde bulunur.

3. **ECommerce.Domain (Domain Katman�)**:  
   Domain katman�, uygulaman�n temel i� yap�s�n� ve i� kurallar�n� i�erir. `Entities` dizini, veritaban� modellerini ve i� nesnelerini bar�nd�r�r. Bu katman, di�er katmanlardan ba��ms�z olup domain mant���n�n ba��ms�z bir �ekilde geli�tirilmesine olanak tan�r.

4. **ECommerce.Infrastructure (Altyap� Katman�)**:  
   Bu katman, uygulaman�n altyap�s�n� ve d�� ba��ml�l�klar�n� y�netir. `Context`, `Interceptors`, `Migrations` ve `Repositories` dizinleri, veritaban� etkile�imleri, veritaban� ge�i�leri ve di�er altyap� bile�enlerini i�erir. `UnitOfWork` ve `Repository` desenleri, veri eri�imini soyutlayarak birim testlerinin yap�lmas�n� kolayla�t�r�r.

## Sorumluluk Ayr�m� ve Katmanlar Aras� �leti�im

Her katman, yaln�zca kendisine ait sorumlulu�a sahip olup katmanlar aras�ndaki ileti�im minimum d�zeyde tutulmu�tur. Katmanlar, arabirimler ve servisler arac�l���yla etkile�imde bulunur, b�ylece gev�ek ba�lanabilirlik (loose coupling) ve y�ksek tutarl�l�k sa�lan�r. Bu yakla��m, bak�m kolayl��� sa�lar ve sorumluluklar�n do�ru �ekilde ayr�lmas�n� garanti eder.

## SOLID Prensiplerine Uyulmas�

T�m s�n�flar, **Single Responsibility Principle (SRP)** prensibine uygun olarak tasarlanm��t�r. Her s�n�f, tek bir i�levi yerine getirir, bu da kodun okunabilirli�ini, s�rd�r�lebilirli�ini ve test edilebilirli�ini art�r�r.

- �rne�in, `UserService` yaln�zca kullan�c� y�netimi ile ilgilenirken, `CategoryService` sadece kategori i�lemleriyle ilgilenir. Bu �ekilde her bile�en, tek bir sorumlulu�a sahiptir.
  
Di�er SOLID prensiplerinin kullan�m�, mimarinin esnek, kolayca de�i�tirilebilir ve de�i�ikliklere diren�li olmas�n� sa�lar.

## Ba��ml�l�k Enjeksiyonu ve Test Edilebilirlik

T�m ba��ml�l�klar **Dependency Injection (DI)** kullan�larak enjekte edilmi�tir. Bu sayede bile�enler aras�ndaki s�k� ba��ml�l�klar ortadan kald�r�lm�� ve esneklik art�r�lm��t�r. Ayr�ca, bu y�ntem birim testlerini kolayla�t�r�r, ��nk� testlerde ger�ek ba��ml�l�klar yerine sahte (mock) nesneler kullan�labilir.

## Performans ve �zlenebilirlik

**Middleware** katman�, performans optimizasyonlar�n� ve hata y�netimini i�erir. `ApiLoggingMiddleware` s�n�f�, API �a�r�lar� i�in ayr�nt�l� g�nl�kler tutarak uygulama izlenebilirli�ini art�r�r. Ayr�ca, `ActionInterceptor` gibi interceptors s�n�flar�, logging ve exception handling gibi ek i�levler sa�lar.

## Veritaban� Tasar�m� ve Migration

Veritaban� tasar�m�, `ECommerceDBContext` s�n�f� arac�l���yla y�netilir. `Migrations` dizininde yer alan migration dosyalar�, uygulama ile veritaban� �emas�n�n senkronizasyonunu sa�lar.

## Mod�lerlik ve Geni�letilebilirlik

Her bir bile�en, ba��ms�z bir �ekilde geli�tirilebilir ve ba�ka sistemlerle kolayca entegre edilebilir. `DTO` ve `Helper` dizinleri, farkl� mod�ller veya uygulamalar aras�nda veri transferini sa�lar. Bu mod�ler yap�, uygulaman�n kolayca geni�letilmesini ve yeni �zelliklerin eklenmesini sa�lar.

## G�venlik ve �zinler

Her API �a�r�s�ndan �nce izinler `PermissionAttribute` s�n�f� arac�l���yla kontrol edilir. Bu, yaln�zca yetkili kullan�c�lar�n belirli kaynaklara eri�mesini sa�lar ve sistemi daha g�venli hale getirir.

## Sonu�

Bu mimari, mod�lerlik, s�rd�r�lebilirlik ve test edilebilirlik gibi temel hedefleri �n planda tutarak, sistemin uzun vadede �l�eklenebilir ve esnek olmas�n� sa�lar. Katmanl� yap� ve ba��ms�z bile�enler, sistemin kolayca g�ncellenmesini ve geni�letilmesini m�mk�n k�lar.
