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

		private static async Task Main()
		{
			_source = new();
			_thing = new(_source.Token);
			_governor = new(985573019084804166);
			var exit = _thing.MyTask.ContinueWith(x => _governor.Shutdown()).Unwrap();
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
			var bg = new Icon("https://llvm.org/img/DragonMedium.png", IconKind.MediaProxy);
			var ico = new Icon("https://pngimg.com/uploads/github/github_PNG40.png", IconKind.MediaProxy, "https://github.com/Darxxemiyur/PersonalDiscordRichPresenceStatus");
			var record1 = new StatusRecord(TimeSpan.FromSeconds(10), "You are getting angry because you haven't reached your goal yet. In the end that'll be why you'll reach it", null, ico, bg);
			var block = new Block(record1);
			var doc = new Document(Enumerable.Range(1, 50).Select(x => (IStatusYieldable)new YieldablePointer(block)).Prepend(block));
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