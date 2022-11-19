using System;


namespace R5T.F0074
{
	public class S3BucketNameOperator : IS3BucketNameOperator
	{
		#region Infrastructure

	    public static IS3BucketNameOperator Instance { get; } = new S3BucketNameOperator();

	    private S3BucketNameOperator()
	    {
        }

	    #endregion
	}
}