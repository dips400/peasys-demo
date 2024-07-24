using Peasys;
using peasysdemo.Models;

namespace peasysdemo.Views;

/// <summary>
/// Page permettant d'afficher l'�tat de la configuration et du sous-syst�me Peasys.
/// </summary>
public partial class Configuration : ContentPage
{
    private readonly IDBConnectionService _connectionService;
    /// <summary>
    /// Constructeur de la page. R�cup�re la liste des jobs actifs du sous-syst�me.
    /// </summary>
    public Configuration(IDBConnectionService connectionService)
    {
        InitializeComponent();

        _connectionService = connectionService;

        try
        {
            ServerName.Text = $"Nom du serveur : {_connectionService.Connexion.PartitionName}";
            ServerIp.Text = $"Adress IP : {_connectionService.Connexion.IpAdress}";

            PeaSelectResponse jobResponse = _connectionService.Connexion.ExecuteSelect("SELECT JOB_NAME_SHORT, JOB_USER, JOB_NUMBER, JOB_TYPE, JOB_STATUS, RUN_PRIORITY FROM TABLE (QSYS2.ACTIVE_JOB_INFO(SUBSYSTEM_LIST_FILTER => 'DIPSMG')) X");

            List<Job> jobs = [];
            if (jobResponse.HasSucceeded)
            {
                for (int i = 0; i < jobResponse.RowCount; i++)
                {
                    jobs.Add(new Job
                    {
                        Name = jobResponse.Result["job_name_short"][i],
                        User = jobResponse.Result["job_user"][i],
                        Number = jobResponse.Result["job_number"][i],
                        Status = jobResponse.Result["job_status"][i],
                        Type = jobResponse.Result["job_type"][i],
                        //StartDate = jobResponse.Result["job_name_short"][i], 
                        Priority = jobResponse.Result["run_priority"][i]
                    });
                }
            }
            else
            {
                ErrorMessage.Text = "Unable to find corresponding jobs. Please, report to the administrator.";
                return;
            }

            BindingContext = new JobViewModel(jobs);
        }
        catch (Exception ex)
        {
            ErrorMessage.Text = ex.ToString(); 
        }
    }
}