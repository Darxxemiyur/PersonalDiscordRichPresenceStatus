using System.Diagnostics.CodeAnalysis;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	/// <summary>
	/// Display unit
	/// </summary>
	public class StatusRecord
	{
		public const int MaxTextLength = 128;
		public readonly string? TopText;
		public readonly string? BottomText;
		public readonly Icon? SmallIcon;
		public readonly Icon? LargeIcon;
		public readonly string? ApplicationID;
		[NotNull] public readonly TimeSpan Duration;
		public bool HasAnything => (TopText != null || BottomText != null || SmallIcon != null || LargeIcon != null) && Duration.TotalSeconds > 0;

		public StatusRecord(string? topText = null, string? bottomText = null, Icon? smallIcon = null, Icon? largeIcon = null, TimeSpan? duration = default, ulong? applicationID = null) : this(topText, bottomText, smallIcon, largeIcon, duration, applicationID?.ToString())
		{
		}

		public StatusRecord(string? topText = null, string? bottomText = null, Icon? smallIcon = null, Icon? largeIcon = null, TimeSpan? duration = default, string? applicationID = null)
		{
			TopText = AcceptOrThrow(topText);
			BottomText = AcceptOrThrow(bottomText);
			SmallIcon = smallIcon;
			LargeIcon = largeIcon;
			Duration = TimeSpan.FromMilliseconds(Math.Max(0, duration?.TotalMilliseconds ?? 0));
			ApplicationID = applicationID;
		}

		private static string? AcceptOrThrow([MaybeNull] string? text)
		{
			text = text?.Trim();
			if (text?.Length > MaxTextLength)
				throw new ArgumentOutOfRangeException(nameof(text), $"Supplied text length was longer than allowed by discord! Maximum text length is {MaxTextLength} characters!");
			return text;
		}
	}
}