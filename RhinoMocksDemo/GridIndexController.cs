using System;

namespace AgentDevice
{
	public class GridIndexController : CCControllerBase
	{
		private Guid currentImageViewGuid;
		private IViewAgent viewAgent;
		private IConfigurationAgent configAgent;
		private IPositioner positioner;
		public bool GridIndexingEnabled { get; set; }
		public event EventHandler RefreshNeeded;

		/// <summary>
		/// Base Constructor
		/// </summary>
		public GridIndexController(Navigator navigator) : base(navigator)
		{
			viewAgent = new CCViewAgent();
			configAgent = new CCConfigurationAgent();
			positioner = new CCPositioner();
		}

		/// <summary>
		/// Alternate constructor for dependency injection
		/// </summary>
		public GridIndexController(IViewAgent va, IConfigurationAgent ca, IPositioner p)
		{
			viewAgent = va;
			configAgent = ca;
			positioner = p;
			positioner.StatusChanged += OnPositionerStatusChanged;
		}

		/// <summary>
		/// Status changed at the positioner
		/// </summary>
		private void OnPositionerStatusChanged(object sender, EventArgs e)
		{
			var currentGridIndex = GridIndexingEnabled;

			// re-evaluate gird indexing if the hardware value has changed
			EvaluateGridIndexing();

			var newGridIndex = GridIndexingEnabled;

			// let the view know if something changed
			if (currentGridIndex != newGridIndex)
			{
				if (RefreshNeeded != null)
				{
					RefreshNeeded(this, null);
				}
			}
		}

		/// <summary>
		/// Evaluate if grid indexing should be enabled 
		/// </summary>
		public void EvaluateGridIndexing()
		{
			bool griddedView = false;
			bool keyopEnabled = false;
			bool gridInstalled = false;

			try
			{
				// Is grid on for the current view
				if (currentImageViewGuid != null)
				{
					var viewDS = viewAgent.Get();
					var viewRow = viewDS.FindViewFromGuid(currentImageViewGuid);
					griddedView = (viewRow.Rule1 == "grid=true");
				}

				// has key-op enabled grid indexing
				keyopEnabled = configAgent.GetBoolean(CCConfigurationPaths.IndexingPath + "GridIndexing", false);

				// is the actual grid installed on the detector
				gridInstalled = positioner.GridInstalled;

				// if the grid is installed then always enable
				if (gridInstalled)
				{
					GridIndexingEnabled = true;
				}
				else
				{
					// grid is not installed, only enable if both the keyop setting is true and this is a gridded iew 
					if (keyopEnabled && griddedView)
					{
						GridIndexingEnabled = true;
					}
					else
					{
						GridIndexingEnabled = false;
					}
				}
			}
			catch (Exception e)
			{
				CCLogger.Log(CCLogLevel.Error, "Error evaluating grid indexing:", e);
				throw;
			}
		}
	}
}
