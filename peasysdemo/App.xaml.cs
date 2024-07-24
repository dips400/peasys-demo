using peasysdemo.Views;

namespace peasysdemo
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = new NavigationPage(serviceProvider.GetRequiredService<Login>());//new Login(new DBConnexionService());
        }
    }
}
