using System;

using R5T.T0131;


namespace R5T.F0074
{
	[ValuesMarker]
	public partial interface IURIs : IValuesMarker
	{
		public string AllUsersGrantee => "http://acs.amazonaws.com/groups/global/AllUsers";
	}
}