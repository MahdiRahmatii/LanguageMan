using System;
using System.Collections.Generic;

namespace LanToArabic
{
    public class LanToArabic
    {
        /// <summary>
        /// Fix the specified string.
        /// </summary>
        /// <param name='str'>
        /// String to be fixed.
        /// </param>
        public static string Fix(string str)
        {
            return Fix(str, false, true);
        }

        public static string Fix(string str, bool rtl)
        {
            if (rtl)
                return Fix(str);
            else
            {
                string[] words = str.Split(' ');
                string result = "";
                string arabicToIgnore = "";
                foreach (string word in words)
                {
                    if (char.IsLower(word.ToLower()[word.Length / 2]))
                    {
                        result += Fix(arabicToIgnore) + word + " ";
                        arabicToIgnore = "";
                    }
                    else
                        arabicToIgnore += word + " ";
                }
                if (arabicToIgnore != "")
                    result += Fix(arabicToIgnore);

                return result;
            }
        }

        /// <summary>
        /// Fix the specified string with customization options.
        /// </summary>
        /// <param name='str'>
        /// String to be fixed.
        /// </param>
        /// <param name='showTashkeel'>
        /// Show tashkeel.
        /// </param>
        /// <param name='useHinduNumbers'>
        /// Use hindu numbers.
        /// </param>
        public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
        {
            ArabicFixerTool.showTashkeel = showTashkeel;
            ArabicFixerTool.useHinduNumbers = useHinduNumbers;

            if (str.Contains("\n"))
                str = str.Replace("\n", Environment.NewLine);

            if (str.Contains(Environment.NewLine))
            {
                string[] stringSeparators = new string[] { Environment.NewLine };
                string[] strSplit = str.Split(stringSeparators, StringSplitOptions.None);

                if (strSplit.Length == 0)
                    return ArabicFixerTool.FixLine(str);
                else if (strSplit.Length == 1)
                    return ArabicFixerTool.FixLine(str);
                else
                {
                    string outputString = ArabicFixerTool.FixLine(strSplit[0]);
                    int iteration = 1;
                    if (strSplit.Length > 1)
                    {
                        while (iteration < strSplit.Length)
                        {
                            outputString += Environment.NewLine + ArabicFixerTool.FixLine(strSplit[iteration]);
                            iteration++;
                        }
                    }
                    return outputString;
                }
            }
            else
                return ArabicFixerTool.FixLine(str);
        }
    }
}

/// <summary>
/// arabic Contextual forms General - Unicode
/// </summary>
internal enum IsolatedarabicLetters
{
	Hamza = 0xFE80,
	Alef = 0xFE8D,
	AlefHamza = 0xFE83,
	WawHamza = 0xFE85,
	AlefMaksoor = 0xFE87,
	AlefMaksora = 0xFBFC,
	HamzaNabera = 0xFE89,
	Ba = 0xFE8F,
	Ta = 0xFE95,
	Tha2 = 0xFE99,
	Jeem = 0xFE9D,
	H7aa = 0xFEA1,
	Khaa2 = 0xFEA5,
	Dal = 0xFEA9,
	Thal = 0xFEAB,
	Ra2 = 0xFEAD,
	Zeen = 0xFEAF,
	Seen = 0xFEB1,
	Sheen = 0xFEB5,
	S9a = 0xFEB9,
	Dha = 0xFEBD,
	T6a = 0xFEC1,
	T6ha = 0xFEC5,
	Ain = 0xFEC9,
	Gain = 0xFECD,
	Fa = 0xFED1,
	Gaf = 0xFED5,
	Kaf = 0xFED9,
	Lam = 0xFEDD,
	Meem = 0xFEE1,
	Noon = 0xFEE5,
	Ha = 0xFEE9,
	Waw = 0xFEED,
	Ya = 0xFBFC,//0xFEF1,
	Ya2 = 0xFBFC,//0xFEF1,
	AlefMad = 0xFE81,
	TaMarboota = 0xFE93,
	arabicPe = 0xFB56,  	// arabic Letters;
	arabicChe = 0xFB7A,
	arabicZe = 0xFB8A,
	arabicGaf = 0xFB92,
	arabicGaf2 = 0xFB8E
	
}

