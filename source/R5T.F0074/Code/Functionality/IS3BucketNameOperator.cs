using System;
using System.Linq;

using R5T.F0000;
using R5T.T0132;
using R5T.T0146;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IS3BucketNameOperator : IFunctionalityMarker
	{
        public char GetTokenSeparator()
        {
            var tokenSeparator = S3BucketNameValues.Instance.DefaultTokenSeparator;
            return tokenSeparator;
        }

        public string Combine(string firstBucketNamePart, string secondBucketNamePart)
        {
            var tokenSeparator = this.GetTokenSeparator();

            var output = $"{firstBucketNamePart}{tokenSeparator}{secondBucketNamePart}";
            return output;  
        }

        public S3BucketName ToS3BucketName_WithValidation(string s3BucketName)
        {
            this.ValidateS3BucketName(s3BucketName);

            var typedS3BucketName = this.ToS3BucketName_WithoutValidation(s3BucketName);
            return typedS3BucketName;
        }

        public S3BucketName ToS3BucketName_WithoutValidation(string s3BucketNameString)
        {
            var s3BucketName = S3BucketName.From(s3BucketNameString);
            return s3BucketName;
        }

        /// <summary>
        /// Chooses <see cref="ToS3BucketName_WithValidation(string)"/> as the default.
        /// </summary>
        public S3BucketName ToS3BucketName(string s3BucketNameString)
        {
            var s3BucketName = S3BucketName.From(s3BucketNameString);
            return s3BucketName;
        }

        public string GetGuidedBucketName(Guid guid)
		{
            // Must use lowercase letters.
			var guidString = GuidOperator.Instance.ToString_D_Format(guid);

            var bucketName = this.Combine(
                Instances.BucketNameTokens.Bucket,
                guidString);

			return bucketName;
		}

        /// <summary>
        /// Gets a random bucket name that is the same every time.
        /// </summary>
        public string GetFixedRandomBucketName_String()
        {
            var guid = GuidOperator.Instance.NewSeededGuid();

            var bucketName = this.GetGuidedBucketName(guid);
            return bucketName;
        }

        /// <inheritdoc cref="GetFixedRandomBucketName_String"/>
        public S3BucketName GetFixedRandomBucketName()
        {
            var bucketNameString = this.GetFixedRandomBucketName_String();

            var bucketName = this.ToS3BucketName(bucketNameString);
            return bucketName;
        }

        /// <summary>
        /// Gets a random bucket name that is the same every time.
        /// </summary>
        public string GetFixedRandomBucketName_Public_String()
        {
            var guid = GuidOperator.Instance.NewSeededGuid();

            var bucketName = this.GetGuidedBucketName(guid);

            var publicBucketName = this.MakePublic(bucketName);
            return publicBucketName;
        }

        /// <inheritdoc cref="GetFixedRandomBucketName_Public_String"/>
        public S3BucketName GetFixedRandomBucketName_Public()
        {
            var bucketNameString = this.GetFixedRandomBucketName_Public_String();

            var bucketName = this.ToS3BucketName(bucketNameString);
            return bucketName;
        }

        public string GetNewRandomBucketName()
        {
            var guid = GuidOperator.Instance.New();

            var bucketName = this.GetGuidedBucketName(guid);
            return bucketName;
        }

        /// <summary>
        /// See: <see href="https://docs.aws.amazon.com/AmazonS3/latest/userguide/bucketnamingrules.html"/>.
        /// </summary>
        public Result<bool> IsValidS3BucketName(string s3BucketName)
        {
            var result = ResultOperator.Instance.New<bool>()
                .WithTitle("Validate AWS S3 bucket name")
                .WithMetadata(
                    nameof(s3BucketName), s3BucketName)
                ;

            var length = s3BucketName.Length;

            // Length at least 3 characters long.
            var isTooShort = length < Instances.S3BucketNameValues.MinimumLength;
            if (isTooShort)
            {
                result.WithFailure($"Length is too short. Minimum length is {Instances.S3BucketNameValues.MinimumLength}, found: {length}.");
            }

            // Length no more than 63 characters long.
            var isTooLong = length > Instances.S3BucketNameValues.MaximumLength;
            if (isTooLong)
            {
                result.WithFailure($"Length is too long. Maximum length is {Instances.S3BucketNameValues.MinimumLength}, found: {length}.");
            }

            // Only lowercase letters, numbers, dots (periods), and hyphens allowed.
            var allowedCharacters = Instances.S3BucketNameValues.AllowedCharacters;

            var disallowedCharactersInName = s3BucketName.AsEnumerable()
                .Distinct()
                .Except(allowedCharacters)
                .OrderAlphabetically()
                .ToArray();

            var anyDisallowedCharacters = disallowedCharactersInName.Any();
            if (anyDisallowedCharacters)
            {
                var disallowedCharacters = disallowedCharactersInName.Join();

                result.WithFailure($"Disallowed characters found: {disallowedCharacters}");
            }

            // Must begin and end with letter or number.
            var firstCharacter = s3BucketName.First();
            var lastCharacter = s3BucketName.Last();

            var allowedEndingCharacters = Instances.S3BucketNameValues.AllowedEndingCharacters;

            var firstCharacterIsNotAllowed = !allowedEndingCharacters.Contains(firstCharacter);
            if (firstCharacterIsNotAllowed)
            {
                result.WithFailure($"First character must be a lowercase letter or number, found: {firstCharacter}");
            }

            var lastCharacterIsNotAllowed = !allowedEndingCharacters.Contains(lastCharacter);
            if (lastCharacterIsNotAllowed)
            {
                result.WithFailure($"Last character must be a lowercase letter or number, found: {lastCharacter}");
            }

            // There cannot be two adjacent periods.
            var containsAdjacentPeriods = StringOperator.Instance.Contains(
                s3BucketName,
                Instances.S3BucketNameValues.AdjacentPeriods);

            if (containsAdjacentPeriods)
            {
                result.WithFailure("The bucket name cannot contain adjacent periods.");
            }

            // Cannot be formatted as an IP address.
            var isIPAddress = IPAddressOperator.Instance.IsIPAddress(s3BucketName);
            if (isIPAddress)
            {
                result.WithFailure($"The bucket name cannot resemble an IP address, found IP address: {isIPAddress.Result}.");
            }

            // Cannot start with special reserved prefix "xn--".
            var beginsWithXnReservedPrefix = StringOperator.Instance.BeginsWith(
                s3BucketName,
                Instances.S3BucketNameValues.XnReservedPrefix);

            if (beginsWithXnReservedPrefix)
            {
                result.WithFailure($"The bucket name cannot begin with the reserved prefix '{Instances.S3BucketNameValues.XnReservedPrefix}'.");
            }

            // Cannot end with special suffix "-s3alias".
            var endssWithS3AliasReservedPrefix = StringOperator.Instance.EndsWith(
                s3BucketName,
                Instances.S3BucketNameValues.S3AliasReservedSuffix);

            if (endssWithS3AliasReservedPrefix)
            {
                result.WithFailure($"The bucket name cannot end with the reserved prefix '{Instances.S3BucketNameValues.S3AliasReservedSuffix}'.");
            }

            var isSuccess = result.IsSuccess();

            result.WithValue(isSuccess);

            return result;
        }

        public string MakePublic(string bucketName)
        {
            var publicBucketName = this.Combine(
                Instances.BucketNameTokens.Public,
                bucketName);

            return publicBucketName;
        }
        
        public void ValidateS3BucketName(string s3BucketName)
        {
            var isValid = this.IsValidS3BucketName(s3BucketName);
            if(!isValid.Value)
            {
                throw new Exception($"Invalid S3 bucket name: {s3BucketName}");
            }
        }
    }
}