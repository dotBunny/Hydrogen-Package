using System.Collections;
using System.Collections.Generic;
using Hydrogen;

[NUnit.Framework.TestFixtureAttribute]
public class HydrogenSerializationTest {
	
	private System.Text.StringBuilder _file;
	private string _processed;


	private Dictionary<string,string> _expectedData; 
	private Dictionary<string,string> _processedData;
	private bool _check;
	
	#region Hydrogen.Serialization.INI
	[NUnit.Framework.TestAttribute]
	public void Serialize_INI_DeserializeOneLine()
	{
		_file = new System.Text.StringBuilder();
		_file.AppendLine("Key=Value");

		_processedData = Hydrogen.Serialization.INI.Deserialize(_file.ToString(), '=');
	
		_expectedData = new Dictionary<string, string>();
		_expectedData.Add("Key", "Value");

		if ( _expectedData.IsDictionaryEqual(_processedData) ) _check = true;

		NUnit.Framework.Assert.AreEqual(true, _check, "Hydrogen.Serialization.INI.DeserializeOneLine");
	}

	[NUnit.Framework.TestAttribute]
	public void Serialize_INI_SerializeOneLine()
	{
		_file = new System.Text.StringBuilder();
		_file.AppendLine("Key=Value");

		_expectedData = new Dictionary<string, string>();
		_expectedData.Add("Key", "Value");
		
		_processed = Hydrogen.Serialization.INI.Serialize(_expectedData, '=');


		NUnit.Framework.Assert.AreEqual(_file.ToString(), _processed, "Hydrogen.Serialization.INI.SerializeOneLine");
	}

	#endregion
}