/// <summary>
/// arabic Contextual forms - Isolated
/// </summary>
internal enum GeneralarabicLetters
{
	Hamza = 0x0621,
	Alef = 0x0627,
	AlefHamza = 0x0623,
	WawHamza = 0x0624,
	AlefMaksoor = 0x0625,
	AlefMagsora = 0x0649,
	HamzaNabera = 0x0626,
	Ba = 0x0628,
	Ta = 0x062A,
	Tha2 = 0x062B,
	Jeem = 0x062C,
	H7aa = 0x062D,
	Khaa2 = 0x062E,
	Dal = 0x062F,
	Thal = 0x0630,
	Ra2 = 0x0631,
	Zeen = 0x0632,
	Seen = 0x0633,
	Sheen = 0x0634,
	S9a = 0x0635,
	Dha = 0x0636,
	T6a = 0x0637,
	T6ha = 0x0638,
	Ain = 0x0639,
	Gain = 0x063A,
	Fa = 0x0641,
	Gaf = 0x0642,
	Kaf = 0x0643,
	Lam = 0x0644,
	Meem = 0x0645,
	Noon = 0x0646,
	Ha = 0x0647,
	Waw = 0x0648,
	Ya = 0x064A,
	Ya2 = 0x06CC,
	AlefMad = 0x0622,
	TaMarboota = 0x0629,
	arabicPe = 0x067E,		// arabic Letters;
	arabicChe = 0x0686,
	arabicZe = 0x0698,
	arabicGaf = 0x06AF,
	arabicGaf2 = 0x06A9
	
}

/// <summary>
/// Data Structure for conversion
/// </summary>
internal class ArabicMapping
{
	public int from;
	public int to;
	public ArabicMapping(int from, int to)
	{
		this.from = from;
		this.to = to;
	}
}

/// <summary>
/// Sets up and creates the conversion table 
/// </summary>
internal class ArabicTable
{
	
	private static List<ArabicMapping> mapList;
	private static ArabicTable arabicMapper;
	
	/// <summary>
	/// Setting up the conversion table
	/// </summary>
	private ArabicTable()
	{
		mapList = new List<ArabicMapping>();
		
		
		
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Hamza, (int)IsolatedarabicLetters.Hamza));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Alef, (int)IsolatedarabicLetters.Alef));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.AlefHamza, (int)IsolatedarabicLetters.AlefHamza));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.WawHamza, (int)IsolatedarabicLetters.WawHamza));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.AlefMaksoor, (int)IsolatedarabicLetters.AlefMaksoor));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.AlefMagsora, (int)IsolatedarabicLetters.AlefMaksora));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.HamzaNabera, (int)IsolatedarabicLetters.HamzaNabera));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Ba, (int)IsolatedarabicLetters.Ba));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Ta, (int)IsolatedarabicLetters.Ta));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Tha2, (int)IsolatedarabicLetters.Tha2));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Jeem, (int)IsolatedarabicLetters.Jeem));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.H7aa, (int)IsolatedarabicLetters.H7aa));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Khaa2, (int)IsolatedarabicLetters.Khaa2));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Dal, (int)IsolatedarabicLetters.Dal));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Thal, (int)IsolatedarabicLetters.Thal));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Ra2, (int)IsolatedarabicLetters.Ra2));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Zeen, (int)IsolatedarabicLetters.Zeen));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Seen, (int)IsolatedarabicLetters.Seen));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Sheen, (int)IsolatedarabicLetters.Sheen));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.S9a, (int)IsolatedarabicLetters.S9a));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Dha, (int)IsolatedarabicLetters.Dha));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.T6a, (int)IsolatedarabicLetters.T6a));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.T6ha, (int)IsolatedarabicLetters.T6ha));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Ain, (int)IsolatedarabicLetters.Ain));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Gain, (int)IsolatedarabicLetters.Gain));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Fa, (int)IsolatedarabicLetters.Fa));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Gaf, (int)IsolatedarabicLetters.Gaf));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Kaf, (int)IsolatedarabicLetters.Kaf));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Lam, (int)IsolatedarabicLetters.Lam));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Meem, (int)IsolatedarabicLetters.Meem));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Noon, (int)IsolatedarabicLetters.Noon));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Ha, (int)IsolatedarabicLetters.Ha));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Waw, (int)IsolatedarabicLetters.Waw));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Ya, (int)IsolatedarabicLetters.Ya));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.Ya2, (int)IsolatedarabicLetters.Ya2));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.AlefMad, (int)IsolatedarabicLetters.AlefMad));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.TaMarboota, (int)IsolatedarabicLetters.TaMarboota));		
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.arabicPe, (int)IsolatedarabicLetters.arabicPe)); 		// arabic Letters;
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.arabicChe, (int)IsolatedarabicLetters.arabicChe));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.arabicZe, (int)IsolatedarabicLetters.arabicZe));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.arabicGaf, (int)IsolatedarabicLetters.arabicGaf));
		mapList.Add(new ArabicMapping((int)GeneralarabicLetters.arabicGaf2, (int)IsolatedarabicLetters.arabicGaf2));
		
		
		
		
		//for (int i = 0; i < generalarabic.Length; i++)
		//    mapList.Add(new arabicMapping((int)generalarabic.GetValue(i), (int)isolatedarabic.GetValue(i)));    // I
		
		
	}
	
	/// <summary>
	/// Singleton design pattern, Get the mapper. If it was not created before, create it.
	/// </summary>
	internal static ArabicTable ArabicMapper
	{
		get
		{
			if (arabicMapper == null)
				arabicMapper = new ArabicTable();
			return arabicMapper;
		}
	}
	
	internal int Convert(int toBeConverted)
	{
		
		foreach (ArabicMapping arabicMap in mapList)
			if (arabicMap.from == toBeConverted)
		{
			return arabicMap.to;
		}
		return toBeConverted;
	}
	
	
}


