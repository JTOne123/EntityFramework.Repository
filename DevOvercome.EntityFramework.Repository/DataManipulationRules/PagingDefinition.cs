using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.DataManipulationRules
{
	public class PagingDefinition
	{
		public PagingRule Paging { get; set; }
		public SortingRule Sorting { get; set; }
	}
}
