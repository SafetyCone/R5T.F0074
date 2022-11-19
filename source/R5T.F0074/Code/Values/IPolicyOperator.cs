using System;

using R5T.T0131;


namespace R5T.F0074
{
	[ValuesMarker]
	public partial interface IPolicyOperator : IValuesMarker
	{
		public bool IsNonExistentPolicy(string policy)
        {
			var output = policy == S3Values.Instance.NonExistentPolicy;
			return output;
        }

		public bool PolicyExists(string policy)
        {
			var isNonExistentPolicy = this.IsNonExistentPolicy(policy);

			var policyExists = !isNonExistentPolicy;
			return policyExists;
        }
	}
}