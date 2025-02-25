namespace SisAt.API
{
    public interface IImportacaoAPIService
    {
        Task<LocaisApi> LocaisApiResponse();
        Task<LocaisDadosApi> GetLocalByIdAPIAsync(int localId);
        Task<ServicosApi> ServicosApiResponse();
    }
}
