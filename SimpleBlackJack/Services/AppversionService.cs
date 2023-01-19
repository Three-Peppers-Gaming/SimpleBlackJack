using System.Reflection;


namespace SimpleBlackJack.Services
{
    public class AppVersionService : IAppVersionService
    {
        string IAppVersionService.Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

    }
}
