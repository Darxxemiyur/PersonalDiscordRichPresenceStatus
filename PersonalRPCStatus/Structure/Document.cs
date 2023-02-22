using Name.Bayfaderix.Darxxemiyur.Common;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	/// <summary>
	/// Document
	/// </summary>
	internal sealed class Document
	{
		private sealed class EmptyDocumentRecord : IStatusYieldable
		{
			public async IAsyncEnumerable<StatusRecord> UnrollRecords()
			{
				yield return new StatusRecord(TimeSpan.FromSeconds(10), "Empty document!", null, null, null, null);
			}
		}
		internal static Document Empty => EmptyDocument();
		private static Document EmptyDocument()
		{
			var doc = new Document();
			doc._yieldables.AddLast(new EmptyDocumentRecord());
			return doc;
		}
		private readonly LinkedList<IStatusYieldable> _yieldables;
		public Document(params IStatusYieldable[] records) : this(records.AsEnumerable()) { }
		internal Document(IEnumerable<IStatusYieldable>? input = null)
		{
			_yieldables = input?.DistinctUntilChanged()?.ToLinkedList() ?? new();
		}
		public async IAsyncEnumerable<StatusRecord> UnrollRecords()
		{
			foreach (var node in _yieldables)
			{
				await foreach (var child in node.UnrollRecords())
				{
					yield return child;
				}
			}
		}
	}
}