using System;

namespace DevOvercome.EntityFramework.Repository.Internals.Utils
{
	// from EF source code
	internal class Check
	{
		public static T NotNull<T>(T value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}

			return value;
		}

		public static T? NotNull<T>(T? value, string parameterName) where T : struct
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}

			return value;
		}

		public static string NotEmpty(string value, string parameterName)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("argument is null or empty - " + parameterName);
			}

			return value;
		}
	}


}
