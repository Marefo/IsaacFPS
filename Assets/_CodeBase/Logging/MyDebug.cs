using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _CodeBase.Logging
{
	public static class MyDebug
	{
		public static bool enable = true;

		public enum DebugColor
		{
			green,
			black,
			blue,
			cyan,
			white,
			yellow,
			red
		}

		public static void Log(object message, DebugColor color = DebugColor.black, GameObject gameObject = null)
		{
			if (!enable)
			{
				return;
			}

			Debug.Log(Message(message, color), gameObject);
		}

		public static void LogBold(object message, DebugColor color = DebugColor.black)
		{
			if (!enable)
			{
				return;
			}

			Debug.Log(Message(message, color, true));
		}

		public static void LogItalic(object message, DebugColor color = DebugColor.black)
		{
			if (!enable)
			{
				return;
			}

			Debug.Log(Message(message, color, false, true));
		}

		public static void LogBoldItalic(object message, DebugColor color = DebugColor.black)
		{
			if (!enable)
			{
				return;
			}

			Debug.Log(Message(message, color, true, true));
		}

		public static void LogTitle(object title, int level, DebugColor color)
		{
			if (!enable)
			{
				return;
			}

			LogBold(Title(title, level, color));
		}

		public static void LogObject<T>(T content, DebugColor color = DebugColor.black)
		{
			if (!enable)
			{
				return;
			}

			Debug.Log(Message(typeof(T).Name, color, true) + "\n" + ObjectMessage(content));
		}

		public static void LogList<T>(List<T> content)
		{
			if (!enable || content == null || content.Count <= 0)
			{
				return;
			}

			var result = Title(typeof(T).Name + " List", 0, DebugColor.green) + "\n\n";
			for (int i = 0; i < content.Count; i++)
			{
				result += Title(typeof(T).Name + "_" + i, 1, DebugColor.green) + "\n";
				result += ObjectMessage<T>(content[i]) + "\n";
			}

			result += Title("End", 0, DebugColor.green);

			Debug.Log(result);
		}

		public static void LogArray<T>(T[] content)
		{
			if (!enable || content == null || content.Length <= 0)
			{
				return;
			}

			var result = Title(typeof(T).Name + " List", 0, DebugColor.green) + "\n";
			for (int i = 0; i < content.Length; i++)
			{
				result += Title(typeof(T).Name + "_" + i, 1, DebugColor.green) + "\n";
				result += ObjectMessage<T>(content[i]) + "\n";
			}

			result += Title("End", 0, DebugColor.green);

			Debug.Log(result);
		}

		private static string Space(int number)
		{
			var space = "";
			for (int i = 0; i < number; i++)
			{
				space += " ";
			}

			return space;
		}

		private static string Separate(int number, string character = "=")
		{
			var result = "";
			for (int i = 0; i < number; i++)
			{
				result += "=";
			}

			return result;
		}

		private static string Message(object message, DebugColor color = DebugColor.black, bool bold = false, bool italic = false)
		{
			var content = string.Format("<color={0}>{1}</color>", color.ToString(), message);
			content = bold ? string.Format("<b>{0}</b>", content) : content;
			content = italic ? string.Format("<i>{0}</i>", content) : content;

			return content;
		}

		private static string Title(object title, int level, DebugColor color)
		{
			return Message(string.Format("{0}{1}{2}{1}", Space(level * 2), Separate(16 - level * 2), title), color);
		}

		private static string ObjectMessage<T>(T content)
		{
			var result = "";

			if (typeof(T) == typeof(Rigidbody)) return result;
			
			var fields = typeof(T).GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				result += Space(4) + Message(fields[i].Name + " : ", DebugColor.green, true) + fields[i].GetValue(content) + "\n";
			}

			var properties = typeof(T).GetProperties();
			var propertiesList = properties.Where(propery => propery.IsDefined(typeof(ObsoleteAttribute), true) == false).ToList();
			for (int i = 0; i < propertiesList.Count; i++)
			{
				result += Space(4) + Message(propertiesList[i].Name + " : ", DebugColor.green, true) +
				          propertiesList[i].GetValue(content, null) + "\n";
			}

			return result;
		}
	}
}