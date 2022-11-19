using System;


namespace R5T.F0074
{
	public class S3BucketNameValues : IS3BucketNameValues
	{
		#region Infrastructure

	    public static IS3BucketNameValues Instance { get; } = new S3BucketNameValues();

	    private S3BucketNameValues()
	    {
        }

	    #endregion
	}
}