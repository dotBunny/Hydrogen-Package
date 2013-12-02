using System.Collections;
using System.Collections.Generic;
using Hydrogen;

[NUnit.Framework.TestFixtureAttribute]
public class HydrogenSerializationTest {
	
	private System.Text.StringBuilder _file;
	private string _processed;


	private List<KeyValuePair<string, string>> _expectedData; 
	private List<KeyValuePair<string, string>> _processedData;
	private bool _check;
	
	#region Hydrogen.Serialization.INI
	[NUnit.Framework.TestAttribute]
	public void Serialize_INI_DeserializeOneLine()
	{
		_file = new System.Text.StringBuilder();
		_file.AppendLine("Key=Value");

		_processedData = Hydrogen.Serialization.INI.Deserialize(_file.ToString(), '=');
	
		_expectedData = new List<KeyValuePair<string, string>>();
		_expectedData.Add(new KeyValuePair<string, string>("Key", "Value"));

		NUnit.Framework.Assert.AreEqual(
			true, 
			Hydrogen.Validate.ScrambledEquals(_expectedData, _processedData), 
			"Hydrogen.Serialization.INI.DeserializeOneLine");
	}

	[NUnit.Framework.TestAttribute]
	public void Serialize_INI_SerializeOneLine()
	{
		_file = new System.Text.StringBuilder();
		_file.AppendLine("Key=Value");

		_expectedData = new List<KeyValuePair<string, string>>();
		_expectedData.Add(new KeyValuePair<string, string>("Key", "Value"));
		
		_processed = Hydrogen.Serialization.INI.Serialize(_expectedData, '=');

		NUnit.Framework.Assert.AreEqual(_file.ToString(), _processed, "Hydrogen.Serialization.INI.SerializeOneLine");
	}

	#endregion
}