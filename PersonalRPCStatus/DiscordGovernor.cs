using DiscordRPC;

using Name.Bayfaderix.Darxxemiyur.Common;
using Name.Bayfaderix.Darxxemiyur.Common.Extensions;
using Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus
{
	/// <summary>
	/// Controls what to display, in what context.
	/// </summary>
	internal class DiscordGovernor
	{
		private DiscordRpcClient? _link;
		private readonly CancellationTokenSource _source;
		private Task _waiterTask;
		private readonly AsyncLocker _lock;
		private readonly ulong _defaultId;

		internal DiscordGovernor(ulong defaultId)
		{
			_defaultId = defaultId;
			_lock = new();
			_source = new();
			_waiterTask = Task.CompletedTask;
		}

		private async Task PrivateShutdown()
		{
			if (_link == null)
				return;

			await MyTaskExtensions.RunOnScheduler(_link.Deinitialize);
			await MyTaskExtensions.RunOnScheduler(_link.Dispose);
			_link = null;
		}

		private async Task UpdateIfNeeded(string? id)
		{
			if (_link == null && id == null)
				id = $"{_defaultId}";

			if (_link?.ApplicationID == id || id == null)
				return;

			await PrivateShutdown();

			_link = new(id);
			using var relay = new MyTaskSource();
			_link.OnReady += (x, y) => relay.TrySetResult();

			if (!await MyTaskExtensions.RunOnScheduler(_link.Initialize))
			{
				await PrivateShutdown();
				throw new InvalidOperationException($"{nameof(_link)} wasn't able to initialize!");
			}

			await relay.MyTask;
		}

		private async Task SetStatus(StatusRecord record)
		{
			if (_link == null)
				throw new InvalidOperationException($"{nameof(_link)} was null!");

			if (!record.HasAnything)
				return;

			var rp = new RichPresence();

			if (record.TopText != null)
				rp.WithDetails(record.TopText);
			if (record.BottomText != null)
				rp.WithState(record.BottomText);

			var asset = new Assets();
			if (record.SmallIcon != null)
			{
				asset.SmallImageKey = record.SmallIcon.Target;
				if (record.SmallIcon.Text != null)
					asset.SmallImageText = record.SmallIcon.Text;
			}
			if (record.LargeIcon != null)
			{
				asset.LargeImageKey = record.LargeIcon.Target;
				if (record.LargeIcon.Text != null)
					asset.LargeImageText = record.LargeIcon.Text;
			}
			rp.WithAssets(asset);

			var thingy = MyTaskExtensions.RunOnScheduler(() => _link.SetPresence(rp), _source.Token);
			_waiterTask = Task.WhenAll(thingy, Task.Delay(2500, _source.Token), Task.Delay(record.Duration, _source.Token));
			await thingy;
		}

		public async Task PassNext(StatusRecord record)
		{
			await using (var _ = await _lock.BlockAsyncLock())
			{

				await _waiterTask;
				await UpdateIfNeeded(record.ApplicationID);
				await SetStatus(record);
			}
			await _waiterTask;
		}

		public async Task Shutdown()
		{
			await using var _ = await _lock.BlockAsyncLock();
			await MyTaskExtensions.RunOnScheduler(_source.Cancel);
			await PrivateShutdown();
		}
	}
}