using System;


namespace R5T.F0074
{
	public class GrantOperator : IGrantOperator
	{
		#region Infrastructure

	    public static IGrantOperator Instance { get; } = new GrantOperator();

	    private GrantOperator()
	    {
        }

	    #endregion
	}
}