using System;

using R5T.T0131;


namespace R5T.F0074
{
	/// <summary>
	/// See <see href="https://docs.aws.amazon.com/AmazonS3/latest/API/ErrorResponses.html#ErrorCodeList"/>.
	/// </summary>
	[ValuesMarker]
	public partial interface IS3ErrorCodes : IValuesMarker
	{
		public string BucketAlreadyExists => "BucketAlreadyExists";
		public string BucketAlreadyOwnedByYou => "BucketAlreadyOwnedByYou";
		public string NoSuchBucket => "NoSuchBucket";
	}
}