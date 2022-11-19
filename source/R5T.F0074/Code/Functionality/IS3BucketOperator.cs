using System;
using System.Linq;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

using R5T.F0000;
using R5T.T0132;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IS3BucketOperator : IFunctionalityMarker
	{
		/// <summary>
		/// Quality-of-life overload for <see cref="IS3BucketOperator.DoesBucketExistForClientOwner(AmazonS3Client, S3BucketName)"/>, which is usually what is meant when asking if a bucket exists (because bucket names are globally unique and someone else might have a particular bucket name).
		/// </summary>
		public async Task<bool> BucketExists(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var bucketExists = await this.DoesBucketExistForClientOwner(
				client,
				bucketName);

			return bucketExists;
        }

		/// <summary>
		/// Creates a bucket. Idempotent in that no error is thrown if the bucket already exists.
		/// Returns whether the bucket was actually created (true) or if the bucket already existed (false).
		/// </summary>
		public async Task<bool> CreateBucket_Idempotent(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var request = new PutBucketRequest
			{
				BucketName = bucketName,
                UseClientRegion = true,
            };

			/// See: <see cref="Documentation.ExceptionTrappingIsRequired"/>.
			try
			{
				var response = await client.PutBucketAsync(request);

				return true;
			}
			catch (AmazonS3Exception exception)
			{
				if (exception.BucketAlreadyExists())
				{
					// That's ok, the bucket already existed.
					return false;
				}
				else
				{
					// Rethrow.
					throw;
				}
			}
		}

		/// <summary>
		/// Creates a bucket. Non-idempotent in that an error is thrown if the bucket already exists.
		/// </summary>
		public async Task CreateBucket_NonIdempotent(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var bucketWasCreated = await this.CreateBucket_Idempotent(
				client,
				bucketName);

			if(!bucketWasCreated)
            {
				throw new Exception($"{bucketName}: Bucket already existed.");
            }
        }

		/// <summary>
		/// Chooses <see cref="CreateBucket_Idempotent(AmazonS3Client, S3BucketName)"/> as the default.
		/// </summary>
		public async Task<bool> CreateBucket(
			AmazonS3Client client,
			S3BucketName bucketName)
		{
			var bucketWasCreated = await this.CreateBucket_Idempotent(
				client,
				bucketName);

			return bucketWasCreated;
		}

		/// <summary>
		/// Deletes a bucket. Idempotent in that no error is thrown if the bucket already does not exists.
		/// Returns whether the bucket was actually deleted (true) or if the bucket was already deleted (false).
		/// </summary>
		public async Task<bool> DeleteBucket_Idempotent(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			// TODO: implement allow-if-not-empty logic when I have put objects in things.

			var request = new DeleteBucketRequest
			{
				BucketName = bucketName.Value,
				UseClientRegion = true,
			};

			/// See <see cref="Documentation.ExceptionTrappingIsRequired"/>.
			try
			{
				var response = await client.DeleteBucketAsync(request);

				return true;
			}
			catch (AmazonS3Exception s3Exception)
			{
				if (s3Exception.BucketNotFound())
				{
					// Then it's ok, bucket did not exist.
					return false;
				}
				else
				{
					// Rethrow.
					throw;
				}
			}
		}

		/// <summary>
		/// Deletes a bucket. Non-idempotent in that an error is thrown if the bucket already does not exists.
		/// </summary>
		public async Task DeleteBucket_NonIdempotent(
			AmazonS3Client client,
			S3BucketName bucketName)
		{
			var bucketWasDeleted = await this.DeleteBucket_Idempotent(
				client,
				bucketName);

			if(!bucketWasDeleted)
            {
				throw new Exception($"{bucketName}: Bucket already did not exist.");
            }
		}

		/// <summary>
		/// Chooses <see cref="DeleteBucket_Idempotent(AmazonS3Client, S3BucketName)"/> as the default.
		/// </summary>
		public async Task<bool> DeleteBucket(
			AmazonS3Client client,
			S3BucketName bucketName)
		{
			var wasDeleted = await this.DeleteBucket_Idempotent(
				client,
				bucketName);

			return wasDeleted;
		}

		/// <summary>
		/// Determines if a bucket exists for anyone in the world.
		/// This is usually *not* what you mean when when you want to know whether a bucket exists (does the bucket exist for *you*, not whether the bucket exists globally for anyone), so also see <see cref="IS3BucketOperator.DoesBucketExistForClientOwner(AmazonS3Client, S3BucketName)(AmazonS3Client, S3BucketName)"/>.
		/// <inheritdoc cref="Documentation.BucketNamesAreGloballyUnique" path="/summary"/>
		/// </summary>
		public async Task<bool> DoesBucketExistGlobally(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var bucketExistsGlobally = await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);
			return bucketExistsGlobally;
		}

		

		/// <summary>
		/// Determines whether a bucket exists by name for the owner specified by the S3 client.
		/// This is usually what you mean when when you want to know whether a bucket exists (does the bucket exist for *you*, not whether the bucket exists globally for anyone), but also see <see cref="IS3BucketOperator.DoesBucketExistGlobally(AmazonS3Client, S3BucketName)"/>.
		/// <inheritdoc cref="Documentation.BucketNamesAreGloballyUnique" path="/summary"/>
		/// </summary>
		/// <remarks>
		/// This method gets a list of ALL buckets of the owner specified by the S3 client, then tests to see if the input bucket name exists in that list.
		/// </remarks>
		public async Task<bool> DoesBucketExistForClientOwner(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var buckets = await this.ListAllBucketsForOwner(client);

			var bucketExists = buckets
				.Where(x => x.BucketName == bucketName.Value)
				.Any();

			return bucketExists;
		}

		/// <summary>
		/// Quality-of-life method related to <see cref="IS3BucketOperator.DoesBucketExistGlobally(AmazonS3Client, S3BucketName)"/>, which is usually what is meant when you asking if you can create a bucket (because bucket names are globally unique and someone else might have a particular bucket name).
		/// </summary>
		public async Task<bool> IsBucketAvailable(
			AmazonS3Client client,
			S3BucketName bucketName)
		{
			var bucketExistsGlobally = await this.DoesBucketExistGlobally(
				client,
				bucketName);

			var bucketIsAvailable = !bucketExistsGlobally;
			return bucketIsAvailable;
		}

		public string GetPolicy_ObjectsPublicReadAccessByDefault(string bucketName)
        {
			var rawPolicy =
$@"
{{
	""Id"": ""Policy1666992206658"",
	""Version"": ""2012-10-17"",
	""Statement"": [
	{{
		""Sid"": ""Stmt1666992202786"",
		""Action"": [
		""s3:GetObject""
		],
		""Effect"": ""Allow"",
		""Resource"": ""arn:aws:s3:::{bucketName}/*"",
		""Principal"": ""*""
	}}]
}}
";
			var policy = rawPolicy.Trim();
			return policy;
		}

		public async Task<WasFound<string>> HasPolicy(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var request = new GetBucketPolicyRequest
			{
				BucketName = bucketName,
			};

			var response = await client.GetBucketPolicyAsync(request);

			var policy = response.Policy;

			var policyExists = Instances.PolicyOperator.PolicyExists(policy);

			var output = WasFound.From(policyExists, policy);
			return output;
        }

		public async Task<S3Bucket[]> ListAllBucketsForOwner(AmazonS3Client client)
        {
			var listBucketsRequest = new ListBucketsRequest();
			// Request is empty. One might worry about pagination of bucket (since queries default to 100 results, or can be set to a maximum of 1000), however, you can only have 100 S3 buckets! (Or 1000 if you have a special request.)
			// See: https://docs.aws.amazon.com/AmazonS3/latest/userguide/create-bucket-overview.html

			var response = await client.ListBucketsAsync(listBucketsRequest);

			var output = response.Buckets.ToArray();
			return output;
		}

		public async Task<S3Object[]> ListObjectsInBucket(
			AmazonS3Client client,
			S3BucketName bucketName,
			string prefix = IS3ObjectKeyValues.DefaultPrefix,
			int maximumResultsPerPageCount = IS3Values.DefaultMaximumResultsPerPageCount_Constant)
        {
			var actualMaximumCount = S3Operator.Instance.GetAwsActualMaximumResultsPerPageCount(maximumResultsPerPageCount);

			var request = new ListObjectsV2Request
			{
				BucketName = bucketName,
				Prefix = prefix,
				MaxKeys = actualMaximumCount,
			};

			var objects = await S3ClientOperator.Instance.ListAllObjects(
				client,
				request);

			return objects;
		}

		public async Task<S3AccessControlList> GetAccessControlList(
			AmazonS3Client client,
			S3BucketName bucketName)
		{
			var request = new GetACLRequest
			{
				BucketName = bucketName,
			};

			var response = await client.GetACLAsync(request);

			var accessControlList = response.AccessControlList;
			return accessControlList;
		}

		/// <summary>
		/// <read-access-by-default>Provides all objects in the bucket with public read access by default.</read-access-by-default>
		/// Note: this method is unsafe! It will overwrite any existing policy.
		/// Prefer using the <see cref="SetBucketObjectsPublicReadAccessByDefault_WithoutOverwrite(AmazonS3Client, S3BucketName)"/> method.
		/// </summary>
		public async Task SetBucketObjectsPublicReadAccessByDefault_WithOverwrite(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var policy = this.GetPolicy_ObjectsPublicReadAccessByDefault(bucketName);

			var request = new PutBucketPolicyRequest
			{
				BucketName = bucketName,
				Policy = policy,
			};

			// Ignore the response.
			await client.PutBucketPolicyAsync(request);
        }

		/// <summary>
		/// <inheritdoc cref="SetBucketObjectsPublicReadAccessByDefault_WithOverwrite(AmazonS3Client, S3BucketName)" path="/summary/read-access-by-default"/>
		/// Note: this method is safe. It will *not* overwrite any existing policy.
		/// </summary>
		public async Task SetBucketObjectsPublicReadAccessByDefault_WithoutOverwrite(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			var hasPolicy = await this.HasPolicy(
				client,
				bucketName);

			if(hasPolicy)
            {
				throw new Exception($"{bucketName} - Policy already exists for bucket.");
            }

			// Now set the policy since we know we will not overwrite.
			await this.SetBucketObjectsPublicReadAccessByDefault_WithOverwrite(
				client,
				bucketName);
        }

		/// <summary>
		/// Chooses <see cref="SetBucketObjectsPublicReadAccessByDefault_WithoutOverwrite(AmazonS3Client, S3BucketName)"/> as the default.
		/// </summary>
		public async Task SetBucketObjectsPublicReadAccessByDefault(
			AmazonS3Client client,
			S3BucketName bucketName)
        {
			await this.SetBucketObjectsPublicReadAccessByDefault_WithoutOverwrite(
				client,
				bucketName);
        }
	}
}