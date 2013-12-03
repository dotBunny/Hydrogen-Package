#region Copyright Notice & License Information
//
// InputExample.cs
//
// Author:
//       Matthew Davey <matthew.davey@dotbunny.com>
//       Robin Southern <betajaen@ihoed.com>
//
// Copyright (c) 2013 dotBunny Inc. (http://www.dotbunny.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using UnityEngine;

/// <summary>
/// Input Example.
/// </summary>
[AddComponentMenu ("")]
public class InputExample : MonoBehaviour
{
		public GameObject TowerObject;
		public GameObject SpawnPoint;
		public GameObject[] Ammo;
		string _fileHolder;
		int[] prefabIDs;

		void Start ()
		{
				// Make sure array is sized properly to hold the reference IDs. 
				prefabIDs = new int[Ammo.Length];

				// Add Objects to Pool
				for (int x = 0; x < Ammo.Length; x++) {
						prefabIDs [x] = hObjectPool.Instance.Add (Ammo [x]);
				}

				// Add our Actions for linking with controls
				hInput.Instance.AddAction ("Move", OnMove);
				hInput.Instance.AddAction ("Turn", OnTurn);
				hInput.Instance.AddAction ("Rotate", OnRotate);
				hInput.Instance.AddAction ("Shoot", OnShoot);
		
				// Create some controls
				hInput.Instance.AddControl ("Mouse X", "Rotate");
				hInput.Instance.AddControl ("Horizontal", "Turn");
				hInput.Instance.AddControl ("Vertical", "Move");
				hInput.Instance.AddControl ("Left", "Shoot");
				hInput.Instance.AddControl ("Space", "Shoot");
		}

		public void OnGUI ()
		{
				if (GUI.Button (new Rect (10, 10, 150, 30), "Save Config")) {
						_fileHolder = Hydrogen.Serialization.INI.Serialize (hInput.Instance.GetControls ());
				}

				if (GUI.Button (new Rect (170, 10, 150, 30), "Clear Controls")) {
						hInput.Instance.ClearControls ();
				}

				if (GUI.Button (new Rect (330, 10, 150, 30), "Set Controls")) {
						hInput.Instance.SetControls (Hydrogen.Serialization.INI.Deserialize (_fileHolder));
				}

				GUI.color = Color.black;
				GUI.Label (new Rect (10, 40, 500, 500), "INI Data\n\r" + _fileHolder);
		}

		void OnRotate (Hydrogen.Peripherals.InputEvent evt, float value, float time)
		{
				// Mouse X Axes are relative movements only. 
				// So we only turn the turret - never directly set the rotation.
				TowerObject.transform.localRotation *= Quaternion.AngleAxis (value * 180.0f * Time.deltaTime, Vector3.up);
		}

		void OnShoot (Hydrogen.Peripherals.InputEvent evt, float value, float time)
		{
				if (evt == Hydrogen.Peripherals.InputEvent.Released) {
						GameObject shell = hObjectPool.Instance.Spawn (
								                   prefabIDs [Random.Range (0, prefabIDs.Length)], 
								                   SpawnPoint.transform.position, Quaternion.identity);

						shell.rigidbody.velocity = SpawnPoint.transform.rotation * Vector3.forward * 250.0f * (time * time + 0.1f);
				}
		}

		void OnMove (Hydrogen.Peripherals.InputEvent evt, float value, float time)
		{
				// Vertical Axes give it's state of absolute values of -1.0, 1.0 only.
				// So this is used to move the box along it's looking direction (horizontal). This will handle forward 
				// and backward movement in one go.
				transform.transform.position += (transform.TransformDirection (Vector3.left) * value * 10.0f) * Time.deltaTime;
		}

		void OnTurn (Hydrogen.Peripherals.InputEvent evt, float value, float time)
		{
				// Horizontal Axes give it's state of absolute values of -1.0, 1.0 only.
				// We normalised this to a steering range, then use this to turn the tank.
				transform.transform.localRotation *= Quaternion.AngleAxis (value * 90.0f * Time.deltaTime, Vector3.up);
		}
}
