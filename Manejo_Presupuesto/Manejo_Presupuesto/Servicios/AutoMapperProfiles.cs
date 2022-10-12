using AutoMapper;
using Manejo_Presupuesto.Models;

namespace Manejo_Presupuesto.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModels>();
            CreateMap<TransaccionActualizacionViewModels, Transaccion>().ReverseMap();
        }
    }
}
