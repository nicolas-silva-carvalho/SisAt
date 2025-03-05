namespace SisAt.API
{
    public interface IImportacaoAPIService
    {
        Task<LocaisApi> LocaisApiResponse();
        Task<LocaisDadosApi> GetLocalByIdAPIAsync(int localId);
        Task<ServicosApi> ServicosApiResponse();
        Task<ServicosDadosApi> ServicosApiResponse(int servicoId);
        Task<SenhaApi> CriacaoDeSenha(int servicoId, string nome);
    }
}
