using System;


namespace R5T.F0074
{
	public class S3ObjectKeyValues : IS3ObjectKeyValues
	{
		#region Infrastructure

	    public static IS3ObjectKeyValues Instance { get; } = new S3ObjectKeyValues();

	    private S3ObjectKeyValues()
	    {
        }

	    #endregion
	}
}