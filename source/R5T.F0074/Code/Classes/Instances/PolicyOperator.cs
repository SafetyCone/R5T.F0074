using System;


namespace R5T.F0074
{
	public class PolicyOperator : IPolicyOperator
	{
		#region Infrastructure

	    public static IPolicyOperator Instance { get; } = new PolicyOperator();

	    private PolicyOperator()
	    {
        }

	    #endregion
	}
}