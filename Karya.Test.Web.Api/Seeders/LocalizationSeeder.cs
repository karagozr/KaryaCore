using Karya.Core.Results;
using Karya.Test.Web.Api.Data;
using Karya.Test.Web.Api.Localization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Seeders;

/// <summary>
/// Seeds the default translations (tr/en) for the known message codes.
/// Runs only when the table is empty, so it is safe to call on every startup.
/// </summary>
public static class LocalizationSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<AppDbContext>();

        if (await db.LocalizationResources.AnyAsync())
            return;

        var rows = new List<LocalizationResource>();

        void Add(string code, string tr, string en)
        {
            rows.Add(new LocalizationResource { Code = code, LanguageCode = "tr", Value = tr });
            rows.Add(new LocalizationResource { Code = code, LanguageCode = "en", Value = en });
        }

        Add(MessageCodes.Success, "İşlem başarılı.", "Operation completed successfully.");
        Add(MessageCodes.Created, "Kayıt başarıyla oluşturuldu.", "Record created successfully.");
        Add(MessageCodes.Updated, "Kayıt başarıyla güncellendi.", "Record updated successfully.");
        Add(MessageCodes.Deleted, "Kayıt başarıyla silindi.", "Record deleted successfully.");
        Add(MessageCodes.NotFound, "[{0}] içinde {1} = {2} değeri bulunamadı.", "Value {1} = {2} could not be found in [{0}].");
        Add(MessageCodes.Required, "[{0}] alanı zorunludur.", "[{0}] is required.");
        Add(MessageCodes.ValidationError, "Doğrulama hatası.", "Validation error.");
        Add(MessageCodes.Unauthorized, "Bu işlem için yetkiniz yok.", "You are not authorized for this operation.");
        Add(MessageCodes.ServerError, "Beklenmeyen bir sunucu hatası oluştu.", "An unexpected server error occurred.");
        Add(MessageCodes.DbError, "Bir veritabanı hatası oluştu.", "A database error occurred.");
        Add(MessageCodes.DbConnectionError, "Veritabanına bağlanılamadı.", "Could not connect to the database.");
        Add(MessageCodes.DbDuplicate, "Aynı değere sahip bir kayıt zaten mevcut.", "A record with the same value already exists.");
        Add(MessageCodes.DbConstraint, "İlişkili veriler nedeniyle işlem tamamlanamadı.", "The operation could not be completed due to related data.");
        Add(MessageCodes.DbDeadlock, "İşlem kilitlendi, lütfen tekrar deneyin.", "The operation was deadlocked, please try again.");
        Add(MessageCodes.DbLoginFailed, "Veritabanı oturum açma başarısız oldu.", "Database login failed.");
        Add(MessageCodes.DbCannotOpen, "Veritabanı açılamadı.", "The database could not be opened.");

        await db.LocalizationResources.AddRangeAsync(rows);
        await db.SaveChangesAsync();
    }
}
