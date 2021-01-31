namespace Core.Configuration.Interfaces
{
    public interface ISystemConfiguration
    {
        T GetConfigurationValue<T>(string sectionKey, string valueKey, T defaultValue);
    }
}