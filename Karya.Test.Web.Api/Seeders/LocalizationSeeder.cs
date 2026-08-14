using Karya.Core.Indentity.Infrastructure.Migrations;
using Karya.Core.Results;
using Karya.Test.Web.Api.Data;
using Karya.Test.Web.Api.Localization;
using Microsoft.EntityFrameworkCore;

namespace Karya.Test.Web.Api.Seeders;

public sealed class LocalizationSeeder : IDatabaseSeeder
{
    private readonly AppDbContext _db;

    public LocalizationSeeder(AppDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (await _db.LocalizationResources.AnyAsync())
            return;

        var turkish = new Dictionary<string, string>
        {
            [MessageCodes.Success] = "İşlem başarılı.",
            [MessageCodes.Created] = "Kayıt başarıyla oluşturuldu.",
            [MessageCodes.Updated] = "Kayıt başarıyla güncellendi.",
            [MessageCodes.Deleted] = "Kayıt başarıyla silindi.",
            [MessageCodes.NotFound] = "[{0}] içinde {1} = {2} değeri bulunamadı.",
            [MessageCodes.Required] = "[{0}] alanı zorunludur.",
            [MessageCodes.ValidationError] = "Doğrulama hatası.",
            [MessageCodes.Unauthorized] = "Bu işlem için yetkiniz yok.",
            [MessageCodes.ServerError] = "Beklenmeyen bir sunucu hatası oluştu.",
            [MessageCodes.DbError] = "Bir veritabanı hatası oluştu.",
            [MessageCodes.DbConnectionError] = "Veritabanına bağlanılamadı.",
            [MessageCodes.DbDuplicate] = "Aynı değere sahip bir kayıt zaten mevcut.",
            [MessageCodes.DbConstraint] = "İlişkili veriler nedeniyle işlem tamamlanamadı.",
            [MessageCodes.DbDeadlock] = "İşlem kilitlendi, lütfen tekrar deneyin.",
            [MessageCodes.DbLoginFailed] = "Veritabanı oturum açma başarısız oldu.",
            [MessageCodes.DbCannotOpen] = "Veritabanı açılamadı."
        };

        var rows = new List<LocalizationResource>();

        foreach (var message in MessageCodes.English)
        {
            rows.Add(new LocalizationResource
            {
                Code = message.Key,
                LanguageCode = "en",
                Value = message.Value,
                Scope = LocalizationScope.Server
            });

            if (turkish.TryGetValue(message.Key, out var tr))
            {
                rows.Add(new LocalizationResource
                {
                    Code = message.Key,
                    LanguageCode = "tr",
                    Value = tr,
                    Scope = LocalizationScope.Server
                });
            }
        }

        rows.Add(new LocalizationResource { Code = "UI_SAVE", LanguageCode = "tr", Value = "Kaydet", Scope = LocalizationScope.Client });
        rows.Add(new LocalizationResource { Code = "UI_SAVE", LanguageCode = "en", Value = "Save", Scope = LocalizationScope.Client });
        rows.Add(new LocalizationResource { Code = "UI_CANCEL", LanguageCode = "tr", Value = "İptal", Scope = LocalizationScope.Client });
        rows.Add(new LocalizationResource { Code = "UI_CANCEL", LanguageCode = "en", Value = "Cancel", Scope = LocalizationScope.Client });

        await _db.LocalizationResources.AddRangeAsync(rows);
        await _db.SaveChangesAsync();
    }
}
