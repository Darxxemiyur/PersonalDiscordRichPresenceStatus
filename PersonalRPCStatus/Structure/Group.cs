namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	internal class Group : IStatusYieldable
	{
		public Task<StatusRecord> GetRecord() => throw new NotImplementedException();

		public Task<bool> IsCompleted() => throw new NotImplementedException();

		public Task<bool> IsCurrentEmpty() => throw new NotImplementedException();

		public Task<bool> Next() => throw new NotImplementedException();

		public Task<IAsyncEnumerable<StatusRecord>> UnrollRecords() => throw new NotImplementedException();
	}
}