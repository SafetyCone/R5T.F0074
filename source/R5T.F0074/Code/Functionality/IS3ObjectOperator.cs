using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using R5T.F0000;
using R5T.T0132;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IS3ObjectOperator : IFunctionalityMarker
	{
		/// <summary>
		/// Deletes an object. Idempotent in that no error occurs if the object already does not exist.
		/// </summary>
		/// <remarks>
		/// <inheritdoc cref="Documentation.NativeS3ClientDeleteObjectIsIdempotent" path="/summary"/>
		/// </remarks>
		public async Task DeleteObject_Idempotent(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
        {
			// Client is idempotent, ignore the response.
			await client.DeleteObjectAsync(bucketName.Value, key.Value);
		}

		/// <summary>
		/// Deletes an object. Non-idempotent in that an error occurs if the object already does not exist.
		/// <inheritdoc cref="Documentation.NonIdempotentIsSlower" path="/summary"/>
		/// You should prefer <see cref="DeleteObject_Idempotent(AmazonS3Client, S3BucketName, S3ObjectKey)"/>.
		/// </summary>
		public async Task DeleteObject_NonIdempotent(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
		{
			await this.VerifyKeyExists(
				client,
				bucketName,
				key);

			await this.DeleteObject_Idempotent(
				client,
				bucketName,
				key);
		}

		/// <summary>
		/// Chooses <see cref="DeleteObject_Idempotent(AmazonS3Client, S3BucketName, S3ObjectKey)"/> as the default.
		/// This matches the behavior of the S3 client.
		/// </summary>
		public async Task DeleteObject(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
		{
			await this.DeleteObject_Idempotent(
				client,
				bucketName,
				key);
		}

		public async Task DownloadToFile(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			string filePath)
		{
            // Overwrite handled.
            using var transferUtility = new TransferUtility(client);

            await transferUtility.DownloadAsync(filePath, bucketName.Value, key.Value);
        }

		public async Task DownloadToStream(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			Stream stream)
        {
			var response = await client.GetObjectAsync(bucketName.Value, key.Value);

			await response.ResponseStream.CopyToAsync(stream);
		}

		public async Task<bool> Exists(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
        {
			var hasObject = await this.HasObject(
				client,
				bucketName,
				key);

			var output = hasObject.Exists;
			return output;
        }

		public async Task<S3AccessControlList> GetAccessControlList(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
        {
			var request = new GetACLRequest
			{
				BucketName = bucketName,
				Key = key,
			};

			var response = await client.GetACLAsync(request);

			var accessControlList = response.AccessControlList;
			return accessControlList;
        }

		public async Task<S3Object> GetObject(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
        {
			var hasObject = await this.HasObject(
				client,
				bucketName,
				key);

			if(!hasObject)
            {
				throw new Exception($"No S3 object found: {bucketName}:{key}");
            }

			return hasObject.Result;
        }

		public async Task<WasFound<S3Object>> HasObject(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
        {
			// GetObjectAsync() throws an exception when the object does not exist.
			// So instead of trapping the exception, asking for a list of only one object.
			var request = new ListObjectsV2Request
			{
                BucketName = bucketName,
				// Use prefix as key, since the key does infact start with a prefix equal to the key...
                Prefix = key,
                MaxKeys = 1,
            };

            var response = await client.ListObjectsV2Async(request);

            var wasFound = WasFound.From(response.S3Objects.FirstOrDefault());
            return wasFound;
        }

		public async Task SetAsPublicRead_Idempotent(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
		{
			// Get the access control list of the object.
			var accessControlList = await this.GetAccessControlList(
				client,
				bucketName,
				key);

			var publicReadGrantAdded = AccessControlListOperator.Instance.AddPublicReadGrant_Idempotent(accessControlList);
			if(publicReadGrantAdded)
            {
				var request = new PutACLRequest
				{
					AccessControlList = accessControlList,
					BucketName = bucketName,
					Key = key,
				};

				// Ignore the response.
				await client.PutACLAsync(request);
            }

			//// Use one of the canned Access Control Lists.
			//var request = new PutACLRequest
			//{
			//	BucketName = bucketName,
			//	Key = key,
			//	CannedACL = S3CannedACL.PublicRead,
			//};

			// Ignore the response.
			//await client.PutACLAsync(request);
		}

		public string ToString_Standard(S3Object s3Object)
        {
			var representation = this.ToString_Standard(
				s3Object.BucketName,
				s3Object.Key);

			return representation;
        }

		public string ToString_Standard(string s3BucketName, string s3ObjectKey)
		{
			var representation = $"{s3BucketName}{Z0000.Strings.Instance.Colon}{s3ObjectKey}";
			return representation;
		}

		/// <summary>
		/// Chooses <see cref="ToString_Standard(S3Object)"/> as the default.
		/// </summary>
		public string ToString(S3Object s3Object)
        {
			var representation = this.ToString_Standard(s3Object);
			return representation;
        }

        public async Task UploadFile_WithOverwrite(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			string filePath)
		{
            using var transferUtility = new TransferUtility(client);

			// Transfer utility overwrites by default.
            await transferUtility.UploadAsync(filePath, bucketName.Value, key.Value);
        }

		/// <summary>
		/// <inheritdoc cref="Documentation.NonIdempotentIsSlower" path="/summary"/>
		/// You should prefer <see cref="UploadFile_WithOverwrite(AmazonS3Client, S3BucketName, S3ObjectKey, string)"/>.
		/// </summary>
		public async Task UploadFile_WithoutOverwrite(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			string filePath)
		{
			await this.VerifyKeyDoesNotExist(
				client,
				bucketName,
				key);

			await this.UploadFile_WithOverwrite(
				client,
				bucketName,
				key,
				filePath);
		}

		/// <summary>
		/// Chooses <see cref="UploadFile_WithOverwrite(AmazonS3Client, S3BucketName, S3ObjectKey, string)"/> as the default.
		/// </summary>
		public async Task UploadFile(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			string filePath)
		{
			await this.UploadFile_WithOverwrite(
				client,
				bucketName,
				key,
				filePath);
		}

		public async Task UploadStream_WithOverwrite(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			Stream stream)
        {
            using var transferUtility = new TransferUtility(client);

			// Transfer utility overwrites by default.
			await transferUtility.UploadAsync(stream, bucketName.Value, key.Value);
        }

		/// <summary>
		/// <inheritdoc cref="Documentation.NonIdempotentIsSlower" path="/summary"/>
		/// You should prefer <see cref="UploadStream_WithOverwrite(AmazonS3Client, S3BucketName, S3ObjectKey, Stream)"/>.
		/// </summary>
		public async Task UploadStream_WithoutOverwrite(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			Stream stream)
		{
			await this.VerifyKeyDoesNotExist(
				client,
				bucketName,
				key);

			await this.UploadStream_WithOverwrite(
				client,
				bucketName,
				key,
				stream);
		}

		/// <summary>
		/// Chooses <see cref="UploadStream_WithOverwrite(AmazonS3Client, S3BucketName, S3ObjectKey, Stream)"/> as the default.
		/// </summary>
		public async Task UploadStream(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key,
			Stream stream)
		{
			await this.UploadStream_WithOverwrite(
				client,
				bucketName,
				key,
				stream);
		}

		public async Task VerifyKeyExists(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
		{
			// Check whether the key exists first.
			var exists = await this.Exists(
				client,
				bucketName,
				key);

			if (!exists)
			{
				throw new Exception($"Key does not exist: {S3ObjectOperator.Instance.ToString_Standard(bucketName, key)}");
			}
		}

		public async Task VerifyKeyDoesNotExist(
			AmazonS3Client client,
			S3BucketName bucketName,
			S3ObjectKey key)
        {
			// Check whether the key exists first.
			var exists = await this.Exists(
				client,
				bucketName,
				key);

			if (exists)
			{
				throw new Exception($"Key already exists: {S3ObjectOperator.Instance.ToString_Standard(bucketName, key)}");
			}
		}
	}
}