using peasysdemo.Views;

namespace peasysdemo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Login", typeof(Login));
            Routing.RegisterRoute("Clients", typeof(MainPage));
            Routing.RegisterRoute("Sales", typeof(Sales));
            Routing.RegisterRoute("Configuration", typeof(Configuration));
        }
    }
}
