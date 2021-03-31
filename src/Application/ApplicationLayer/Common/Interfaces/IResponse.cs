using DomainLayer.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Common.Interfaces
{
	public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse: BaseEntity
	{
		Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
	}
}