internal class TashkeelLocation
{
	public char tashkeel;
	public int position;
	public TashkeelLocation(char tashkeel, int position)
	{
		this.tashkeel = tashkeel;
		this.position = position;
	}
}


internal class ArabicFixerTool
{
	internal static bool showTashkeel = true;
	internal static bool useHinduNumbers = false;
	
	
	internal static string RemoveTashkeel(string str, out List<TashkeelLocation> tashkeelLocation)
	{
		tashkeelLocation = new List<TashkeelLocation>();
		char[] letters = str.ToCharArray();
		for (int i = 0; i < letters.Length; i++)
		{
			if(letters[i] == (char)0x064B) // Tanween Fatha
				tashkeelLocation.Add(new TashkeelLocation((char)0x064B, i));
			else if(letters[i] == (char)0x064C) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x064C, i));
			else if(letters[i] == (char)0x064D) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x064D, i));
			else if(letters[i] == (char)0x064E) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x064E, i));
			else if(letters[i] == (char)0x064F) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x064F, i));
			else if(letters[i] == (char)0x0650) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x0650, i));
			else if(letters[i] == (char)0x0651) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x0651, i));
			else if(letters[i] == (char)0x0652) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x0652, i));
			else if(letters[i] == (char)0x0653) // Shaddah
				tashkeelLocation.Add(new TashkeelLocation((char)0x0653, i));
			
			
			
		}
		
		string[] split = str.Split(new char[]{(char)0x064B,(char)0x064C,(char)0x064D,
			(char)0x064E,(char)0x064F,(char)0x0650,
			(char)0x0651,(char)0x0652,(char)0x0653,});
		
		str = "";
		
		foreach(string s in split)
		{
			str += s;
		}
		
		return str;
	}
	
	internal static char[] ReturnTashkeel(char[] letters, List<TashkeelLocation> tashkeelLocation)
	{
		char[] lettersWithTashkeel = new char[letters.Length + tashkeelLocation.Count];
		
		int letterWithTashkeelTracker = 0;
		for(int i = 0; i<letters.Length; i++)
		{
			lettersWithTashkeel[letterWithTashkeelTracker] = letters[i];
			letterWithTashkeelTracker++;
			foreach(TashkeelLocation hLocation in tashkeelLocation)
			{
				if(hLocation.position == letterWithTashkeelTracker)
				{
					lettersWithTashkeel[letterWithTashkeelTracker] = hLocation.tashkeel;
					letterWithTashkeelTracker++;
				}
			}
		}
		
		return lettersWithTashkeel;
	}
	
	/// <summary>
	/// Converts a string to a form in which the sting will be displayed correctly for arabic text.
	/// </summary>
	/// <param name="str">String to be converted. Example: "Aaa"</param>
	/// <returns>Converted string. Example: "aa aaa A" without the spaces.</returns>
	internal static string FixLine(string str)
	{
		string test = "";
		
		List<TashkeelLocation> tashkeelLocation;
		
		string originString = RemoveTashkeel(str, out tashkeelLocation);
		
		char[] lettersOrigin = originString.ToCharArray();
		char[] lettersFinal = originString.ToCharArray();
		

		
		for (int i = 0; i < lettersOrigin.Length; i++)
		{
			lettersOrigin[i] = (char)ArabicTable.ArabicMapper.Convert(lettersOrigin[i]);
		}
		
		for (int i = 0; i < lettersOrigin.Length; i++)
		{
			bool skip = false;

			
			//lettersOrigin[i] = (char)arabicTable.arabicMapper.Convert(lettersOrigin[i]);


			// For special Lam Letter connections.
			if (lettersOrigin[i] == (char)IsolatedarabicLetters.Lam)
			{
				
				if (i < lettersOrigin.Length - 1)
				{
					//lettersOrigin[i + 1] = (char)arabicTable.arabicMapper.Convert(lettersOrigin[i + 1]);
					if ((lettersOrigin[i + 1] == (char)IsolatedarabicLetters.AlefMaksoor))
					{
						lettersOrigin[i] = (char)0xFEF7;
						lettersFinal[i + 1] = (char)0xFFFF;
						skip = true;
					}
					else if ((lettersOrigin[i + 1] == (char)IsolatedarabicLetters.Alef))
					{
						lettersOrigin[i] = (char)0xFEF9;
						lettersFinal[i + 1] = (char)0xFFFF;
						skip = true;
					}
					else if ((lettersOrigin[i + 1] == (char)IsolatedarabicLetters.AlefHamza))
					{
						lettersOrigin[i] = (char)0xFEF5;
						lettersFinal[i + 1] = (char)0xFFFF;
						skip = true;
					}
					else if ((lettersOrigin[i + 1] == (char)IsolatedarabicLetters.AlefMad))
					{
						lettersOrigin[i] = (char)0xFEF3;
						lettersFinal[i + 1] = (char)0xFFFF;
						skip = true;
					}
				}
				
			}
			
			
			if (!IsIgnoredCharacter(lettersOrigin[i]))
			{
				if (IsMiddleLetter(lettersOrigin, i))
					lettersFinal[i] = (char)(lettersOrigin[i] + 3);
				else if (IsFinishingLetter(lettersOrigin, i))
					lettersFinal[i] = (char)(lettersOrigin[i] + 1);
				else if (IsLeadingLetter(lettersOrigin, i))
					lettersFinal[i] = (char)(lettersOrigin[i] + 2);
			}

            //string strOut = String.Format(@"\x{0:x4}", (ushort)lettersOrigin[i]);
            //UnityEngine.Debug.Log(strOut);

            //strOut = String.Format(@"\x{0:x4}", (ushort)lettersFinal[i]);
            //UnityEngine.Debug.Log(strOut);

            test += Convert.ToString((int)lettersOrigin[i], 16) + " ";
			if (skip)
				i++;
			
			
			//chaning numbers to hindu
			if(useHinduNumbers){
				if(lettersOrigin[i] == (char)0x0030)
					lettersFinal[i] = (char)0x0660;
				else if(lettersOrigin[i] == (char)0x0031)
					lettersFinal[i] = (char)0x0661;
				else if(lettersOrigin[i] == (char)0x0032)
					lettersFinal[i] = (char)0x0662;
				else if(lettersOrigin[i] == (char)0x0033)
					lettersFinal[i] = (char)0x0663;
				else if(lettersOrigin[i] == (char)0x0034)
					lettersFinal[i] = (char)0x0664;
				else if(lettersOrigin[i] == (char)0x0035)
					lettersFinal[i] = (char)0x0665;
				else if(lettersOrigin[i] == (char)0x0036)
					lettersFinal[i] = (char)0x0666;
				else if(lettersOrigin[i] == (char)0x0037)
					lettersFinal[i] = (char)0x0667;
				else if(lettersOrigin[i] == (char)0x0038)
					lettersFinal[i] = (char)0x0668;
				else if(lettersOrigin[i] == (char)0x0039)
					lettersFinal[i] = (char)0x0669;
			}
			
		}
		
		
		
		//Return the Tashkeel to their places.
		if(showTashkeel)
			lettersFinal = ReturnTashkeel(lettersFinal, tashkeelLocation);
		
		
		List<char> list = new List<char>();
		
		List<char> numberList = new List<char>();
		
		for (int i = lettersFinal.Length - 1; i >= 0; i--)
		{
			
			
			//				if (lettersFinal[i] == '(')
			//						numberList.Add(')');
			//				else if (lettersFinal[i] == ')')
			//					numberList.Add('(');
			//				else if (lettersFinal[i] == '<')
			//					numberList.Add('>');
			//				else if (lettersFinal[i] == '>')
			//					numberList.Add('<');
			//				else 
			if (char.IsPunctuation(lettersFinal[i]) && i>0 && i < lettersFinal.Length-1 &&
			    (char.IsPunctuation(lettersFinal[i-1]) || char.IsPunctuation(lettersFinal[i+1])))
			{
				if (lettersFinal[i] == '(')
					list.Add(')');
				else if (lettersFinal[i] == ')')
					list.Add('(');
				else if (lettersFinal[i] == '<')
					list.Add('>');
				else if (lettersFinal[i] == '>')
					list.Add('<');
				else if (lettersFinal[i] == '[')
					list.Add(']');
				else if (lettersFinal[i] == ']')
					list.Add('[');
				else if (lettersFinal[i] != 0xFFFF)
					list.Add(lettersFinal[i]);
			}
			// For cases where english words and arabic are mixed. This allows for using arabic, english and numbers in one sentence.
			else if(lettersFinal[i] == ' ' && i > 0 && i < lettersFinal.Length-1 &&
			        (char.IsLower(lettersFinal[i-1]) || char.IsUpper(lettersFinal[i-1]) || char.IsNumber(lettersFinal[i-1])) &&
			        (char.IsLower(lettersFinal[i+1]) || char.IsUpper(lettersFinal[i+1]) ||char.IsNumber(lettersFinal[i+1])))
				
			{
				numberList.Add(lettersFinal[i]);
			}
			
			else if (char.IsNumber(lettersFinal[i]) || char.IsLower(lettersFinal[i]) ||
			         char.IsUpper(lettersFinal[i]) || char.IsSymbol(lettersFinal[i]) ||
			         char.IsPunctuation(lettersFinal[i]))// || lettersFinal[i] == '^') //)
			{
				
				if (lettersFinal[i] == '(')
					numberList.Add(')');
				else if (lettersFinal[i] == ')')
					numberList.Add('(');
				else if (lettersFinal[i] == '<')
					numberList.Add('>');
				else if (lettersFinal[i] == '>')
					numberList.Add('<');
				else if (lettersFinal[i] == '[')
					list.Add(']');
				else if (lettersFinal[i] == ']')
					list.Add('[');
				else
					numberList.Add(lettersFinal[i]);
			}
			else if( (lettersFinal[i] >= (char)0xD800 && lettersFinal[i] <= (char)0xDBFF) ||
			        (lettersFinal[i] >= (char)0xDC00 && lettersFinal[i] <= (char)0xDFFF))
			{
				numberList.Add(lettersFinal[i]);
			}
			else
			{
				if (numberList.Count > 0)
				{
					for (int j = 0; j < numberList.Count; j++)
						list.Add(numberList[numberList.Count - 1 - j]);
					numberList.Clear();
				}
				if (lettersFinal[i] != 0xFFFF)
					list.Add(lettersFinal[i]);
				
			}
		}
		if (numberList.Count > 0)
		{
			for (int j = 0; j < numberList.Count; j++)
				list.Add(numberList[numberList.Count - 1 - j]);
			numberList.Clear();
		}
		
		// Moving letters from a list to an array.
		lettersFinal = new char[list.Count];
		for (int i = 0; i < lettersFinal.Length; i++)
			lettersFinal[i] = list[i];
		
		
		str = new string(lettersFinal);
		return str;
	}
	
	/// <summary>
	/// English letters, numbers and punctuation characters are ignored. This checks if the ch is an ignored character.
	/// </summary>
	/// <param name="ch">The character to be checked for skipping</param>
	/// <returns>True if the character should be ignored, false if it should not be ignored.</returns>
	internal static bool IsIgnoredCharacter(char ch)
	{
		bool isPunctuation = char.IsPunctuation(ch);
		bool isNumber = char.IsNumber(ch);
		bool isLower = char.IsLower(ch);
		bool isUpper = char.IsUpper(ch);
		bool isSymbol = char.IsSymbol(ch);
		bool isarabicCharacter = ch == (char)0xFB56 || ch == (char)0xFB7A || ch == (char)0xFB8A || ch == (char)0xFB92 || ch == (char)0xFB8E;
        bool isPresentationFormB = (ch <= (char)0xFEFF && ch >= (char)0xFE70);
        bool isAcceptableCharacter = isPresentationFormB || isarabicCharacter || ch == (char)0xFBFC;



        return isPunctuation ||
            isNumber ||
                isLower ||
                isUpper ||
                isSymbol ||
                !isAcceptableCharacter ||
                ch == 'a' || ch == '>' || ch == '<' || ch == (char)0x061B;
		
		//            return char.IsPunctuation(ch) || char.IsNumber(ch) || ch == 'a' || ch == '>' || ch == '<' ||
		//                    char.IsLower(ch) || char.IsUpper(ch) || ch == (char)0x061B || char.IsSymbol(ch)
		//					|| !(ch <= (char)0xFEFF && ch >= (char)0xFE70) // Presentation Form B
		//					|| ch == (char)0xFB56 || ch == (char)0xFB7A || ch == (char)0xFB8A || ch == (char)0xFB92; // arabic Characters
		
		//					arabicPe = 0xFB56,
		//		arabicChe = 0xFB7A,
		//		arabicZe = 0xFB8A,
		//		arabicGaf = 0xFB92
		//lettersOrigin[i] <= (char)0xFEFF && lettersOrigin[i] >= (char)0xFE70
	}
	
	/// <summary>
	/// Checks if the letter at index value is a leading character in arabic or not.
	/// </summary>
	/// <param name="letters">The whole word that contains the character to be checked</param>
	/// <param name="index">The index of the character to be checked</param>
	/// <returns>True if the character at index is a leading character, else, returns false</returns>
	internal static bool IsLeadingLetter(char[] letters, int index)
	{

		bool lettersThatCannotBeBeforeALeadingLetter = index == 0 
			|| letters[index - 1] == ' ' 
				|| letters[index - 1] == '*' // ??? Remove?
				|| letters[index - 1] == 'A' // ??? Remove?
				|| char.IsPunctuation(letters[index - 1])
				|| letters[index - 1] == '>' 
				|| letters[index - 1] == '<' 
				|| letters[index - 1] == (int)IsolatedarabicLetters.Alef
				|| letters[index - 1] == (int)IsolatedarabicLetters.Dal 
				|| letters[index - 1] == (int)IsolatedarabicLetters.Thal
				|| letters[index - 1] == (int)IsolatedarabicLetters.Ra2 
				|| letters[index - 1] == (int)IsolatedarabicLetters.Zeen 
				|| letters[index - 1] == (int)IsolatedarabicLetters.arabicZe
				//|| letters[index - 1] == (int)IsolatedarabicLetters.AlefMaksora 
				|| letters[index - 1] == (int)IsolatedarabicLetters.Waw
				|| letters[index - 1] == (int)IsolatedarabicLetters.AlefMad 
				|| letters[index - 1] == (int)IsolatedarabicLetters.AlefHamza
				|| letters[index - 1] == (int)IsolatedarabicLetters.AlefMaksoor 
				|| letters[index - 1] == (int)IsolatedarabicLetters.WawHamza;

		bool lettersThatCannotBeALeadingLetter = letters[index] != ' ' 
			&& letters[index] != (int)IsolatedarabicLetters.Dal
			&& letters[index] != (int)IsolatedarabicLetters.Thal
				&& letters[index] != (int)IsolatedarabicLetters.Ra2 
				&& letters[index] != (int)IsolatedarabicLetters.Zeen 
				&& letters[index] != (int)IsolatedarabicLetters.arabicZe
				&& letters[index] != (int)IsolatedarabicLetters.Alef 
				&& letters[index] != (int)IsolatedarabicLetters.AlefHamza
				&& letters[index] != (int)IsolatedarabicLetters.AlefMaksoor
				&& letters[index] != (int)IsolatedarabicLetters.AlefMad
				&& letters[index] != (int)IsolatedarabicLetters.WawHamza
				&& letters[index] != (int)IsolatedarabicLetters.Waw
				&& letters[index] != (int)IsolatedarabicLetters.Hamza;

		bool lettersThatCannotBeAfterLeadingLetter = index < letters.Length - 1 
			&& letters[index + 1] != ' '
				&& !char.IsPunctuation(letters[index + 1] )
				&& !char.IsNumber(letters[index + 1])
				&& !char.IsSymbol(letters[index + 1])
				&& !char.IsLower(letters[index + 1])
				&& !char.IsUpper(letters[index + 1])
				&& letters[index + 1] != (int)IsolatedarabicLetters.Hamza;

		if(lettersThatCannotBeBeforeALeadingLetter && lettersThatCannotBeALeadingLetter && lettersThatCannotBeAfterLeadingLetter)

//		if ((index == 0 || letters[index - 1] == ' ' || letters[index - 1] == '*' || letters[index - 1] == 'A' || char.IsPunctuation(letters[index - 1])
//		     || letters[index - 1] == '>' || letters[index - 1] == '<' 
//		     || letters[index - 1] == (int)IsolatedarabicLetters.Alef
//		     || letters[index - 1] == (int)IsolatedarabicLetters.Dal || letters[index - 1] == (int)IsolatedarabicLetters.Thal
//		     || letters[index - 1] == (int)IsolatedarabicLetters.Ra2 
//		     || letters[index - 1] == (int)IsolatedarabicLetters.Zeen || letters[index - 1] == (int)IsolatedarabicLetters.arabicZe
//		     || letters[index - 1] == (int)IsolatedarabicLetters.AlefMaksora || letters[index - 1] == (int)IsolatedarabicLetters.Waw
//		     || letters[index - 1] == (int)IsolatedarabicLetters.AlefMad || letters[index - 1] == (int)IsolatedarabicLetters.AlefHamza
//		     || letters[index - 1] == (int)IsolatedarabicLetters.AlefMaksoor || letters[index - 1] == (int)IsolatedarabicLetters.WawHamza) 
//		    && letters[index] != ' ' && letters[index] != (int)IsolatedarabicLetters.Dal
//		    && letters[index] != (int)IsolatedarabicLetters.Thal
//		    && letters[index] != (int)IsolatedarabicLetters.Ra2 
//		    && letters[index] != (int)IsolatedarabicLetters.Zeen && letters[index] != (int)IsolatedarabicLetters.arabicZe
//		    && letters[index] != (int)IsolatedarabicLetters.Alef && letters[index] != (int)IsolatedarabicLetters.AlefHamza
//		    && letters[index] != (int)IsolatedarabicLetters.AlefMaksoor
//		    && letters[index] != (int)IsolatedarabicLetters.AlefMad
//		    && letters[index] != (int)IsolatedarabicLetters.WawHamza
//		    && letters[index] != (int)IsolatedarabicLetters.Waw
//		    && letters[index] != (int)IsolatedarabicLetters.Hamza
//		    && index < letters.Length - 1 && letters[index + 1] != ' ' && !char.IsPunctuation(letters[index + 1] ) && !char.IsNumber(letters[index + 1])
//		    && letters[index + 1] != (int)IsolatedarabicLetters.Hamza )
		{
			return true;
		}
		else
			return false;
	}
	
	/// <summary>
	/// Checks if the letter at index value is a finishing character in arabic or not.
	/// </summary>
	/// <param name="letters">The whole word that contains the character to be checked</param>
	/// <param name="index">The index of the character to be checked</param>
	/// <returns>True if the character at index is a finishing character, else, returns false</returns>
	internal static bool IsFinishingLetter(char[] letters, int index)
	{
		bool lettersThatCannotBeBeforeAFinishingLetter = (index == 0) ? false : 
				letters[index - 1] != ' '
//				&& char.IsDigit(letters[index-1])
//				&& char.IsLower(letters[index-1])
//				&& char.IsUpper(letters[index-1])
//				&& char.IsNumber(letters[index-1])
//				&& char.IsWhiteSpace(letters[index-1])
//				&& char.IsPunctuation(letters[index-1])
//				&& char.IsSymbol(letters[index-1])

				&& letters[index - 1] != (int)IsolatedarabicLetters.Dal 
				&& letters[index - 1] != (int)IsolatedarabicLetters.Thal
				&& letters[index - 1] != (int)IsolatedarabicLetters.Ra2 
				&& letters[index - 1] != (int)IsolatedarabicLetters.Zeen 
				&& letters[index - 1] != (int)IsolatedarabicLetters.arabicZe
				//&& letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksora 
				&& letters[index - 1] != (int)IsolatedarabicLetters.Waw
				&& letters[index - 1] != (int)IsolatedarabicLetters.Alef 
				&& letters[index - 1] != (int)IsolatedarabicLetters.AlefMad
				&& letters[index - 1] != (int)IsolatedarabicLetters.AlefHamza 
				&& letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksoor
				&& letters[index - 1] != (int)IsolatedarabicLetters.WawHamza 
				&& letters[index - 1] != (int)IsolatedarabicLetters.Hamza



				&& !char.IsPunctuation(letters[index - 1]) 
				&& letters[index - 1] != '>' 
				&& letters[index - 1] != '<';
				

		bool lettersThatCannotBeFinishingLetters = letters[index] != ' ' && letters[index] != (int)IsolatedarabicLetters.Hamza;

	


		if(lettersThatCannotBeBeforeAFinishingLetter && lettersThatCannotBeFinishingLetters)

//		if (index != 0 && letters[index - 1] != ' ' && letters[index - 1] != '*' && letters[index - 1] != 'A'
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Dal && letters[index - 1] != (int)IsolatedarabicLetters.Thal
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Ra2 
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Zeen && letters[index - 1] != (int)IsolatedarabicLetters.arabicZe
//		    && letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksora && letters[index - 1] != (int)IsolatedarabicLetters.Waw
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Alef && letters[index - 1] != (int)IsolatedarabicLetters.AlefMad
//		    && letters[index - 1] != (int)IsolatedarabicLetters.AlefHamza && letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksoor
//		    && letters[index - 1] != (int)IsolatedarabicLetters.WawHamza && letters[index - 1] != (int)IsolatedarabicLetters.Hamza 
//		    && !char.IsPunctuation(letters[index - 1]) && letters[index - 1] != '>' && letters[index - 1] != '<' 
//		    && letters[index] != ' ' && index < letters.Length
//		    && letters[index] != (int)IsolatedarabicLetters.Hamza)
		{
			//try
			//{
			//    if (char.IsPunctuation(letters[index + 1]))
			//        return true;
			//    else
			//        return false;
			//}
			//catch (Exception e)
			//{
			//    return false;
			//}
			
			return true;
		}
		//return true;
		else
			return false;
	}
	
	/// <summary>
	/// Checks if the letter at index value is a middle character in arabic or not.
	/// </summary>
	/// <param name="letters">The whole word that contains the character to be checked</param>
	/// <param name="index">The index of the character to be checked</param>
	/// <returns>True if the character at index is a middle character, else, returns false</returns>
	internal static bool IsMiddleLetter(char[] letters, int index)
	{
		bool lettersThatCannotBeMiddleLetters = (index == 0) ? false : 
			letters[index] != (int)IsolatedarabicLetters.Alef 
				&& letters[index] != (int)IsolatedarabicLetters.Dal
				&& letters[index] != (int)IsolatedarabicLetters.Thal 
				&& letters[index] != (int)IsolatedarabicLetters.Ra2
				&& letters[index] != (int)IsolatedarabicLetters.Zeen 
				&& letters[index] != (int)IsolatedarabicLetters.arabicZe 
				//&& letters[index] != (int)IsolatedarabicLetters.AlefMaksora
				&& letters[index] != (int)IsolatedarabicLetters.Waw 
				&& letters[index] != (int)IsolatedarabicLetters.AlefMad
				&& letters[index] != (int)IsolatedarabicLetters.AlefHamza 
				&& letters[index] != (int)IsolatedarabicLetters.AlefMaksoor
				&& letters[index] != (int)IsolatedarabicLetters.WawHamza 
				&& letters[index] != (int)IsolatedarabicLetters.Hamza;

		bool lettersThatCannotBeBeforeMiddleCharacters = (index == 0) ? false :
				letters[index - 1] != (int)IsolatedarabicLetters.Alef 
				&& letters[index - 1] != (int)IsolatedarabicLetters.Dal
				&& letters[index - 1] != (int)IsolatedarabicLetters.Thal 
				&& letters[index - 1] != (int)IsolatedarabicLetters.Ra2
				&& letters[index - 1] != (int)IsolatedarabicLetters.Zeen 
				&& letters[index - 1] != (int)IsolatedarabicLetters.arabicZe 
				//&& letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksora
				&& letters[index - 1] != (int)IsolatedarabicLetters.Waw 
				&& letters[index - 1] != (int)IsolatedarabicLetters.AlefMad
				&& letters[index - 1] != (int)IsolatedarabicLetters.AlefHamza 
				&& letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksoor
				&& letters[index - 1] != (int)IsolatedarabicLetters.WawHamza 
				&& letters[index - 1] != (int)IsolatedarabicLetters.Hamza
				&& !char.IsPunctuation(letters[index - 1])
				&& letters[index - 1] != '>' 
				&& letters[index - 1] != '<' 
				&& letters[index - 1] != ' ' 
				&& letters[index - 1] != '*';

		bool lettersThatCannotBeAfterMiddleCharacters = (index >= letters.Length - 1) ? false :
			letters[index + 1] != ' ' 
				&& letters[index + 1] != '\r' 
				&& letters[index + 1] != (int)IsolatedarabicLetters.Hamza
				&& !char.IsNumber(letters[index + 1])
				&& !char.IsSymbol(letters[index + 1])
				&& !char.IsPunctuation(letters[index + 1]);
		if(lettersThatCannotBeAfterMiddleCharacters && lettersThatCannotBeBeforeMiddleCharacters && lettersThatCannotBeMiddleLetters)

//		if (index != 0 && letters[index] != ' '
//		    && letters[index] != (int)IsolatedarabicLetters.Alef && letters[index] != (int)IsolatedarabicLetters.Dal
//		    && letters[index] != (int)IsolatedarabicLetters.Thal && letters[index] != (int)IsolatedarabicLetters.Ra2
//		    && letters[index] != (int)IsolatedarabicLetters.Zeen && letters[index] != (int)IsolatedarabicLetters.arabicZe 
//		    && letters[index] != (int)IsolatedarabicLetters.AlefMaksora
//		    && letters[index] != (int)IsolatedarabicLetters.Waw && letters[index] != (int)IsolatedarabicLetters.AlefMad
//		    && letters[index] != (int)IsolatedarabicLetters.AlefHamza && letters[index] != (int)IsolatedarabicLetters.AlefMaksoor
//		    && letters[index] != (int)IsolatedarabicLetters.WawHamza && letters[index] != (int)IsolatedarabicLetters.Hamza
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Alef && letters[index - 1] != (int)IsolatedarabicLetters.Dal
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Thal && letters[index - 1] != (int)IsolatedarabicLetters.Ra2
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Zeen && letters[index - 1] != (int)IsolatedarabicLetters.arabicZe 
//		    && letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksora
//		    && letters[index - 1] != (int)IsolatedarabicLetters.Waw && letters[index - 1] != (int)IsolatedarabicLetters.AlefMad
//		    && letters[index - 1] != (int)IsolatedarabicLetters.AlefHamza && letters[index - 1] != (int)IsolatedarabicLetters.AlefMaksoor
//		    && letters[index - 1] != (int)IsolatedarabicLetters.WawHamza && letters[index - 1] != (int)IsolatedarabicLetters.Hamza 
//		    && letters[index - 1] != '>' && letters[index - 1] != '<' 
//		    && letters[index - 1] != ' ' && letters[index - 1] != '*' && !char.IsPunctuation(letters[index - 1])
//		    && index < letters.Length - 1 && letters[index + 1] != ' ' && letters[index + 1] != '\r' && letters[index + 1] != 'A' 
//		    && letters[index + 1] != '>' && letters[index + 1] != '>' && letters[index + 1] != (int)IsolatedarabicLetters.Hamza
//		    )
		{
			try
			{
				if (char.IsPunctuation(letters[index + 1]))
					return false;
				else
					return true;
			}
			catch
			{
				return false;
			}
			//return true;
		}
		else
			return false;
	}
	
	
}