using System;

using R5T.T0150;
using R5T.T0151;


namespace R5T.F0074
{
    /// <summary>
    /// See <see href="https://docs.aws.amazon.com/AmazonS3/latest/userguide/bucketnamingrules.html"/>
    /// </summary>
    [DraftStrongTypeMarker]
    public class S3BucketName : TypedString
    {
        #region Static

        public static S3BucketName From(string s3BucketNameString)
        {
            var s3BucketName = new S3BucketName(s3BucketNameString);
            return s3BucketName;
        }

        #endregion


        public S3BucketName(string value)
            : base(value)
        {
        }
    }
}
