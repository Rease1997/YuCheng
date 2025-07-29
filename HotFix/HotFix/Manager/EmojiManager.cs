
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class EmojiManager
    {
		private static List<string> patterns = new List<string>() { @"\p{Cs}", @"\p{Co}", @"\p{Cn}", @"[\u2702-\u27B0]" };

		public static InputField input;
		
		public static string lastStr;

		public static void Test()
		{
			patterns.Add(@"\p{Cs}");
			patterns.Add(@"[\u2702-\u27B0]");
			input.onValidateInput = OnValidateInput;
			input.onEndEdit.AddListener((arg0 =>
			{
				//tex.text = arg0;
			}));
			 
		}


		private static char OnValidateInput(string text, int charIndex, char addedChar)
		{
			if (patterns.Count > 0)
			{
				string s = string.Format("{0}", addedChar);
				if (BEmoji(s))
				{
					return '\0';
				}
			}
			return addedChar;
		}

		private static bool BEmoji(string s)
		{
			bool bEmoji = false;
			for (int i = 0; i < patterns.Count; ++i)
			{
				bEmoji = Regex.IsMatch(s, patterns[i]);
				if (bEmoji)
				{
					break;
				}
			}
			return bEmoji;
		}

		public static void AddPatterns(string s)
		{
			patterns.Add(s);
		}

		public static void ClearPatterns(string s)
		{
			patterns.Clear();
		}

    
    }
}
