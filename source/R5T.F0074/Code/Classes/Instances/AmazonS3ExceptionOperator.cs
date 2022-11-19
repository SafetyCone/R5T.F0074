using System;


namespace R5T.F0074
{
	public class AmazonS3ExceptionOperator : IAmazonS3ExceptionOperator
	{
		#region Infrastructure

	    public static IAmazonS3ExceptionOperator Instance { get; } = new AmazonS3ExceptionOperator();

	    private AmazonS3ExceptionOperator()
	    {
        }

	    #endregion
	}
}