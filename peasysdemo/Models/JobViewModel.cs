using System.Collections.ObjectModel;

namespace peasysdemo.Models
{
    /// <summary>
    /// Class représentant le ModelView de la liste des jobs IBMi du sous-système Peasys.
    /// </summary>
    class JobViewModel
    {
        /// <summary>
        /// Liste observable des jobs.
        /// </summary>
        public ObservableCollection<Job> Jobs { get; set; }

        /// <summary>
        /// Constructeur de l'objet qui utilise une liste de jobs pour instancier l'objet.
        /// </summary>
        /// <param name="jobs">La liste de jobs.</param>
        public JobViewModel(List<Job> jobs)
        {
            Jobs = new ObservableCollection<Job>(jobs);
        }
    }

    /// <summary>
    /// Objet représentant un job IBMi
    /// </summary>
    public class Job
    {
        public string Name { get; set; }
        public string User { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public int Priority { get; set; }
    }
}
