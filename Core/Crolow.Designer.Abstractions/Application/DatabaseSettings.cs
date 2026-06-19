namespace Crolow.Designer.Common.Application;

public class DatabaseSettings
{
    public string CurrentDatabase { get; set; } = string.Empty;
    public List<DatabaseSetting> Databases { get; set; } = [];
}
