using System;
using System.Reflection;

namespace SubnauticaUtils
{
	/// <summary>
	/// Class originally written by PrimeSonic. I liked it so much I added it to this project to simplify logging.
	/// All credit goes to them. This is a literal copy/paste.
	/// </summary>
	internal static class QuickLogger
	{
		private static readonly AssemblyName ModName = Assembly.GetExecutingAssembly().GetName();
		internal static bool DebugLogsEnabled = false;

		public static void Info(string msg, bool showOnScreen = false, AssemblyName callingAssembly = null)
		{
			Console.WriteLine("[" + (callingAssembly ?? ModName).Name + ":INFO] " + msg);
			if (!showOnScreen)
				return;
			ErrorMessage.AddMessage(msg);
		}

		public static void Debug(string msg, bool showOnScreen = false, AssemblyName callingAssembly = null)
		{
			if (!DebugLogsEnabled)
				return;
			Console.WriteLine("[" + (callingAssembly ?? ModName).Name + ":DEBUG] " + msg);
			if (!showOnScreen)
				return;
			ErrorMessage.AddDebug(msg);
		}

		public static void Error(string msg, bool showOnScreen = false, AssemblyName callingAssembly = null)
		{
			Console.WriteLine("[" + (callingAssembly ?? ModName).Name + ":ERROR] " + msg);
			if (!showOnScreen)
				return;
			ErrorMessage.AddError(msg);
		}

		public static void Error(string msg, Exception ex, AssemblyName callingAssembly = null) => Console.WriteLine("[" + (callingAssembly ?? ModName).Name + ":ERROR] " + msg + Environment.NewLine + ex.ToString());

		public static void Error(Exception ex, AssemblyName callingAssembly = null) => Console.WriteLine("[" + (callingAssembly ?? ModName).Name + ":ERROR] " + ex.ToString());

		public static void Warning(string msg, bool showOnScreen = false, AssemblyName callingAssembly = null)
		{
			Console.WriteLine("[" + (callingAssembly ?? ModName).Name + ":WARN] " + msg);
			if (!showOnScreen)
				return;
			ErrorMessage.AddWarning(msg);
		}

		public static string GetAssemblyVersion()
		{
			Version version = ModName.Version;
			if (version.Revision > 0)
				return string.Format("{0}.{1}.{2} rev:{3}", (object)version.Major, (object)version.Minor, (object)version.Build, (object)version.Revision);
			if (version.Build > 0)
				return string.Format("{0}.{1}.{2}", (object)version.Major, (object)version.Minor, (object)version.Build);
			return version.Minor > 0 ? string.Format("{0}.{1}.0", (object)version.Major, (object)version.Minor) : string.Format("{0}.0.0", (object)version.Major);
		}

		public static string GetAssemblyName() => ModName.Name;
	}
}
