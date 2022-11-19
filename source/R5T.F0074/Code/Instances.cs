using System;


namespace R5T.F0074
{
    public static class Instances
    {
        public static IBucketNameTokens BucketNameTokens { get; } = F0074.BucketNameTokens.Instance;
        public static IPolicyOperator PolicyOperator { get; } = F0074.PolicyOperator.Instance;
        public static IS3BucketNameOperator S3BucketNameOperator { get; } = F0074.S3BucketNameOperator.Instance;
        public static IS3BucketNameValues S3BucketNameValues { get; } = F0074.S3BucketNameValues.Instance;
        public static IS3ClientOperator S3ClientOperator { get; } = F0074.S3ClientOperator.Instance;
    }
}