using System;

using R5T.T0132;
using R5T.T0146;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IS3ObjectKeyOperator : IFunctionalityMarker
	{
		public S3ObjectKey ToS3ObjectKey_WithoutValidation(string s3ObjectKeyString)
        {
			var s3ObjectKey = S3ObjectKey.From(s3ObjectKeyString);
			return s3ObjectKey;
        }

		public S3ObjectKey ToS3ObjectKey_WithValidation(string s3ObjectKeyString)
		{
			this.ValidateS3ObjectKey(s3ObjectKeyString);

			var s3ObjectKey = S3ObjectKey.From(s3ObjectKeyString);
			return s3ObjectKey;
		}

		/// <summary>
		/// Chooses <see cref="ToS3ObjectKey_WithValidation(string)"/> as the default.
		/// </summary>
		public S3ObjectKey ToS3ObjectKey(string s3ObjectKeyString)
        {
			var s3ObjectKey = this.ToS3ObjectKey_WithValidation(s3ObjectKeyString);
			return s3ObjectKey;
        }

		public Result<bool> IsValidS3ObjectKey(string s3ObjectKeyString)
        {
			return ResultOperator.Instance.Result<bool>()
				.WithValue(true);
        }

		public void ValidateS3ObjectKey(string s3ObjectKeyString)
		{
			var isValid = this.IsValidS3ObjectKey(s3ObjectKeyString);
			if (!isValid.Value)
			{
				throw new Exception($"Invalid S3 object key: {s3ObjectKeyString}");
			}
		}
	}
}