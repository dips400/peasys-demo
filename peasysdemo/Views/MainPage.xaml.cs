using Peasys;
using peasysdemo.Models;

namespace peasysdemo
{
    /// <summary>
    /// Page représentant la liste des clients de l'entreprise.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private ClientsViewModel _clients;
        private ClientsViewModel _filteredClients;

        private readonly IDBConnectionService _connectionService;

        /// <summary>
        /// Constructeur de la page. Récupère la liste des clients dans la base de données.
        /// </summary>
        public MainPage(IDBConnectionService connectionService)
        {
            InitializeComponent();

            _connectionService = connectionService;

            PeaSelectResponse clientsQuery = _connectionService.Connexion.ExecuteSelect($"SELECT * FROM peademo/clients");

            List<Client> clients = new List<Client>();
            for (int i = 0; i < clientsQuery.RowCount; i++)
            {
                clients.Add(new Client()
                {
                    Name = clientsQuery.Result["nom_cli"][i],
                    CodeId = clientsQuery.Result["id_cli"][i],
                    Description = clientsQuery.Result["details"][i],
                    CountryCode = clientsQuery.Result["codepays"][i],
                    Siret = clientsQuery.Result["siret"][i],
                });
            }

            _clients = new ClientsViewModel(clients);
            _filteredClients = new ClientsViewModel(clients);
            BindingContext = _clients;
        }

        /// <summary>
        /// Handler permettant d'afficher le formulaire d'ajout de client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowNewClientForm(object sender, EventArgs e)
        {
            NewClientForm.IsVisible = !NewClientForm.IsVisible;
        }

        /// <summary>
        /// handler permettant d'enregistrer un nouveau client dans la base de données.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewClient(object sender, EventArgs e)
        {
            if(ClientCountryCode.SelectedItem == null)
            {
                ClientCountryCode.SelectedIndex = 0;
            }

            string Name = String.IsNullOrEmpty(ClientName.Text) ? "Client X" : ClientName.Text;
            int CodeId = int.TryParse(ClientCodeId.Text, out int id) ? id : 0;
            string Description = ClientDescription.Text.Replace("!","");
            string CountryCode = ClientCountryCode.SelectedItem.ToString();
            long Siret = long.TryParse(ClientSiret.Text, out _) ? long.Parse(ClientSiret.Text) : 0L;
            PeaInsertResponse peaInsertResponse = _connectionService.Connexion.ExecuteInsert($"INSERT INTO peademo/clients (NOM_CLI, DETAILS, CODEPAYS, SIRET) values ('{Name}', '{Description}', '{CountryCode}', {Siret})");

            _clients.AddClient(new Client
            {
                Name = Name,
                CodeId = CodeId,
                Description = Description,
                CountryCode = CountryCode,
                Siret = Siret
            });

            NewClientForm.IsVisible = false;
        }

        /// <summary>
        /// Handler permettant de supprimer un client de la base de données.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveClient(object sender, EventArgs e)
        {
            MenuFlyoutItem item = (MenuFlyoutItem)sender;

            Client client = (Client)item.BindingContext;

            PeaDeleteResponse peaDeleteResponse = _connectionService.Connexion.ExecuteDelete($"DELETE FROM peademo/clients where ID_CLI = {client.CodeId}");

            _clients.RemoveClient(client);
            NewClientForm.IsVisible = false;
        }

        /// <summary>
        /// Handler permettant d'effectuer une recherche parmis la liste de client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleClientSearch(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchClientName.Text))
            {
                BindingContext = _clients;
            }
            else
            {
                List<Client> c = _clients.GetClients();
                _filteredClients = new ClientsViewModel(c.Where(client => client.Name.IndexOf(SearchClientName.Text, StringComparison.OrdinalIgnoreCase) >= 0).ToList());
                BindingContext = _filteredClients;
            }
        }
    }
}
