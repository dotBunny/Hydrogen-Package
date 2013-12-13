using UnityEngine;
using System.Collections;

public class AmbientAudioTester : MonoBehaviour
{
		public AmbientAudioManager.ChunkAudioSettings chunk1;
		public AmbientAudioManager.ChunkAudioSettings chunk2;
		public AmbientAudioManager.ChunkAudioSettings chunk3;
		public Hydrogen.Core.AudioStackItem inside1;
		public Hydrogen.Core.AudioStackItem inside2;
		AmbientAudioManager _manager;
		// Use this for initialization
		void Start ()
		{
				_manager = GetComponent<AmbientAudioManager> ();
				_manager.CurrentChunkAudio = chunk1;
				_manager.CurrentStructureInteriorAudio = inside1;
		}

		void OnGUI ()
		{
				if (GUI.Button (new Rect (10, 10, 100, 30), "Daytime")) {
						_manager.IsDaytime = true;
				}
				if (GUI.Button (new Rect (120, 10, 100, 30), "Nightime")) {
						_manager.IsDaytime = false;
				}
				if (GUI.Button (new Rect (230, 10, 100, 30), "Underground")) {
						_manager.IsUnderground = true;
				}
				if (GUI.Button (new Rect (340, 10, 100, 30), "Aboveground")) {
						_manager.IsUnderground = false;
				}
				if (GUI.Button (new Rect (10, 50, 100, 30), "Inside")) {
						_manager.IsInsideStructure = true;
				}

				if (GUI.Button (new Rect (120, 50, 100, 30), "Outside")) {
						_manager.IsInsideStructure = false;
				}

				if (GUI.Button (new Rect (10, Screen.height - 40, 100, 30), "Chunk 1")) {
						_manager.CurrentChunkAudio = chunk1;
				}
				if (GUI.Button (new Rect (120, Screen.height - 40, 100, 30), "Chunk 2")) {
						_manager.CurrentChunkAudio = chunk2;
				}
				if (GUI.Button (new Rect (230, Screen.height - 40, 100, 30), "Chunk 3")) {
						_manager.CurrentChunkAudio = chunk3;
				}
				if (GUI.Button (new Rect (340, Screen.height - 40, 100, 30), "Inside 1")) {
						_manager.CurrentStructureInteriorAudio = inside1;
				}
				if (GUI.Button (new Rect (450, Screen.height - 40, 100, 30), "Inside 2")) {
						_manager.CurrentStructureInteriorAudio = inside2;
				}

				GUI.Label (new Rect (10, 120, 100, 35), "Active Sources: " + hAudioStack.Instance.SourcesCount);
		}
}
