using Karya.Core.Interfaces.DTOs;

namespace Karya.Core.Helpers.Generals;

public static class DtoControl
{
    public static string[] GetActiveKeys<TDto>(TDto dto) where TDto : class,IBaseDto, new()
    {
        string[] cols = dto.GetType().GetProperties().Where(x => x.GetValue(dto) != null).Select(x => x.Name).ToArray();
        return cols;
    }
}
