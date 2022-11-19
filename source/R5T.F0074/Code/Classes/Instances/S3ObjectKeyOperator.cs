using System;


namespace R5T.F0074
{
	public class S3ObjectKeyOperator : IS3ObjectKeyOperator
	{
		#region Infrastructure

	    public static IS3ObjectKeyOperator Instance { get; } = new S3ObjectKeyOperator();

	    private S3ObjectKeyOperator()
	    {
        }

	    #endregion
	}
}