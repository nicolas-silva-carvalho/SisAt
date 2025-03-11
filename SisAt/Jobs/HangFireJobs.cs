using Hangfire;
using SisAt.Models;
using SisAt.Repository.Persistence.Interfaces;

namespace SisAt.Jobs
{
    public class HangFireJobs
    {
        public static void Jobs()
        {
            RecurringJob.AddOrUpdate<IAgendamentoPersistence>("Agendamentos -> Update agendamentos que não confirmaram.", x => x.AtualizarAgendamentosJobAsync(), Cron.Minutely());
        }
    }
}
