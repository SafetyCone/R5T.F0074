using System;

using R5T.T0150;
using R5T.T0151;


namespace R5T.F0074
{
    /// <summary>
    /// See <see href="https://docs.aws.amazon.com/AmazonS3/latest/userguide/object-keys.html"/>
    /// </summary>
    [DraftStrongTypeMarker]
    public class S3ObjectKey : TypedString
    {
        #region Static

        public static S3ObjectKey From(string s3ObjectKeyString)
        {
            var s3ObjectKey = new S3ObjectKey(s3ObjectKeyString);
            return s3ObjectKey;
        }

        #endregion


        public S3ObjectKey(string value)
            : base(value)
        {
        }
    }
}
