namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	internal class YieldablePointer : IStatusYieldable
	{
		private readonly IStatusYieldable _target;

		public YieldablePointer(IStatusYieldable target) => _target = target;

		public IAsyncEnumerable<StatusRecord> UnrollRecords() => _target.UnrollRecords();
	}
}