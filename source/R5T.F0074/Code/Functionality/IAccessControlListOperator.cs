using System;
using System.Linq;

using Amazon.S3.Model;

using R5T.T0132;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IAccessControlListOperator : IFunctionalityMarker
	{
		public void AddPublicReadGrant_NonIdempotent(S3AccessControlList accessControlList)
        {
			var grant = GrantOperator.Instance.NewPublicReadGrant();

			accessControlList.Grants.Add(grant);
		}

		/// <summary>
		/// Returns whether a public read grant was added to the access control list.
		/// </summary>
		public bool AddPublicReadGrant_Idempotent(S3AccessControlList accessControlList)
        {
			var hasPublicReadAccess = AccessControlListOperator.Instance.HasPublicReadGrant(accessControlList);
			if (!hasPublicReadAccess)
			{
				this.AddPublicReadGrant_NonIdempotent(accessControlList);
			}

			var output = !hasPublicReadAccess;
			return output;
		}

		public bool HasPublicReadGrant(S3AccessControlList accessControlList)
		{
			var anyAllUsersGrant = accessControlList.Grants
				.Where(grant => true
					&& grant.Grantee.URI == URIs.Instance.AllUsersGrantee
					&& grant.Permission.Value == PermissionNames.Instance.Read)
				.Any();

			return anyAllUsersGrant;
		}
	}
}