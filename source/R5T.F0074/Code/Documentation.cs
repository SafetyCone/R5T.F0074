using System;


namespace R5T.F0074
{
	/// <summary>
	/// AWS S3 functionality.
	/// </summary>
	public static class Documentation
	{
		/// <summary>
		/// S3 bucket names are globally unique. This is to say, two AWS users cannot have the same bucket names at all.
		/// </summary>
		public static readonly object BucketNamesAreGloballyUnique;

		/// <summary>
		/// The default S3 delete operation is idempotent.
		/// </summary>
		public static readonly object NativeS3ClientDeleteObjectIsIdempotent;

		/// <summary>
		/// The default S3 operation is idempotent. To avoid overwriting, a second call is required to ensure the item does not already exist before calling the default S3 operation.
		/// Thus the non-idempotent operation is slower.
		/// </summary>
		public static readonly object NonIdempotentIsSlower;
	}
}