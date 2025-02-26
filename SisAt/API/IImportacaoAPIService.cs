namespace SisAt.API
{
    public interface IImportacaoAPIService
    {
        Task<LocaisApi> LocaisApiResponse();
        Task<LocaisDadosApi> GetLocalByIdAPIAsync(int localId);
        Task<ServicosApi> ServicosApiResponse();
        Task<SenhaApi> CriacaoDeSenha(int servicoId, string nome);
    }
}
