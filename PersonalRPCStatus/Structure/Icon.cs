using System.Diagnostics.CodeAnalysis;

namespace Name.Bayfaderix.Darxxemiyur.PersonalRPCStatus.Structure
{
	/// <summary>
	/// My icon type.
	/// </summary>
	public class Icon
	{
		public const int MaxTextLength = 128;
		public const int MaxTargetLength = 256;
		[NotNull] public readonly string Target;
		[NotNull] public readonly IconKind Type;
		public readonly string? Text;

		public Icon([NotNull] string target, [NotNull] IconKind type, string? text = null)
		{
			if (target.Length > MaxTargetLength)
				throw new ArgumentOutOfRangeException(nameof(target), $"Icon target exceeds length allowed by discord! Max target length is {MaxTargetLength}");

			if ((text?.Length ?? 0) > MaxTextLength)
				throw new ArgumentOutOfRangeException(nameof(text), $"Icon text exceeds length allowed by discord! Max text length is {MaxTextLength}! But supplied text's length is {text?.Length ?? 0}!");

			if (type == IconKind.Asset && !(Uri.TryCreate(target, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
				throw new ArgumentException($"{target} is not a valid target!", nameof(target));

			Target = target;
			Type = type;
			Text = text;
		}
	}

	/// <summary>
	/// Icon kind
	/// </summary>
	public enum IconKind
	{
		Asset,
		MediaProxy
	}
}