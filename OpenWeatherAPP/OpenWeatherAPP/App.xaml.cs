using System.Net;

namespace OpenWeatherAPP
{
    public partial class App : Application
    {
        public App()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
