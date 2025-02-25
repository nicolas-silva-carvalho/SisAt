using Newtonsoft.Json;
using RestSharp;

namespace SisAt.API
{
    public class ImportacaoAPIService : IImportacaoAPIService
    {
        public async Task<LocaisApi> LocaisApiResponse()
        {
            string url = "http://sga2.gnplay.com.br/api-v2/local/pesquisar/?token=6a878fbb0f5bf5747e565fde63a1996202284654&id_unidade=119";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var locais = JsonConvert.DeserializeObject<LocaisApi>(response.Content);
                return locais;
            }
            else
            {
                Console.WriteLine("Erro: " + response.ErrorMessage);
            }

            return null;
        }

        public async Task<LocaisDadosApi> GetLocalByIdAPIAsync(int localId)
        {
            string url = "http://sga2.gnplay.com.br/api-v2/local/pesquisar/?token=6a878fbb0f5bf5747e565fde63a1996202284654&id_unidade=119";
            var client = new RestClient();
            var request = new RestRequest(url, Method.Get);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var locais = JsonConvert.DeserializeObject<LocaisApi>(response.Content);

                foreach (var local in locais.dados)
                {
                    if(localId == local.id)
                    {
                        return local;
                    }
                }
            }
            else
            {
                Console.WriteLine("Erro: " + response.ErrorMessage);
            }

            return null;
        }

        public async Task<ServicosApi> ServicosApiResponse()
        {
            string url = "http://sga2.gnplay.com.br/api-v2/servico/pesquisar/?token=6a878fbb0f5bf5747e565fde63a1996202284654&id_unidade=119";
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
    }
}
