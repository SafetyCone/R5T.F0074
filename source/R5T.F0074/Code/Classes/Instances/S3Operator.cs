using System;


namespace R5T.F0074
{
	public class S3Operator : IS3Operator
	{
		#region Infrastructure

	    public static IS3Operator Instance { get; } = new S3Operator();

	    private S3Operator()
	    {
        }

	    #endregion
	}
}