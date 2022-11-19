using System;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using R5T.T0132;


namespace R5T.F0074
{
	[FunctionalityMarker]
	public partial interface IS3Operator : IFunctionalityMarker
	{
		/// <summary>
		/// Get the actual number of results per page that will be returned by an AWS request.
		/// You might specify an unlimited number of results per page, but the AWS infrastructure will limit you.
		/// </summary>
		/// <returns>The <see cref="IS3Values.MaximumAllowedMaximumResultsPerPageCount"/> if the <paramref name="requestedMaximumResultsPerPageCount"/> is greater than the maximum allowed maximum results per page, otherwise the <paramref name="requestedMaximumResultsPerPageCount"/>.</returns>
		public int GetAwsActualMaximumResultsPerPageCount(int requestedMaximumResultsPerPageCount)
		{
			if (this.IsGreaterThanMaximumAllowedMaximumResultsPerPageCount(requestedMaximumResultsPerPageCount)
				|| F0000.QueryOperator.Instance.IsNoLimitMaximumResultsCount(requestedMaximumResultsPerPageCount))
			{
				// If you request more than the allowed maximum, or no limit, you will actually be limited by the AWS infrastructure.
				return S3Values.Instance.MaximumAllowedMaximumResultsPerPageCount;
			}
			else
			{
				// Otherwise, your requested value is fine.
				return requestedMaximumResultsPerPageCount;
			}
		}

		public bool IsGreaterThanMaximumAllowedMaximumResultsPerPageCount(int requestedMaximumResultsPerPageCount)
		{
			var output = requestedMaximumResultsPerPageCount > S3Values.Instance.MaximumAllowedMaximumResultsPerPageCount;
			return output;
		}
	}
}