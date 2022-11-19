using System;

using R5T.T0131;


namespace R5T.F0074
{
	[ValuesMarker]
	public partial interface IBucketNameTokens : IValuesMarker
	{
		public string Bucket => "bucket";
		public string Public => "public";
	}
}