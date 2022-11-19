using System;


namespace R5T.F0074
{
	public class URIs : IURIs
	{
		#region Infrastructure

	    public static IURIs Instance { get; } = new URIs();

	    private URIs()
	    {
        }

	    #endregion
	}
}