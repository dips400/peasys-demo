using Microcharts;
using Peasys;
using peasysdemo.Models;
using SkiaSharp;

namespace peasysdemo.Views;

/// <summary>
/// Page représentant le tableau de bord des salesQuery de l'entreprise.
/// </summary>
public partial class Sales : ContentPage
{
    private static readonly SKColor[] COLORS =
    [
        SKColor.Parse("#2c3e50"),
        SKColor.Parse("#77d065"),
        SKColor.Parse("#b455b6"),
        SKColor.Parse("#3498db"),
        SKColor.Parse("#3ab8db"),
    ];

    private readonly IDBConnectionService _connectionService;

    public Sales(IDBConnectionService connectionService)
    {

        InitializeComponent();
        _connectionService = connectionService;

        PeaSelectResponse salesQuery = _connectionService.Connexion.ExecuteSelect("SELECT * FROM PEADEMO/VENTES");

        List<Sale> sales = new List<Sale>();
        for (int i = 0; i < salesQuery.RowCount; i++)
        {
            sales.Add(new Sale()
            {
                Client = salesQuery.Result["socnom"][i].Trim(),
                Date = salesQuery.Result["date_vente"][i],
                Amount = salesQuery.Result["montant"][i]
            });
        }

        var rand = new Random();
        var groupedSales = sales.GroupBy(sale => sale.Client);
        var entriesDonutChart = groupedSales.Select((group, index) => new ChartEntry(group.Select(s => s.Amount).Sum())
        {
            Label = group.First().Client,
            ValueLabel = group.Select(s => s.Amount).Sum().ToString() + "€",
            Color = COLORS[index % COLORS.Length],
        }).OrderByDescending(e => e.Value).Take(5);
        piechart.Chart = new DonutChart
        {
            Entries = entriesDonutChart,
            BackgroundColor = SKColors.WhiteSmoke
        };

        var entriesLineChart = sales.GroupBy(
            sale1 => sale1.Date.Year,
            sale1 => sale1,
            (baseSale, sales1) =>
                sales1.GroupBy(
                    sale => sale.Date.Month,
                    sale => sale,
                    (baseSale, sales) =>
                    new ChartEntry(sales.Select(s => s.Amount).Sum())
                    {
                        Label = sales.First().Date.Month.ToString() + "/" + sales.First().Date.Year.ToString(),
                        ValueLabel = sales.Select(s => s.Amount).Sum().ToString() + "€",
                        Color = COLORS[0]
                    }
                )
        ).SelectMany(i => i);
        linechart.Chart = new LineChart
        {
            Entries = entriesLineChart,
            BackgroundColor = SKColors.WhiteSmoke
        };


        PeaSelectResponse productsQuery = _connectionService.Connexion.ExecuteSelect("SELECT * FROM peademo.produits");
        List<Product> products = new();
        for (int i = 0; i < productsQuery.RowCount; i++)
        {
            products.Add(new Product()
            {
                Id = productsQuery.Result["id_prod"][i],
                Name = productsQuery.Result["nom"][i],
                Category = productsQuery.Result["categorie"][i].Trim(),
                Stock = productsQuery.Result["stock"][i],
                EAN = productsQuery.Result["ean"][i],
                QuantityAsked = productsQuery.Result["qte_commande"][i],
                Sales = productsQuery.Result["ventes"][i],
                Supplier = productsQuery.Result["fournisseurs"][i],
            });
        }

        var groupedProducts = products.GroupBy(product => product.Category);

        var entriesRadialChart = groupedProducts.Select((group, index) => new ChartEntry(group.Select(p => p.Sales).Sum())
        {
            Label = group.First().Name,
            ValueLabel = group.Select(s => s.Sales).Sum().ToString(),
            Color = COLORS[index % COLORS.Length]
        }
        ).Take(5);
        entriesRadialChart = entriesRadialChart.Prepend(new ChartEntry(products.Select(p => p.Sales).Sum())
        {
            Label = "Total",
            ValueLabel = products.Select(p => p.Sales).Sum().ToString(),
            Color = COLORS[0]
        });

        chartView.Chart = new RadialGaugeChart()
        {
            Entries = entriesRadialChart,
            BackgroundColor = SKColors.WhiteSmoke
        };

        BindingContext = new SalesViewModel(sales);
    }
}