using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus
{
	public class Program
	{
		private static DiscordGovernor _governor;
		private static MyTaskSource _thing;
		private static CancellationTokenSource _source;

		private static async Task Main()
		{
			_thing = new();
			_source = new();
			_governor = new();
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
			try
			{
				await WorkRecords(_governor, null);
			}
			catch (TaskCanceledException)
			{
				await _governor.Shutdown();
				await _thing.TrySetResultAsync();
			}
		}

		private static async Task WorkRecords(DiscordGovernor governor, IStatusYieldable yieldable)
		{
			await foreach (var record in await yieldable.UnrollRecords())
				await governor.PassNext(record);
		}

		private static void OnProcessExit(object sender, EventArgs e)
		{
			_source.Cancel();
			_thing.MyTask.Wait();
		}
	}
}