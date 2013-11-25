#region Copyright Notice & License Information
// 
// ObjectPoolExample.cs
//  
// Author:
//   Matthew Davey <matthew.davey@dotbunny.com>
//
// Copyright (C) 2013 dotBunny Inc. (http://www.dotbunny.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("")]
public class MailChimpExample : MonoBehaviour {

	public string apiKey = "";
	public string listID = "";

	private string _emailAddress = "sample@sample.com";
	private string _response = "";


	public void MailChimpCallback(int hash, string response)
	{
		_response = hash.ToString() + ": " + response;
	}

	void OnGUI()
	{
		_emailAddress = GUI.TextField(new Rect(25,25, 150, 25), _emailAddress);

		if ( GUI.Button(new Rect(25, 60, 75, 20), "Submit") )
		{
			Dictionary<string, string> data = new Dictionary<string, string>();

			data.Add("apikey", apiKey);
			data.Add("id", listID);
			data.Add("email", _emailAddress);

			hWebPool.Instance.POST(
				"https://" +  apiKey.Substring(apiKey.LastIndexOf('-') + 1) + ".api.mailchimp.com/2.0/lists/subscribe.json",
				"application/json",
				Hydrogen.Serialization.JSON.Serialize(data),
				null,
				MailChimpCallback);
		}
		GUI.Label(new Rect(25,100, 500, 500), _response);

	}
}
