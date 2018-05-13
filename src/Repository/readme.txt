Welcome to DI-aware flexible DbContext wrapper!

Entry points:
	- IBasicRepository - contains generic methods to access any table or view under context
	- IBasicTableRepository - same, but methods are limited to single table or view under context.

	You can find them at DevOvercome.EntityFramework.Repository.RepositoryFactory
	This is basically DI-container helper, in case if your container doesnt see internal classes.
	Also, this factory can be inherited in order to return your custom repositories. Just in case.

// TODO: move to wiki.
Simple example for ASP.NET with Ninject
(Note that I ommited a lot of code, to point important parts)

	NinjectWebCommon.cs must contain this directive:
		kernel.Bind(x =>
		{
			// Have only ONE implementation of ONE interface. 
			// Like we used to UserManager : IUserManager. 
			// Please follow this rule and you will be fine
			try
			{
				x.FromAssembliesMatching("*")
					.IncludingNonPublicTypes()
					.SelectAllClasses()
					.BindDefaultInterface();
			}
			catch(ReflectionTypeLoadException rex)
			{
				// cant reproduce locally
				throw rex.LoaderExceptions[0];
			}
		});

	See my list of ninject-packages where it works:
	  <package id="Ninject" version="3.3.4" targetFramework="net471" />
	  <package id="Ninject.Extensions.Conventions" version="3.3.0" targetFramework="net471" />
	  <package id="Ninject.Extensions.Factory" version="3.3.2" targetFramework="net471" />
	  <package id="Ninject.MVC5" version="3.3.0" targetFramework="net471" />
	  <package id="Ninject.Web.Common" version="3.3.0" targetFramework="net471" />
	  <package id="Ninject.Web.Common.WebHost" version="3.3.0" targetFramework="net471" />

	Now, when you have DI-container, you can just ask for repository in Controller constructor just like that:
		public HomeController(
			IBasicTableRepository<vw_someView> viewRepository, 
			IBasicTableRepository<table1> table1Repository)
		{
			this.viewRepository = viewRepository;
		}

		and call the internal INTERFACE property which has been initialized by IMPLEMENTATION

		// see get
		public ActionResult GetView()
		{
			var res = viewRepository
				.BuildFetch()
				.AddFilter(x => x.type = 'someType')
				.AddFilter(x => x.dateCreated >= DateTime.Now) // whatever
				// not required, but you can sort. only once.
				.AddSorting("id", SortDirectionEnum.Asc) 
				.Fetch(); // or FetchAsync in async actions
		}

		// see get with paging
		public ActionResult GetView(int pageIndex, int pageSize)
		{
			var res = viewRepository
				.BuildFetch()
				.AsPagingBuilder()
				.SetPaging(pageIndex, pageSize)
				.FetchPaging(); // or FetchPagingAsync

			// res is PagingResult class
			return res.Items; // actual items
		}

		// see save
		public ActionResult AddItem(table1 item){
			table1Repository
				.AddItem(item)
				.Save(); // or SaveAsync
		}

		// see get with includes!
		public ActionResult GetItemWithCategories(int id)
		{
			var res = table1Repository
				.BuildFetch()
				.Include(x => x.table2)
				.AddFilter(x => x.id == id)
				.FetchOne(); // or fetch one async

			// object is in context, you can update it
			res.dateLastViewed = DateTime.UtcNow;
			table1Repository
				.UpdateItem(res)
				.Save();
		}

		// force context loading!
		public ActionResult BulkUpdate(string status, int[] ids)
		{
			table1Reposisotry
				.BuildFetch()
				.AddFilter(x => ids.Contains(x.id))
				.SetNoTracking(false)
				.Fetch()
				.ForEach(x =>
				{
					x.status = status;
					table1Repository.UpdateItem(x);
				});

			table1Repository.BuildSave().Save();
		}

	Please note that 
	- multiple results like Fetch or PagingFetch forces NoTracking=true. 
	- single results like FetchOne forces NoTracking=false.
	You can override it in any particular query, as shown above.

	This is a basic usage.

Advanced repositories
	The situation, where DAL must work with couple of table as one, can be very often occured.
	In order to encapsulate that in single class, 
	feel free to introduce your own repository, 
	with your own methods,
	with NO FOLLOWING IBasicRepository pattern.
	How? A wrapper!

	// your name could be different, of course. Just follow this pattern
	// also I strongly advise to follow INTERFACE-IMPLEMENTATION pattern - better for DI-container, testable, and with no sweat.
	public class CoupledRepository : ICoupledRepository 
	{
		public CoupledRepository(
			IBasicRepository wholeContext,  // way too abstract. Dont recommend to abuse it - makes your repo's contract blurry. However, it works like sharm for tiny cases.
			IBasicTableRepository<table1> justTable1Repository,  // good way to keep your class's contract clear.
			IBasicTableRepository<table2> justTable2Repository, 
			)
			{
				// initialization ommited
			}
		public void DoWork(table1 t1, table2 t2, table3 t3){
		    justTable1Repository.Update(t1);
			justTable2Repository.Update(t2);

			wholeContext<table3>
				.Update(t3)
				.Save(); // you can call Save at any time, on any repository, since DbContext behind them is singleton per request
		}
	}





Guides and best practices
	WARNING!
	Please, be adviced - you can face the situation when constructor contains too much dependencies. 
	It is not fine! But you actually can live with that.
	Hovewer, the repository wrappers (see above) are strongly recommended.

	IBasicRepository vs IBasicTableRepository
	I strongly recommend to use IBasicTableRepository in order to keep contract and class's responsibility clear.
	Hovewer, there is still a place for IBasicRepository 
		- if class should invoke just one operation for one of dozens tables 
		- this is the case where I personally prefer blurry single object that dozens specific noisy TableRepositories

	Keep in mind that you are free to make any complicated dependencies without bothering for wiring. 
	All implementations will be shared per request.

This concludes Welcome.txt. 
Feel free to explore further opportunities which DevOvercome.Repository offers! 
It is designed to serve wide range of cases (mean, less restrictions)