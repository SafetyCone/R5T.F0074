using System;

using R5T.T0131;


namespace R5T.F0074
{
	[ValuesMarker]
	public partial interface IS3ObjectKeyValues : IValuesMarker
	{
		public const string DefaultDelimiter = Z0000.IStrings.Slash_Constant;
		public const string DefaultPrefix = Z0000.IStrings.Null_Constant;
	}
}