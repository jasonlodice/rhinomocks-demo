using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgentDevice;
using Rhino.Mocks;

namespace AgentDeviceTest
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class GridIndexTest
	{
		private static IViewAgent viewAgent;
		private static CCViewDS viewDS;
		private static IConfigurationAgent configAgent;
		private static IPositioner positioner;
		private static GridIndexController gridIndexingController;

		/// <summary>
		/// Use TestInitialize to run code before running each test in the class 
		/// </summary>
		[TestInitialize]
		public void GridIndexTestInitialize()
		{
			// stub out a view agent and view DS
			viewAgent = MockRepository.GenerateStub<IViewAgent>();
			viewDS = new CCViewDS();

			// when the controller calls viewAgent.get return this DS
			viewAgent.Stub(x => x.Get()).Return(viewDS);

			// stub out a config agent
			configAgent = MockRepository.GenerateStub<IConfigurationAgent>();

			// stub out a positioner
			positioner = MockRepository.GenerateStub<IPositioner>();

			// create the controller
			gridIndexingController = new GridIndexController(viewAgent, configAgent, positioner);
		}

		[TestMethod]
		public void EnableGridIndexing_GridedViewKeyopTrueGridInstalled_GridIndexingEnabled()
		{
			// arrange
			viewDS.AddViewRow(new CCViewRow { Rule1 = "grid=true" });
			
            configAgent
                .Stub(x => x.GetBoolean(Arg.Is(CCConfigurationPaths.IndexingPath), Arg<bool>.Is.Anything))
                .Return(true);
			
            positioner.GridInstalled = true;

			// act
			gridIndexingController.EvaluateGridIndexing();

			// assert
			Assert.IsTrue(gridIndexingController.GridIndexingEnabled);
		}

		[TestMethod]
		public void EnableGridIndexing_GridedViewKeyopTrueGridNotInstalled_GridIndexingDisabled()
		{
			// arrange
			viewDS.AddViewRow(new CCViewRow { Rule1 = "grid=true" });
			
            configAgent
                .Stub(x => x.GetBoolean(Arg.Is(CCConfigurationPaths.IndexingPath), Arg<bool>.Is.Anything))
                .Return(true);

			positioner.GridInstalled = false;

			// act
			gridIndexingController.EvaluateGridIndexing();

			// assert
			Assert.IsFalse(gridIndexingController.GridIndexingEnabled);
		}

		[TestMethod]
		public void EnableGridIndexing_GridedViewKeyopFalseGridInstalled_GridIndexingEnabled()
		{
			// arrange
			viewDS.AddViewRow(new CCViewRow { Rule1 = "grid=true" });
			
            configAgent
                .Stub(x => x.GetBoolean(Arg.Is(CCConfigurationPaths.IndexingPath), Arg<bool>.Is.Anything))
                .Return(false);

			positioner.GridInstalled = true;

			// act
			gridIndexingController.EvaluateGridIndexing();

			// assert
			Assert.IsTrue(gridIndexingController.GridIndexingEnabled);
		}

		[TestMethod]
		public void StatusChanged_GridEnabledChanged_GridIndexingDisabled()
		{
			// arrange
			viewDS.AddViewRow(new CCViewRow { Rule1 = "grid=true" });
			configAgent
                .Stub(x => x.GetBoolean(Arg.Is(CCConfigurationPaths.IndexingPath), Arg<bool>.Is.Anything))
                .Return(true);

			positioner.GridInstalled = true;

			// set initial state of grid indexing
			gridIndexingController.EvaluateGridIndexing();

			// act
			positioner.GridInstalled = false;
			// raise the positioner status changed event
			positioner.Raise(x => x.StatusChanged += null, this, new EventArgs());

			// assert
			Assert.IsFalse(gridIndexingController.GridIndexingEnabled);
		}

		[TestMethod]
		public void StatusChanged_GridEnabledChanged_RefreshEventCalled()
		{
			// arrange
			viewDS.AddViewRow(new CCViewRow { Rule1 = "grid=true" });
			
            configAgent
                .Stub(x => x.GetBoolean(Arg.Is(CCConfigurationPaths.IndexingPath), Arg<bool>.Is.Anything))
                .Return(true);
			
            positioner.GridInstalled = true;
			
            // set initial state of grid indexing
			gridIndexingController.EvaluateGridIndexing();

			bool refreshFired = false;
			gridIndexingController.RefreshNeeded += (e,s) => refreshFired = true;

			// act
			positioner.GridInstalled = false;
			// raise the positioner status changed event
			positioner.Raise(x => x.StatusChanged += null, this, new EventArgs());

			// assert
			Assert.IsTrue(refreshFired, "Refresh event was fired to the view when grid enable changed");
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void ViewAgentGet_NullViewDS_ExceptionThrown()
		{
			// arrange
			viewDS = null;
			configAgent
                .Stub(x => x.GetBoolean(Arg.Is(CCConfigurationPaths.IndexingPath), Arg<bool>.Is.Anything))
                .Return(false);

			positioner.GridInstalled = true;

			// act
			gridIndexingController.EvaluateGridIndexing();

			// assert
			// throw
		}
	}
}
