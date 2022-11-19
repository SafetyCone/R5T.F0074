using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

using R5T.T0132;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IS3ClientOperator : IFunctionalityMarker
	{
        public Task CreateBucket(AmazonS3Client client, string bucketName)
        {
            var s3BucketName = S3BucketName.From(bucketName);

            return this.CreateBucket(
                client,
                s3BucketName);
        }

        public async Task CreateBucket(AmazonS3Client client, S3BucketName bucketName)
        {
            var request = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true,
            };

            //try
            //{
            var response = await client.PutBucketAsync(request);

            //    return true;
            //}
            //catch (AmazonS3Exception exception)
            //{
            //    if (exception.BucketAlreadyExists())
            //    {
            //        // That's ok, the bucket already existed.
            //        return false;
            //    }
            //    else
            //    {
            //        throw new UnhandledAmazonS3Exception(exception);
            //    }
            //}
        }

        public AmazonS3Client GetS3Client()
        {
            var accesKeyFilePath = @"C:\Users\David\Dropbox\Organizations\Rivet\Shared\Data\Secrets\AWS-david.coats@gmail.com.json";

            var ourAwsCredentials = F0032.JsonOperator.Instance.Deserialize_Synchronous<AwsCredentials>(
                accesKeyFilePath);

            var awsRegionEndpoint = RegionEndpoint.USWest1;

            var awsCredentials = new BasicAWSCredentials(
                ourAwsCredentials.AccessKeyID,
                ourAwsCredentials.SecretAccessKey);

            var s3Client = new AmazonS3Client(
                awsCredentials,
                awsRegionEndpoint);

            return s3Client;
        }

        public async Task<S3Object[]> ListAllObjects(
            AmazonS3Client s3Client,
            ListObjectsV2Request listObjectsRequest)
        {
            var objects = new List<S3Object>();

            ListObjectsV2Response response;
            do
            {
                response = await s3Client.ListObjectsV2Async(listObjectsRequest);

                objects.AddRange(response.S3Objects);

                listObjectsRequest.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);

            var output = objects.ToArray();
            return output;
        }
    }
}