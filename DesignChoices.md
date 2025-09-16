# Tasarým Kararlarý

## Giriþ
Bu mimari, endüstri standartlarýna dayalý temiz, sürdürülebilir ve modüler bir yapý saðlamak amacýyla tasarlanmýþtýr.
Tasarýmýn ana hedefi, ölçeklenebilirlik, test edilebilirlik ve bakým kolaylýðý saðlamaktýr.
Aþaðýda, bu hedeflere ulaþmak için alýnan temel mimari kararlar açýklanmýþtýr.

## Katmanlý Mimari

Mimari, her biri belirli bir görevi üstlenen dört ana katmana ayrýlmýþtýr:

1. **ECommerce.API (API Katmaný)**:  
   Bu katman, tüm dýþ etkileþimleri (istemci API çaðrýlarý) yönetir. `Controllers` dizini, belirli alanlarla (örneðin kategori, ürün, kullanýcý) ilgili iþlemleri yöneten çeþitli controller'larý içerir. Ayrýca, hata yönetimi ve izin kontrolleri de bu katmanda yapýlýr.

2. **ECommerce.Application (Uygulama Katmaný)**:  
   Bu katman, iþ mantýðýný ve uygulama seviyesindeki servisleri içerir. `DTO` ve `Services` dizinlerinde, verilerin taþýnmasý ve iþlenmesinden sorumlu sýnýflar yer alýr. Domain'e ait doðrulama iþlemleri `Validations` klasöründe bulunur.

3. **ECommerce.Domain (Domain Katmaný)**:  
   Domain katmaný, uygulamanýn temel iþ yapýsýný ve iþ kurallarýný içerir. `Entities` dizini, veritabaný modellerini ve iþ nesnelerini barýndýrýr. Bu katman, diðer katmanlardan baðýmsýz olup domain mantýðýnýn baðýmsýz bir þekilde geliþtirilmesine olanak tanýr.

4. **ECommerce.Infrastructure (Altyapý Katmaný)**:  
   Bu katman, uygulamanýn altyapýsýný ve dýþ baðýmlýlýklarýný yönetir. `Context`, `Interceptors`, `Migrations` ve `Repositories` dizinleri, veritabaný etkileþimleri, veritabaný geçiþleri ve diðer altyapý bileþenlerini içerir. `UnitOfWork` ve `Repository` desenleri, veri eriþimini soyutlayarak birim testlerinin yapýlmasýný kolaylaþtýrýr.

## Sorumluluk Ayrýmý ve Katmanlar Arasý Ýletiþim

Her katman, yalnýzca kendisine ait sorumluluða sahip olup katmanlar arasýndaki iletiþim minimum düzeyde tutulmuþtur. Katmanlar, arabirimler ve servisler aracýlýðýyla etkileþimde bulunur, böylece gevþek baðlanabilirlik (loose coupling) ve yüksek tutarlýlýk saðlanýr. Bu yaklaþým, bakým kolaylýðý saðlar ve sorumluluklarýn doðru þekilde ayrýlmasýný garanti eder.

## SOLID Prensiplerine Uyulmasý

Tüm sýnýflar, **Single Responsibility Principle (SRP)** prensibine uygun olarak tasarlanmýþtýr. Her sýnýf, tek bir iþlevi yerine getirir, bu da kodun okunabilirliðini, sürdürülebilirliðini ve test edilebilirliðini artýrýr.

- Örneðin, `UserService` yalnýzca kullanýcý yönetimi ile ilgilenirken, `CategoryService` sadece kategori iþlemleriyle ilgilenir. Bu þekilde her bileþen, tek bir sorumluluða sahiptir.
  
Diðer SOLID prensiplerinin kullanýmý, mimarinin esnek, kolayca deðiþtirilebilir ve deðiþikliklere dirençli olmasýný saðlar.

## Baðýmlýlýk Enjeksiyonu ve Test Edilebilirlik

Tüm baðýmlýlýklar **Dependency Injection (DI)** kullanýlarak enjekte edilmiþtir. Bu sayede bileþenler arasýndaki sýký baðýmlýlýklar ortadan kaldýrýlmýþ ve esneklik artýrýlmýþtýr. Ayrýca, bu yöntem birim testlerini kolaylaþtýrýr, çünkü testlerde gerçek baðýmlýlýklar yerine sahte (mock) nesneler kullanýlabilir.

## Performans ve Ýzlenebilirlik

**Middleware** katmaný, performans optimizasyonlarýný ve hata yönetimini içerir. `ApiLoggingMiddleware` sýnýfý, API çaðrýlarý için ayrýntýlý günlükler tutarak uygulama izlenebilirliðini artýrýr. Ayrýca, `ActionInterceptor` gibi interceptors sýnýflarý, logging ve exception handling gibi ek iþlevler saðlar.

## Veritabaný Tasarýmý ve Migration

Veritabaný tasarýmý, `ECommerceDBContext` sýnýfý aracýlýðýyla yönetilir. `Migrations` dizininde yer alan migration dosyalarý, uygulama ile veritabaný þemasýnýn senkronizasyonunu saðlar.

## Modülerlik ve Geniþletilebilirlik

Her bir bileþen, baðýmsýz bir þekilde geliþtirilebilir ve baþka sistemlerle kolayca entegre edilebilir. `DTO` ve `Helper` dizinleri, farklý modüller veya uygulamalar arasýnda veri transferini saðlar. Bu modüler yapý, uygulamanýn kolayca geniþletilmesini ve yeni özelliklerin eklenmesini saðlar.

## Güvenlik ve Ýzinler

Her API çaðrýsýndan önce izinler `PermissionAttribute` sýnýfý aracýlýðýyla kontrol edilir. Bu, yalnýzca yetkili kullanýcýlarýn belirli kaynaklara eriþmesini saðlar ve sistemi daha güvenli hale getirir.

## Sonuç

Bu mimari, modülerlik, sürdürülebilirlik ve test edilebilirlik gibi temel hedefleri ön planda tutarak, sistemin uzun vadede ölçeklenebilir ve esnek olmasýný saðlar. Katmanlý yapý ve baðýmsýz bileþenler, sistemin kolayca güncellenmesini ve geniþletilmesini mümkün kýlar.
