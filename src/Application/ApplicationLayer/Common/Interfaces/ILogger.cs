using System;

namespace ApplicationLayer.Common.Interfaces
{
	public interface ILogger
	{
		System.Threading.Tasks.Task InfoAsync(string msg);
		System.Threading.Tasks.Task ErrorAsync(string msg);
		System.Threading.Tasks.Task ErrorAsync(Exception e);
		System.Threading.Tasks.Task WarningAsync(string msg);
		System.Threading.Tasks.Task SuccessAsync(string msg);
	}
}
