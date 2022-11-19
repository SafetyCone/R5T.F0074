using System;

using Amazon.S3;

using R5T.T0132;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IAmazonS3ExceptionOperator : IFunctionalityMarker
	{
        public bool ErrorCodeIs(
            AmazonS3Exception exception,
            string errorCode)
        {
            var output = exception.ErrorCode == errorCode;
            return output;
        }

        /// <summary>
        /// Confusingly, this is that the bucket is already owned by you.
        /// </summary>
        public bool IsBucketAlreadyExists(AmazonS3Exception exception)
        {
            var output = this.ErrorCodeIs(
                exception,
                S3ErrorCodes.Instance.BucketAlreadyOwnedByYou);

            return output;
        }

        /// <summary>
        /// Confusingly, this is that the bucket already exists, but is owned by someone else.
        /// </summary>
        public bool IsBucketNameAlreadyTaken(AmazonS3Exception exception)
        {
            var output = this.ErrorCodeIs(
                exception,
                S3ErrorCodes.Instance.BucketAlreadyExists);

            return output;
        }

        /// <summary>
        /// The bucket does not exist.
        /// It is neither owned by you, nor anyone else.
        /// </summary>
        public bool IsBucketNotFound(AmazonS3Exception exception)
        {
            var output = this.ErrorCodeIs(
                exception,
                S3ErrorCodes.Instance.NoSuchBucket);

            return output;
        }
    }
}