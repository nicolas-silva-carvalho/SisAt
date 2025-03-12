using AutoMapper;
using SisAt.API.ModelsAPI;
using SisAt.Models;
using SisAt.Models.ViewModel;

namespace SisAt.Helper
{
    public class SISATProfile : Profile
    {
        public SISATProfile()
        {
            CreateMap<UsuarioRequestDto, Usuario>().ReverseMap();
            CreateMap<UsuarioResponseDto, Usuario>().ReverseMap();
            CreateMap<RegistrarUsuarioRequestDto, Usuario>().ReverseMap();
            CreateMap<AgendamentoViewModel, Agendamento>().ReverseMap();
            CreateMap<ServicoViewModel, ServicosDadosApi>().ReverseMap();
            CreateMap<CadastroDeHorariosViewModel, CadastroDeHorarios>().ReverseMap();
            CreateMap<SenhaDadosApi, Models.ViewModel.Senha>().ReverseMap();
            CreateMap<SenhaViewlModel, Models.ViewModel.Senha>().ReverseMap();
            CreateMap<AgendamentoPesquisa, Agendamento>().ReverseMap();
            CreateMap<CalendarioViewlModel, Calendario>().ReverseMap();
        }
    }
}
