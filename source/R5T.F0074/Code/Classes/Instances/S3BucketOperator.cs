using System;


namespace R5T.F0074
{
	public class S3BucketOperator : IS3BucketOperator
	{
		#region Infrastructure

	    public static IS3BucketOperator Instance { get; } = new S3BucketOperator();

	    private S3BucketOperator()
	    {
        }

	    #endregion
	}
}