using AutoMapper;
using SisAt.API;
using SisAt.Models;

namespace SisAt.Helper
{
    public class SISATProfile : Profile
    {
        public SISATProfile()
        {
            CreateMap<UsuarioRequestDto, Usuario>().ReverseMap();
            CreateMap<UsuarioResponseDto, Usuario>().ReverseMap();
            CreateMap<RegistrarUsuarioRequestDto, Usuario>().ReverseMap();
            CreateMap<LocaisViewModel, LocaisDadosApi>().ReverseMap();
            CreateMap<AgendamentoViewModel, Agendamento>().ReverseMap();
            CreateMap<ServicoViewModel, ServicosDadosApi>().ReverseMap();
            CreateMap<CadastroDeHorariosViewModel, CadastroDeHorarios>().ReverseMap();
        }
    }
}
