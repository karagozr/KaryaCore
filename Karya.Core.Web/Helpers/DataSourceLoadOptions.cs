using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Karya.Core.Web.Helpers;

[ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
public class DataSourceLoadOptions : DataSourceLoadOptionsBase
{
}

public class DataSourceLoadOptionsBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var loadOptions = new DataSourceLoadOptions();

        // 1. Standart alanları (skip, take, filter, sort vb.) doldur
        DataSourceLoadOptionsParser.Parse(loadOptions, key =>
            bindingContext.ValueProvider.GetValue(key).FirstOrDefault()
        );

        // 2. TotalSummary alanını manuel olarak JSON'dan çöz (Deserialize)
        var totalSummaryValue = bindingContext.ValueProvider.GetValue("totalSummary").FirstOrDefault();
        if (!string.IsNullOrEmpty(totalSummaryValue))
        {
            try
            {
                // DevExtreme internal SummaryInfo tipini kullanarak serileştiriyoruz
                loadOptions.TotalSummary = JsonSerializer.Deserialize<SummaryInfo[]>(
                    totalSummaryValue,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch
            {
                // Hatalı JSON formatı gelirse sessizce geçebilir veya modelstate'e hata ekleyebilirsiniz
            }
        }

        // 3. (İhtiyaç varsa) GroupSummary alanını da aynı şekilde çözebilirsiniz
        var groupSummaryValue = bindingContext.ValueProvider.GetValue("groupSummary").FirstOrDefault();
        if (!string.IsNullOrEmpty(groupSummaryValue))
        {
            try
            {
                loadOptions.GroupSummary = JsonSerializer.Deserialize<SummaryInfo[]>(
                    groupSummaryValue,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch { }
        }

        bindingContext.Result = ModelBindingResult.Success(loadOptions);
        return Task.CompletedTask;
    }

}