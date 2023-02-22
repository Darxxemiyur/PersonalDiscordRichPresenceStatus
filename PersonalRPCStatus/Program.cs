using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure;

using System.Linq;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus
{
	public class Program
	{
		private static DiscordGovernor _governor;
		private static MyTaskSource _thing;
		private static CancellationTokenSource _source;
		private const ulong defID = 985573019084804166;

		private static async Task Main()
		{
			_source = new();
			_thing = new(_source.Token);
			_governor = new(defID);
			var exit = _thing.MyTask.ContinueWith(x => _governor.Shutdown()).Unwrap();
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

			var doc = Document.Empty;
			await _thing.MimicResult(WorkRecords(_governor, doc));
			await exit;
		}

		private static async Task WorkRecords(DiscordGovernor governor, Document yieldable)
		{
			await foreach (var record in yieldable.UnrollRecords())
				await governor.PassNext(record);
		}

		private static void OnProcessExit(object sender, EventArgs e)
		{
			_source.Cancel();
			_thing.MyTask.Wait();
		}
	}
}