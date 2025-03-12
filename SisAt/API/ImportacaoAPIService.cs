using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using SisAt.API.ModelsAPI;
using SisAt.Settings;

namespace SisAt.API
{
    public class ImportacaoAPIService : IImportacaoAPIService
    {
        private readonly SGAApiService _sgaService;

        public ImportacaoAPIService(IOptions<SGAApiService> sgaService)
        {
            _sgaService = sgaService.Value;
        }

        public async Task<ServicosApi> ServicosApiResponse()
        {
            string url = _sgaService.UrlServico;
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var servicos = JsonConvert.DeserializeObject<ServicosApi>(response.Content);
                return servicos;
            }
            else
            {
                Console.WriteLine("Erro: " + response.ErrorMessage);
            }

            return null;
        }

        public async Task<ServicosDadosApi> ServicosApiResponse(int servicoId)
        {
            string url = _sgaService.UrlServico;
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var servicos = JsonConvert.DeserializeObject<ServicosApi>(response.Content);

                foreach (var servico in servicos.dados)
                {
                    if(servicoId == servico.id)
                    {
                        return servico;
                    }
                }
            }
            else
            {
                Console.WriteLine("Erro: " + response.ErrorMessage);
            }

            return null;
        }

        public async Task<SenhaApi> CriacaoDeSenha(int servicoId, string nome)
        {
            string url = _sgaService.UrlSenha;
            var client = new RestClient();
            var request = new RestRequest(url, Method.Post);
            request.AddParameter("id_servico", servicoId);
            request.AddParameter("cidadao", nome);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var senhaApi = JsonConvert.DeserializeObject<SenhaApi>(response.Content);
                return senhaApi;
            }

            return null;
        }
    }
}
