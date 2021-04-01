using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Common.Interfaces
{
	public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse: class
	{
		Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
	}
}