using CommunityToolkit.Maui.Storage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace peasysdemo.Models
{
    /// <summary>
    /// Class représentant le ModelView des ventes de l'entreprise.
    /// </summary>
    public class SalesViewModel
    {
        /// <summary>
        /// La liste observable des ventes.
        /// </summary>
        public ObservableCollection<Sale> Sales { get; set; }

        /// <summary>
        /// Méthode utilitaire permettant l'export PDF de la vente.
        /// </summary>
        public ICommand ExportPDF { get; }

        /// <summary>
        /// Constructeur de l'objet. Utilise une liste de ventes pour construire la vue.
        /// </summary>
        /// <param name="sales">La liste de ventes de l'entreprise.</param>
        public SalesViewModel(List<Sale> sales)
        {
            // Initialize the collection of sales data
            Sales = new ObservableCollection<Sale>(sales);
            ExportPDF = new Command<Sale>(ExportClientSalesInPDF);
        }

        /// <summary>
        /// Méthode permettant l'export PDF d'une vente.
        /// </summary>
        /// <param name="sale">La vente à exporter en PDF.</param>
        private async void ExportClientSalesInPDF(Sale sale)
        {
            MemoryStream workStream = new();
            Document document = new();

            document.SetPageSize(PageSize.A4);

            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.Open();

            // Add logo
            if (File.Exists("peasysdemo.Resources.logo.png"))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("peasysdemo.Resources.logo.png");
                logo.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                logo.ScaleToFit(200f, 200f); // Adjust width and height as needed
                document.Add(logo);
            }

            // Add title
            iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, 36, BaseColor.BLACK);
            Paragraph title = new Paragraph("Invoice", titleFont);
            title.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            title.SpacingAfter = 100;
            document.Add(title);

            // Add invoice details
            iTextSharp.text.Font detailsFont = FontFactory.GetFont(FontFactory.COURIER, 12, BaseColor.BLACK);
            PdfPTable detailsTable = new PdfPTable(2);
            detailsTable.WidthPercentage = 100;
            detailsTable.AddCell(GetCell("Invoice Number: 123456", detailsFont, PdfPCell.NO_BORDER));
            detailsTable.AddCell(GetCell("Invoice Date:" + sale.Date, detailsFont, PdfPCell.NO_BORDER));
            detailsTable.AddCell(GetCell("Due Date:" + sale.Date.AddDays(30).ToShortDateString(), detailsFont, PdfPCell.NO_BORDER));
            detailsTable.SpacingAfter = 20;
            document.Add(detailsTable);

            // Add client information
            iTextSharp.text.Font clientFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, 16, BaseColor.BLACK);
            Paragraph clientTitle = new Paragraph("Client Information", clientFont);
            clientTitle.SpacingAfter = 10;
            document.Add(clientTitle);
            document.Add(new Paragraph($"Name: {sale.Client}", detailsFont));
            document.Add(new Paragraph("Address: 123 Main Street", detailsFont));

            // Add itemized list
            PdfPTable table = new PdfPTable(4); // 4 columns
            table.WidthPercentage = 80;
            table.AddCell(GetHeaderCell("Item", detailsFont));
            table.AddCell(GetHeaderCell("Quantity", detailsFont));
            table.AddCell(GetHeaderCell("Unit Price", detailsFont));
            table.AddCell(GetHeaderCell("Total", detailsFont));

            table.AddCell(GetCell("Item 1", detailsFont, 1));
            table.AddCell(GetCell("1", detailsFont, 1));
            table.AddCell(GetCell(sale.Amount.ToString(), detailsFont, 1));
            table.AddCell(GetCell(sale.Amount.ToString(), detailsFont, 1));

            table.SpacingAfter = 20;
            table.SpacingBefore = 50;
            document.Add(table);

            // Add total amount
            iTextSharp.text.Font totalFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, 22, BaseColor.BLACK);
            Paragraph total = new Paragraph($"Total: {sale.Amount}", totalFont);
            total.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            document.Add(total);

            document.Close();

            var fileSaverResult = await FileSaver.Default.SaveAsync($"Facture_985423_{sale.Client}.pdf", workStream);

            //Sales.Add(new Sale() { Client = "azeazr", Date = new DateTime(), Amount = 100m }); // just for testing
        }

        // Méthode utilitaire permettant de construire le PDF.
        private static PdfPCell GetCell(string title, iTextSharp.text.Font font, int border)
        {
            PdfPCell cell = new PdfPCell(new Phrase(title, font));
            if (border == 0)
            {
                cell.Border = border;
            }
            cell.PaddingBottom = 5;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            cell.PaddingBottom = 5;
            cell.PaddingRight = 20;

            return cell;
        }

        // Méthode utilitaire permettant de construire le PDF.
        private static PdfPCell GetHeaderCell(string title, iTextSharp.text.Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(title, font));
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.PaddingBottom = 5;
            cell.PaddingTop = 5;
            return cell;
        }
    }

    /// <summary>
    /// Objet représentant une vente pour l'entreprise.
    /// </summary>
    public class Sale
    {
        public string Client { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}
