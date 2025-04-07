using MediatR;
using AppointmentSystem.Dtos.Admin;
namespace AppointmentSystem.Handlers.Admin.Query
{
    public class GetAdminRequest : IRequest<List<AdminResponseDto>> { }
}
