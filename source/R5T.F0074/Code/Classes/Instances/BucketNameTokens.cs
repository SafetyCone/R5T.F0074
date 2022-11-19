using System;


namespace R5T.F0074
{
	public class BucketNameTokens : IBucketNameTokens
	{
		#region Infrastructure

	    public static IBucketNameTokens Instance { get; } = new BucketNameTokens();

	    private BucketNameTokens()
	    {
        }

	    #endregion
	}
}