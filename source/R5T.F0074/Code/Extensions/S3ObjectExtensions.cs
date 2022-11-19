using System;

using Amazon.S3.Model;

using S3ObjectOperator = R5T.F0074.S3ObjectOperator;


public static class S3ObjectExtensions
{
    public static string ToString_Standard(S3Object s3Object)
    {
        var representation = S3ObjectOperator.Instance.ToString_Standard(s3Object);
        return representation;
    }
}

