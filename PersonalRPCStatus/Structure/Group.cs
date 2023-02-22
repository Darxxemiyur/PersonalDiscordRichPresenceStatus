using Name.Bayfaderix.Darxxemiyur.Common;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	internal class Group : IStatusYieldable
	{
		private readonly LinkedList<IStatusYieldable> _yieldables;
		public Group(params IStatusYieldable[] records) : this(records.AsEnumerable()) { }
		internal Group(IEnumerable<IStatusYieldable>? children = null)
		{
			_yieldables = children?.DistinctUntilChanged()?.ToLinkedList() ?? new();
		}

		public async IAsyncEnumerable<StatusRecord> UnrollRecords()
		{
			foreach (var node in _yieldables)
				await foreach (var child in node.UnrollRecords())
					yield return child;
		}
	}
}