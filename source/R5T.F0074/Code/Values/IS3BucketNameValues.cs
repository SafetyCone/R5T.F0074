using System;
using System.Linq;

using R5T.Z0000;
using R5T.T0131;


namespace R5T.F0074
{
	[ValuesMarker]
	public partial interface IS3BucketNameValues : IValuesMarker
	{
		public char DefaultTokenSeparator => Characters.Instance.Period;

		public int MinimumLength => 3;
		public int MaximumLength => 63;

		public string AdjacentPeriods => "..";
		public string XnReservedPrefix => "xn--";
		public string S3AliasReservedSuffix => "-s3alias";

		public char[] AllowedCharacters => IS3BucketNameValues.zAllowedCharacters.Value;
		private static readonly Lazy<char[]> zAllowedCharacters = new Lazy<char[]>(() =>
			IS3BucketNameValues.zAllowedEndingCharacters.Value
			.Append(Characters.Instance.Period)
			.Append(Characters.Instance.Hyphen)
			.ToArray());

		public char[] AllowedEndingCharacters => IS3BucketNameValues.zAllowedEndingCharacters.Value;
		private static readonly Lazy<char[]> zAllowedEndingCharacters = new Lazy<char[]>(() =>
			CharacterSets.Instance.LowercaseLetters
			.Append(CharacterSets.Instance.Numbers)
			.ToArray());
	}
}