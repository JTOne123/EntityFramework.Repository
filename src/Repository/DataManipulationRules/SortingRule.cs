namespace DevOvercome.EntityFramework.Repository.DataManipulationRules
{
	public class SortingRule
	{
		public string Key { get; set; } // probably extract to interface.
		public SortDirectionEnum SortDirection { get; set; }

		public static SortingRule Default { get { return new SortingRule() { Key = "Id", SortDirection = SortDirectionEnum.Asc }; } }
	}

	public enum SortDirectionEnum
	{
		Asc,
		Desc
	}
}
