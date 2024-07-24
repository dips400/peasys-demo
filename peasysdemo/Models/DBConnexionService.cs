using Peasys;

namespace peasysdemo.Models
{
    public interface IDBConnectionService
    {
        PeaClient Connexion { get; set; }
    }

    /// <summary>
    /// Objet permettant de manager la connexion avec la base de données
    /// </summary>
    public class DBConnexionService : IDBConnectionService
    {
        public PeaClient Connexion { get; set; }
    }
}
