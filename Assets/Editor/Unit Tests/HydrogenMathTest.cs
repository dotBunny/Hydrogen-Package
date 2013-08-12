
[NUnit.Framework.TestFixtureAttribute]
public class HydrogenMathTest {
	
	private float _actualFloat;
	private float _expectedFloat; 

	#region Hydrogen.Math.UnsignedAngle
	[NUnit.Framework.TestAttribute]
	public void UnsignedAngle_90()
	{
		_actualFloat = Hydrogen.Math.UnsignedAngle(90f);
		_expectedFloat = 90f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.UnsignedAngle(90f)");
	}
	
	[NUnit.Framework.TestAttribute]
	public void UnsignedAngle_181()
	{
		_actualFloat = Hydrogen.Math.UnsignedAngle(181f);
		_expectedFloat = 181f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.UnsignedAngle(181f)");
	}
	
	[NUnit.Framework.TestAttribute]
	public void UnsignedAngle_380()
	{
		_actualFloat = Hydrogen.Math.UnsignedAngle(380f);
		_expectedFloat = 380f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.UnsignedAngle(380f)");
	}
	
	[NUnit.Framework.TestAttribute]
	public void UnsignedAngle_N90()
	{
		_actualFloat = Hydrogen.Math.UnsignedAngle(-90f);
		_expectedFloat = 270f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.UnsignedAngle(-90f)");
	}
	[NUnit.Framework.TestAttribute]
	public void UnsignedAngle_N180()
	{
		_actualFloat = Hydrogen.Math.UnsignedAngle(-180f);
		_expectedFloat = 180f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.UnsignedAngle(-180f)");
	}
	
	[NUnit.Framework.TestAttribute]
	public void UnsignedAngle_N380()
	{
		_actualFloat = Hydrogen.Math.UnsignedAngle(-380f);
		_expectedFloat = 700f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.UnsignedAngle(-380f)");
	}
	[NUnit.Framework.TestAttribute]
	public void UnsignedAngle_N980()
	{
		_actualFloat = Hydrogen.Math.UnsignedAngle(-980f);
		_expectedFloat = 820f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.UnsignedAngle(-980f)");
	}
	#endregion
	
	#region Hydrogen.Math.SignedAngle
	
	[NUnit.Framework.TestAttribute]
	public void SignedAngle_90()
	{
		_actualFloat = Hydrogen.Math.SignedAngle(90f);
		_expectedFloat = 90f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.SignedAngle(90f)");
	}	
	
	[NUnit.Framework.TestAttribute]
	public void SignedAngle_181()
	{	
		_actualFloat = Hydrogen.Math.SignedAngle(181f);
		_expectedFloat = -179f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.SignedAngle(181f)");
	}
		
	[NUnit.Framework.TestAttribute]
	public void SignedAngle_380()
	{
		_actualFloat = Hydrogen.Math.SignedAngle(380f);
		_expectedFloat = 20f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.SignedAngle(380f)");
	}
			
	[NUnit.Framework.TestAttribute]
	public void SignedAngle_N90()
	{
		_actualFloat = Hydrogen.Math.SignedAngle(-90f);
		_expectedFloat = -90f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.SignedAngle(-90f)");
	}
		
	[NUnit.Framework.TestAttribute]
	public void SignedAngle_N380()
	{	
		_actualFloat = Hydrogen.Math.SignedAngle(-380f);
		_expectedFloat = -20f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.SignedAngle(-380f)");
	}
		
	[NUnit.Framework.TestAttribute]
	public void SignedAngle_N181()
	{
		_actualFloat = Hydrogen.Math.SignedAngle(-181f);
		_expectedFloat = 1f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.SignedAngle(-181f)");		
	}
		
	[NUnit.Framework.TestAttribute]
	public void SignedAngle_N980()
	{
		_actualFloat = Hydrogen.Math.SignedAngle(-980f);
		_expectedFloat = 80f;
		NUnit.Framework.Assert.AreEqual(_expectedFloat, _actualFloat, "Hydrogen.Math.SignedAngle(-980f)");	
	}
	
	#endregion
}