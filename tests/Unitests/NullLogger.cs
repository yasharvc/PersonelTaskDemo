using ApplicationLayer.Common.Interfaces;
using System;


namespace Unitests
{
	class NullLogger : ILogger
	{
		public System.Threading.Tasks.Task ErrorAsync(string msg) => System.Threading.Tasks.Task.CompletedTask;

		public System.Threading.Tasks.Task ErrorAsync(Exception e) => System.Threading.Tasks.Task.CompletedTask;

		public System.Threading.Tasks.Task InfoAsync(string msg) => System.Threading.Tasks.Task.CompletedTask;

		public System.Threading.Tasks.Task SuccessAsync(string msg) => System.Threading.Tasks.Task.CompletedTask;

		public System.Threading.Tasks.Task WarningAsync(string msg) => System.Threading.Tasks.Task.CompletedTask;
	}
}