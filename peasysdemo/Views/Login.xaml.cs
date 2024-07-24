using Peasys;
using peasysdemo.Models;
namespace peasysdemo.Views;

/// <summary>
/// Page d'authentification et de connexion à la base de données.
/// </summary>
public partial class Login : ContentPage
{
    private readonly IDBConnectionService _connectionService;
    /// <summary>
    /// Constructeur de la page.
    /// </summary>
    public Login(IDBConnectionService dBConnectionService)
    {
        InitializeComponent();
        _connectionService = dBConnectionService;
    }

    /// <summary>
    /// Handler permmetant d'effectuer la connexion avec les informations renseignés et de naviguer vers la page des ventes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleLogin(object sender, EventArgs e)
    {
        _connectionService.Connexion = new PeaClient("45.137.144.252", "DIPS01", 8125, "USERDEMO", "pwddemo", "", false, false);

        // Navigate to the main AppShell
        Application.Current.MainPage = new AppShell();
    }
}