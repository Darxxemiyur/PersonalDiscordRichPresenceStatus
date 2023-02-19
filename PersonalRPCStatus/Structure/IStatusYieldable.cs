namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	/// <summary>
	/// Describes an interface of getting the StatusRecord objects. Similar to IEnumerable.
	/// </summary>
	public interface IStatusYieldable
	{
		Task<bool> IsCompleted();

		Task<bool> IsCurrentEmpty();

		Task<bool> Next();

		Task<StatusRecord> GetRecord();

		Task<IAsyncEnumerable<StatusRecord>> UnrollRecords();
	}
}