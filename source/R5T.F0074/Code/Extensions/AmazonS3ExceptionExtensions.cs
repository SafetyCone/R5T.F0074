using System;

using Amazon.S3;


public static class AmazonS3ExceptionExtensions
{
    public static bool BucketAlreadyExists(this AmazonS3Exception exception)
    {
        var output = R5T.F0074.AmazonS3ExceptionOperator.Instance.IsBucketAlreadyExists(exception);
        return output;
    }

    public static bool BucketNameAlreadyTaken(this AmazonS3Exception exception)
    {
        var output = R5T.F0074.AmazonS3ExceptionOperator.Instance.IsBucketNameAlreadyTaken(exception);
        return output;
    }

    public static bool BucketNotFound(this AmazonS3Exception exception)
    {
        var output = R5T.F0074.AmazonS3ExceptionOperator.Instance.IsBucketNotFound(exception);
        return output;
    }
}

