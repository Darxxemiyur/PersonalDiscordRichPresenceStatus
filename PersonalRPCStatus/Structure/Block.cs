using Name.Bayfaderix.Darxxemiyur.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	internal class Block : IStatusYieldable
	{
		private readonly LinkedList<StatusRecord> _records;
		public Block(params StatusRecord[] records) : this(records.AsEnumerable()) { }
		public Block(IEnumerable<StatusRecord>? records = null) => _records = records?.DistinctUntilChanged()?.ToLinkedList() ?? new();
		public async IAsyncEnumerable<StatusRecord> UnrollRecords()
		{
			foreach (var record in _records)
				yield return record;
		}
	}
}
