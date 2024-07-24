using ClosedXML.Excel;
using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace peasysdemo.Models
{
    /// <summary>
    /// Class représentant le ViewModel des clients de l'entrprise.
    /// </summary>
    public class ClientsViewModel
    {
        /// <summary>
        /// Liste observable des clients
        /// </summary>
        public ObservableCollection<Client> Clients { get; set; }

        /// <summary>
        /// Methode mise à disposition pour exporter le client sous excel.
        /// </summary>
        public ICommand ExportExcel { get; }

        /// <summary>
        /// Methode mise à disposition pour filtrer la liste des clients.
        /// </summary>
        public ICommand FilterData { get; }

        /// <summary>
        /// Constructeur du modèle.
        /// </summary>
        /// <param name="clients">La liste des clients à afficher.</param>
        public ClientsViewModel(List<Client> clients)
        {
            // Initialize the collection of sales data
            Clients = new ObservableCollection<Client>(clients);
            ExportExcel = new Command(ExportClientsInExcel);
            FilterData = new Command<string>(HandleFiltering);
        }

        /// <summary>
        /// Méthode permettant d'ajouter un client dans la liste et dans la base de données.
        /// </summary>
        /// <param name="client">Le client à ajouter.</param>
        public void AddClient(Client client)
        {
            Clients.Add(client);
        }

        /// <summary>
        /// Méthode permettant d'enlever un client dans la liste et de la base de données.
        /// </summary>
        /// <param name="client">Le client à supprimer.</param>
        public void RemoveClient(Client client)
        {
            Clients.Remove(client);
        }

        /// <summary>
        /// Méthode utilitaire permettant d'avoir la liste des clients.
        /// </summary>
        /// <returns>LA liste des clients.</returns>
        public List<Client> GetClients()
        {
            return [.. Clients];
        }

        /// <summary>
        /// Méthode qui permet l'export sous excel
        /// </summary>
        private async void ExportClientsInExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Clients");

                // header
                worksheet.Cell("A1").Value = "Société";
                worksheet.Cell("B1").Value = "Code ID";
                worksheet.Cell("C1").Value = "Description";
                worksheet.Cell("D1").Value = "Code Pays";
                worksheet.Cell("F1").Value = "Siret";

                // data
                foreach (var (value, i) in Clients.Select((value, i) => (value, i)))
                {
                    worksheet.Cell($"A{i + 2}").Value = value.Name;
                    worksheet.Cell($"B{i + 2}").Value = value.CodeId;
                    worksheet.Cell($"C{i + 2}").Value = value.Description;
                    worksheet.Cell($"D{i + 2}").Value = value.CountryCode;
                    worksheet.Cell($"F{i + 2}").Value = value.Siret;
                }
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var fileSaverResult = await FileSaver.Default.SaveAsync("Clients.xlsx", stream);
            }
        }

        /// <summary>
        /// Méthode qui permet de filtrer la liste des clients.
        /// </summary>
        /// <param name="label">Le filtre.</param>
        private void HandleFiltering(string label)
        {
            var sortedClients = Clients.ToList();

            if (label == "Code ID")
            {
                sortedClients = Clients.OrderBy(c => c.CodeId).ToList();
            }
            else if (label == "Description")
            {
                sortedClients = Clients.OrderBy(c => c.Description).ToList();
            }
            else if (label == "Code Pays")
            {
                sortedClients = Clients.OrderBy(c => c.CountryCode).ToList();
            }
            else
            {
                return;
            }

            Clients.Clear();
            foreach (Client item in sortedClients)
            {
                Clients.Add(item);
            }
        }
    }

    /// <summary>
    /// Objet représentant un client.
    /// </summary>
    public class Client
    {
        public string Name { get; set; }
        public int CodeId { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public long Siret { get; set; }
    }
}
