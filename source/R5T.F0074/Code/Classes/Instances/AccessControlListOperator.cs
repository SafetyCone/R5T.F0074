using System;


namespace R5T.F0074
{
	public class AccessControlListOperator : IAccessControlListOperator
	{
		#region Infrastructure

	    public static IAccessControlListOperator Instance { get; } = new AccessControlListOperator();

	    private AccessControlListOperator()
	    {
        }

	    #endregion
	}
}