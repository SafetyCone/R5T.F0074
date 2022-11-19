using System;


namespace R5T.F0074
{
	public class S3Values : IS3Values
	{
		#region Infrastructure

	    public static IS3Values Instance { get; } = new S3Values();

	    private S3Values()
	    {
        }

	    #endregion
	}
}