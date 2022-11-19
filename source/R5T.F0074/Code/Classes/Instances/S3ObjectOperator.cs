using System;


namespace R5T.F0074
{
	public class S3ObjectOperator : IS3ObjectOperator
	{
		#region Infrastructure

	    public static IS3ObjectOperator Instance { get; } = new S3ObjectOperator();

	    private S3ObjectOperator()
	    {
        }

	    #endregion
	}
}