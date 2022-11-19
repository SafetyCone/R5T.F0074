using System;

using Amazon;

using R5T.T0131;


namespace R5T.F0074
{
	[ValuesMarker]
	public partial interface IRegionEndpoints : IValuesMarker
	{
		public RegionEndpoint Default => RegionEndpoint.USWest1;
	}
}