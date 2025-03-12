using SisAt.API.ModelsAPI;

namespace SisAt.API
{
    public interface IImportacaoAPIService
    {
        Task<ServicosApi> ServicosApiResponse();
        Task<ServicosDadosApi> ServicosApiResponse(int servicoId);
        Task<SenhaApi> CriacaoDeSenha(int servicoId, string nome);
    }
}
