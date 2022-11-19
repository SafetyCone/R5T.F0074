using System;


namespace R5T.F0074
{
	public class S3ErrorCodes : IS3ErrorCodes
	{
		#region Infrastructure

	    public static IS3ErrorCodes Instance { get; } = new S3ErrorCodes();

	    private S3ErrorCodes()
	    {
        }

	    #endregion
	}
}