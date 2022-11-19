using System;


namespace R5T.F0074
{
	public class RegionEndpoints : IRegionEndpoints
	{
		#region Infrastructure

	    public static IRegionEndpoints Instance { get; } = new RegionEndpoints();

	    private RegionEndpoints()
	    {
        }

	    #endregion
	}
}