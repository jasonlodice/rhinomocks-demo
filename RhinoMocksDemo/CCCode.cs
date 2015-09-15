// This is stubed code from common platform
using System;
using System.Data;

namespace AgentDevice
{
	public class CCCode
	{
		static void Main(string[] args) { }
	}

	#region *** cfg Agent ***
	public interface IConfigurationAgent
	{
		bool GetBoolean(string empty, bool b);
		void SetValue(string empty, string s, bool b);
	}

	public class CCConfigurationAgent : IConfigurationAgent
	{
		public bool GetBoolean(string empty, bool b)
		{
			throw new NotImplementedException();
		}

		public void SetValue(string empty, string s, bool b)
		{
			throw new NotImplementedException();
		}
	}
	public static class CCConfigurationPaths
	{
		public static string IndexingPath
		{
			get { return @"\Capture\DR\System\Indexing\GridPresent";} 
		}
	}
	#endregion *** cfg Agent ***


	#region *** positioner ***

	public interface IPositioner
	{
		bool GridInstalled { get; set; }
		event EventHandler StatusChanged;
	}

	public class CCPositioner : IPositioner
	{
		public bool GridInstalled { get; set; }
		public event EventHandler StatusChanged;
	}
	#endregion *** positioner ***


	#region *** View Agent ***
	public interface IViewAgent
	{
		CCViewDS Get();
	}

	public class CCViewAgent : IViewAgent
	{
		public CCViewDS Get()
		{
			var viewDS = new CCViewDS(); //procedureMappingData.PopulateViews();
			return viewDS;
		}
	}

	public class CCViewDS : DataSet
	{
		public CCViewRow ViewRow { get; set; }

		public CCViewRow FindViewFromGuid(Guid currentImageViewGuid)
		{
			return ViewRow;
		}

		public void AddViewRow(CCViewRow ccViewRow)
		{
			ViewRow = ccViewRow;
		}
	}

	public class CCViewRow
	{
		public string Rule1 { get; set; }
	}
	#endregion *** View Agent ***


	public enum CCLogLevel
	{
		Error,
		Warning
	}
	public static class CCLogger
	{
		public static void Log(CCLogLevel ll, string empty, Exception exception)
		{
		}
	}

	public class CCControllerBase {protected CCControllerBase(Navigator navigator){}protected CCControllerBase(){}}
	public class Navigator { }
}
