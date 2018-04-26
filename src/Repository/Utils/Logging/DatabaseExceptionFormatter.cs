using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace DevOvercome.EntityFramework.Repository.Utils.Logging
{
	public static class DatabaseExceptionFormatter
	{

		public static List<DatabaseExceptionInfo> TryParse(this Exception ex)
		{
			if (!(ex is DataException))
			{
				return new List<DatabaseExceptionInfo>();

			}


			if (ex is DbEntityValidationException)
			{
				return ParseEntityException((DbEntityValidationException)ex);
			}

			if (ex is DbUpdateException)
			{
				return ParseDbUpdateException((DbUpdateException)ex);
			}

			if (ex is UpdateException)
			{
				return ParseUpdateException((UpdateException)ex);
			}


			return new List<DatabaseExceptionInfo>();
		}

		private static List<DatabaseExceptionInfo> ParseDbUpdateException(DbUpdateException ex)
		{
			return ex.Entries.Select(x => FromEntry(x)).ToList();
		}

		private static List<DatabaseExceptionInfo> ParseUpdateException(UpdateException ex)
		{
			return ex.StateEntries.Select(x =>
			{
				var db = FromEntry(x);
				db.ValidationErrors.Add(new DatabaseExceptionValidationError()
				{
					Error = ex.InnerException != null ? ex.InnerException.Message : ""
				});
				return db;
			})
				.ToList();
		}


		private static DatabaseExceptionInfo FromEntry(DbEntityEntry entry)
		{
			return new DatabaseExceptionInfo()
			{
				EntityType = entry.Entity.GetType().Name,
				EntityState = entry.State.ToString()
			};
		}

		private static DatabaseExceptionInfo FromEntry(ObjectStateEntry entry)
		{
			return new DatabaseExceptionInfo()
			{
				EntityType = entry.EntitySet.Name,
				EntityState = entry.State.ToString()
			};
		}

		private static List<DatabaseExceptionInfo> ParseEntityException(DbEntityValidationException ex)
		{
			var res = new List<DatabaseExceptionInfo>();
			foreach (var eve in ex.EntityValidationErrors)
			{
				var item = FromEntry(eve.Entry);

				item.ValidationErrors = eve.ValidationErrors.Select(x => new DatabaseExceptionValidationError()
				{
					Error = x.ErrorMessage,
					Property = x.PropertyName
				}).ToList();

				res.Add(item);
			}

			return res;
		}
	}
}
