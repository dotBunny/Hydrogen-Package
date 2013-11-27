using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hydrogen;

public class InputExample : MonoBehaviour
{
	public GameObject turret;
	public GameObject muzzle;
	public GameObject[] ammo;

	private string _fileHolder;

	void Awake()
	{
	}

	void Start()
	{
		hInput.Instance.AddAction("Move", OnMove);
		hInput.Instance.AddAction("Turn", OnTurn);
		hInput.Instance.AddAction("Rotate", OnRotate);
		//hInput.Instance.AddAction("Shoot", OnShoot);
		
		hInput.Instance.AddControl("Mouse X", "Rotate");
		hInput.Instance.AddControl("Horizontal", "Turn");
		hInput.Instance.AddControl("Vertical", "Move");
		//hInput.Instance.AddControl("Left", "Shoot");
		//hInput.Instance.AddControl("Space", "Shoot");

	

	}

	public void OnGUI()
	{
		if ( GUI.Button(new Rect(10,10,150, 30), "Save Config"))
		{
			_fileHolder = Hydrogen.Serialization.INI.Serialize(hInput.Instance.GetControls());
		}

		if ( GUI.Button(new Rect(170, 10, 150, 30), "Clear Controls"))
		{
			hInput.Instance.ClearControls();

		}

		if ( GUI.Button(new Rect(330, 10, 150, 30), "Set Controls"))
		{
			hInput.Instance.SetControls(Hydrogen.Serialization.INI.Deserialize(_fileHolder, '='));
		}

		GUI.color = Color.black;
		GUI.Label(new Rect(10,40,500,500), "INI Data\n\r" + _fileHolder);
	}

	private void OnRotate(Hydrogen.Peripherals.InputEvent evt, float value, float time)
	{
		// Mouse X Axes are relative movements only. So we only turn the turret - never directly set the rotation.
		turret.transform.localRotation *= Quaternion.AngleAxis(value * 180.0f * Time.deltaTime, Vector3.up);
	}
	
	//private void OnShoot(Hydrogen.Peripherals.InputEvent evt, float value, float time)
	//{
	//	if (evt == Hydrogen.Peripherals.InputEvent.Released)
	//	{
		//	GameObject shell = Instantiate(ammo, muzzle.transform.position, Quaternion.identity) as GameObject;
		//	shell.rigidbody.velocity = turret.transform.rotation * Vector3.forward * 10.0f * (time * time + 0.1f);
	//	}
	//}
	
	private void OnMove(Hydrogen.Peripherals.InputEvent evt, float value, float time)
	{
		// Vertical Axes give it's state of absolute values of -1.0, 1.0 only.
		// So this is used to move the box along it's looking direction (horizontal). This will handle forward and backward movement in one go.
		transform.transform.position += (transform.rotation * transform.forward) * value * 4.0f * Time.deltaTime;
	}
	
	private void OnTurn(Hydrogen.Peripherals.InputEvent evt, float value, float time)
	{
		// Horizontal Axes give it's state of absolute values of -1.0, 1.0 only.
		// We normalised this to a steering range, then use this to turn the tank.
		transform.transform.localRotation *= Quaternion.AngleAxis(value * 90.0f * Time.deltaTime, Vector3.up);
	}

}
