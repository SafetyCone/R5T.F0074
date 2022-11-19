using System;

using R5T.T0131;
using R5T.Z0000;


namespace R5T.F0074
{
	[ValuesMarker]
	public partial interface IS3Values : IValuesMarker
	{
        public string NonExistentPolicy => Strings.Instance.Null;

        /// <summary>
        /// If you want more than the <see cref="DefaultMaximumResultsPerPageCount"/> results per page, you can request a greater number of results per page.
        /// However, you can only request a greater number of results up to this allowed maximum results per page.
        /// You might want to request ALL results in a large list of results, but this allowed maximum means that you must instead get those results a batch at a time, which requires a fixed network latency per batch.
        /// This encourages use of query parameters to limit the size of a request result set.
        /// </summary>
        public const int MaximumAllowedMaximumResultsPerPageCount_Constant = 1000;
        /// <inheritdoc cref="MaximumAllowedMaximumResultsPerPageCount_Constant"/>
        public int MaximumAllowedMaximumResultsPerPageCount => IS3Values.MaximumAllowedMaximumResultsPerPageCount_Constant;

        /// <summary>
        /// By default, results will be returned in batches of this many items.
        /// If a request returns more items than the per-page key count, paging of multiple pages of results will be required.
        /// </summary>
        public const int DefaultMaximumResultsPerPageCount_Constant = 100;
        /// <inheritdoc cref="DefaultMaximumResultsPerPageCount_Constant"/>
        public int DefaultMaximumResultsPerPageCount => IS3Values.DefaultMaximumResultsPerPageCount_Constant;
    }
}