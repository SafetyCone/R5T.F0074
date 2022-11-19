using System;


namespace R5T.F0074
{
	public class S3ClientOperator : IS3ClientOperator
	{
		#region Infrastructure

	    public static IS3ClientOperator Instance { get; } = new S3ClientOperator();

	    private S3ClientOperator()
	    {
        }

	    #endregion
	}
}