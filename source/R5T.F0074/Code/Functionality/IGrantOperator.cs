using System;

using Amazon.S3;
using Amazon.S3.Model;

using R5T.T0132;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IGrantOperator : IFunctionalityMarker
	{
		public S3Grant NewPublicReadGrant()
        {
			var grant = new S3Grant
			{
				Grantee = new S3Grantee
				{
					URI = URIs.Instance.AllUsersGrantee,
				},
				Permission = S3Permission.READ,
			};

			return grant;
		}
	}
}