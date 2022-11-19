using System;


namespace R5T.F0074
{
	public class PermissionNames : IPermissionNames
	{
		#region Infrastructure

	    public static IPermissionNames Instance { get; } = new PermissionNames();

	    private PermissionNames()
	    {
        }

	    #endregion
	}
}