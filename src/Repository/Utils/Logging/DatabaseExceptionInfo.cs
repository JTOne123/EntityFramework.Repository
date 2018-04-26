using System.Collections.Generic;

namespace DevOvercome.EntityFramework.Repository.Utils.Logging
{
	public sealed class DatabaseExceptionInfo
	{
		public string EntityType { get; set; }
		public string EntityState { get; set; }
		public List<DatabaseExceptionValidationError> ValidationErrors { get; set; }

		public DatabaseExceptionInfo()
		{
			ValidationErrors = new List<DatabaseExceptionValidationError>();
		}

	}

	public sealed class DatabaseExceptionValidationError
	{
		public string Property { get; set; }
		public string Error { get; set; }

	}
}
