using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals.Utils
{
	// from EF source code
	public class DebugCheck
	{
		[Conditional("DEBUG")]
		public static void NotNull<T>(T value) where T : class
		{
			Debug.Assert(value != null);
		}

		[Conditional("DEBUG")]
		public static void NotNull<T>(T? value) where T : struct
		{
			Debug.Assert(value != null);
		}

		[Conditional("DEBUG")]
		public static void NotEmpty(string value)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(value));
		}
	}


}
