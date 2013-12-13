using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// An example of how image data maps can be used to control the playing of all sorts of different things.
/// </summary>
public class AmbientAudioManager : MonoBehaviour
{
		public AudioClip[] demoBank;

		public AudioClip Get (string key)
		{
				return demoBank [Convert.ToInt32 (key)];
		}
		/*
		 * Create a look up table to simulate mod (get clip by string)
		 * manager setting are just the key (
		 * 
		 * always just use the stacks key reference to look up things never actually storing the item here
		 * 
		 * 
		 */
		public float FadeInTime = 4.0f;
		public float FadeOutTime = 5.0f;
		public float RainIntensity = 0f;
		float _currentRainIntensity;
		float _previousRainIntensity;
		bool _updatedRainIntensity;
		public float WindIntensity = 0f;
		public float ThunderIntensity = 0f;
		/// <summary>
		/// Was the color changed?
		/// </summary>
		bool _colorUpdated;
		/// <summary>
		/// The current color.
		/// </summary>
		Color _currentColor = Color.black;
		/// <summary>
		/// The current ChunkAudioSettings to be used.
		/// </summary>
		ChunkAudioSettings _currentChunkAudio;
		/// <summary>
		/// The current StructureInteriorAudio to be used.
		/// </summary>
		Hydrogen.Core.AudioStackItem _currentStructureInteriorAudio;
		/// <summary>
		/// Is it daytime?
		/// </summary>
		bool _isDaytime;
		/// <summary>
		/// Are we underground?
		/// </summary>
		bool _isUnderground;
		/// <summary>
		/// Living in a world inside of walls?
		/// </summary>
		bool _isInsideStructure;
		/// <summary>
		/// The previous ChunkAudioSettings to be used.
		/// </summary>
		ChunkAudioSettings _previousChunkAudio;
		/// <summary>
		/// The previous color.
		/// </summary>
		Color _previousColor = Color.black;
		/// <summary>
		/// The current StructureInteriorAudio to be used.
		/// </summary>
		Hydrogen.Core.AudioStackItem _previousStructureInteriorAudio;
		Hydrogen.Core.AudioStackItem _structureItem;
		/// <summary>
		/// Was any of the audio chunk information updated recently?
		/// </summary>
		bool _updatedChunkAudio;
		/// <summary>
		/// Was the daytime changed?
		/// </summary>
		bool _updatedDaytime;
		/// <summary>
		/// Was the inside structure status changed?
		/// </summary>
		bool _updatedInsideStructure;
		/// <summary>
		/// Was the structures audio settings updated?
		/// </summary>
		bool _updatedStructureInteriorAudio;
		/// <summary>
		/// Was the underground status changed?
		/// </summary>
		bool _updatedUnderground;

		public Color CurrentColor {
				get {
						return _currentColor;
				}
				set {
						if (value != _currentColor) {

								CurrentChunkAudio.AgDayCoastal.TargetVolume = value.r;
								CurrentChunkAudio.AgNightCoastal.TargetVolume = value.r;
								CurrentChunkAudio.UgShallow.TargetVolume = value.r;

								CurrentChunkAudio.AgDayForest.TargetVolume = value.r;
								CurrentChunkAudio.AgNightForested.TargetVolume = value.r;
								CurrentChunkAudio.UgDeep.TargetVolume = value.r;

								CurrentChunkAudio.AgDayCivilized.TargetVolume = value.r;
								CurrentChunkAudio.AgNightCivilized.TargetVolume = value.r;
								CurrentChunkAudio.UgEnclosed.TargetVolume = value.r;

								CurrentChunkAudio.AgDayOpen.TargetVolume = value.r;
								CurrentChunkAudio.AgNightOpen.TargetVolume = value.r;
								CurrentChunkAudio.UgOpen.TargetVolume = value.r;

								PushVolumesToStack ();

								_colorUpdated = true;
								_previousColor = _currentColor;
								_currentColor = value;
						}
				}
		}

		/// <summary>
		/// Gets or sets the current ChunkAudioSettings.
		/// </summary>
		/// <value>The current ChunkAudioSettings.</value>
		public ChunkAudioSettings CurrentChunkAudio {
				get { return _currentChunkAudio; }
				set { 
						if (value != _currentChunkAudio) {
								_updatedChunkAudio = true;

								if (value == _previousChunkAudio) {
										//TODO: Might want to handle this a bit cleaner
								}



								_previousChunkAudio = _currentChunkAudio;
								_currentChunkAudio = value;
						}

				}
		}

		/// <summary>
		/// Gets or sets the current AmbientAudioSetting for structures.
		/// </summary>
		/// <value>The current structure AmbientAudioSetting</value>
		public Hydrogen.Core.AudioStackItem CurrentStructureInteriorAudio {
				get { return _currentStructureInteriorAudio; }
				set { 
						if (value != _currentStructureInteriorAudio) {
								_updatedStructureInteriorAudio = true;

								if (value == _previousStructureInteriorAudio) {
										//TODO: Might want to handle this a bit cleaner
								}
								_previousStructureInteriorAudio = _currentStructureInteriorAudio;
								_currentStructureInteriorAudio = value;
						}
				}
		}

		/// <summary>
		/// Gets or sets a value indicating whether it is daytime.
		/// </summary>
		/// <value><c>true</c> if its daytime; otherwise, <c>false</c>.</value>
		public bool IsDaytime {
				get { return _isDaytime; }
				set {
						if (value != _isDaytime) {

								if (!_isUnderground) {
										if (value) {
												ToggleAboveGroundDayAmbience (true);
												ToggleAboveGroundNightAmbience (false);
										} else {
												ToggleAboveGroundDayAmbience (false);
												ToggleAboveGroundNightAmbience (true);
										}

								}

								_updatedDaytime = true;
								_isDaytime = value;
						}
				}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the player is inside structure.
		/// </summary>
		/// <value><c>true</c> if inside a structure otherwise, <c>false</c>.</value>
		public bool IsInsideStructure {
				get { return _isInsideStructure; }
				set {
						if (value != _isInsideStructure) {

								// Update Our Structure
								ToggleStructureAmbience (value);

								// Additional Effects (If we are not underground)
								if (!_isUnderground) {
										ToggleAboveGroundEffects (!value);
								}
								// Turn On Above ground
								if (!_isUnderground && !value) {
										if (_isDaytime) {
												ToggleAboveGroundDayAmbience (true);
										} else {
												ToggleAboveGroundNightAmbience (true);
										}
								}

								if (_isUnderground && !value) {
										// Turn on underground
										ToggleUnderGroundAmbience (true);
								}

								// Add turn on underground or aboveground
								_updatedInsideStructure = true;
								_isInsideStructure = value;
						}
				}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the player is underground.
		/// </summary>
		/// <value><c>true</c> if underground; otherwise, <c>false</c>.</value>
		public bool IsUnderground {
				get { return _isUnderground; }
				set {
						if (value != _isUnderground) {

								ToggleUnderGroundAmbience (value);

								// Turn off above aground 
								if (value) {
										if (_isDaytime) {
												ToggleAboveGroundDayAmbience (false);

										} else {
												ToggleAboveGroundNightAmbience (false);
										}
										ToggleAboveGroundEffects (false);

								} else {
										if (_isDaytime) {
												ToggleAboveGroundDayAmbience (true);
										} else {
												ToggleAboveGroundNightAmbience (true);
										}
										ToggleAboveGroundEffects (true);
								}

								_updatedUnderground = true;
								_isUnderground = value;
						}
				}
		}

		void PushVolumesToStack ()
		{
				if (_isInsideStructure && hAudioStack.Instance.IsLoaded (CurrentStructureInteriorAudio.Key)) {
						hAudioStack.Instance.LoadedItems [CurrentStructureInteriorAudio.Key].TargetVolume = 
								CurrentStructureInteriorAudio.TargetVolume;

				} else if (_isUnderground) {

						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.UgDeep.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.UgDeep.Key].TargetVolume = 
										CurrentChunkAudio.UgDeep.TargetVolume;
						}
						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.UgEnclosed.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.UgEnclosed.Key].TargetVolume = 
										CurrentChunkAudio.UgEnclosed.TargetVolume;
						}

						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.UgOpen.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.UgOpen.Key].TargetVolume = 
										CurrentChunkAudio.UgOpen.TargetVolume;
						}

						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.UgShallow.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.UgShallow.Key].TargetVolume = 
										CurrentChunkAudio.UgShallow.TargetVolume;
						}

				} else if (_isDaytime) {
						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgDayOpen.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgDayOpen.Key].TargetVolume = 
										CurrentChunkAudio.AgDayOpen.TargetVolume;
						}
						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgDayForest.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgDayForest.Key].TargetVolume = 
										CurrentChunkAudio.AgDayForest.TargetVolume;
						}

						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgDayCoastal.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgDayCoastal.Key].TargetVolume = 
										CurrentChunkAudio.AgDayCoastal.TargetVolume;
						}

						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgDayCivilized.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgDayCivilized.Key].TargetVolume = 
										CurrentChunkAudio.AgDayCivilized.TargetVolume;
						}
				} else {
						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgNightOpen.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgNightOpen.Key].TargetVolume = 
										CurrentChunkAudio.AgNightOpen.TargetVolume;
						}
						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgNightForested.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgNightForested.Key].TargetVolume = 
										CurrentChunkAudio.AgNightForested.TargetVolume;
						}

						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgNightCoastal.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgNightCoastal.Key].TargetVolume = 
										CurrentChunkAudio.AgNightCoastal.TargetVolume;
						}

						if (hAudioStack.Instance.IsLoaded (CurrentChunkAudio.AgNightCivilized.Key)) {
								hAudioStack.Instance.LoadedItems [CurrentChunkAudio.AgNightCivilized.Key].TargetVolume = 
										CurrentChunkAudio.AgNightCivilized.TargetVolume;
						}
				}
		}

		void ToggleAboveGroundDayAmbience (bool flag)
		{
				if (flag) {
						UpdateFlagBased (flag,
								CurrentChunkAudio.AgDayCoastal.Key, 
								Get (CurrentChunkAudio.AgDayCoastal.Key), 
								CurrentChunkAudio.AgDayCoastal.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgDayForest.Key, 
								Get (CurrentChunkAudio.AgDayForest.Key), 
								CurrentChunkAudio.AgDayForest.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgDayCivilized.Key, 
								Get (CurrentChunkAudio.AgDayCivilized.Key), 
								CurrentChunkAudio.AgDayCivilized.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgDayOpen.Key, 
								Get (CurrentChunkAudio.AgDayOpen.Key), 
								CurrentChunkAudio.AgDayOpen.TargetVolume);
				} else {
						UpdateFlagBased (flag,
								CurrentChunkAudio.AgDayCoastal.Key, 
								null, 
								CurrentChunkAudio.AgDayCoastal.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgDayForest.Key, 
								null, 
								CurrentChunkAudio.AgDayForest.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgDayCivilized.Key, 
								null, 
								CurrentChunkAudio.AgDayCivilized.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgDayOpen.Key, 
								null, 
								CurrentChunkAudio.AgDayOpen.TargetVolume);
				}
		}

		void ToggleAboveGroundEffects (bool flag)
		{
				if (flag) {
						UpdateFlagBased (flag, CurrentChunkAudio.Rain.Key, 
								Get (CurrentChunkAudio.Rain.Key), 
								CurrentChunkAudio.Rain.TargetVolume);
						UpdateFlagBased (flag, CurrentChunkAudio.Thunder.Key, 
								Get (CurrentChunkAudio.Thunder.Key), 
								CurrentChunkAudio.Wind.TargetVolume);
						UpdateFlagBased (flag, CurrentChunkAudio.Wind.Key, 
								Get (CurrentChunkAudio.Wind.Key), 
								CurrentChunkAudio.Wind.TargetVolume);
				} else {
						UpdateFlagBased (flag, CurrentChunkAudio.Rain.Key, 
								null, 
								CurrentChunkAudio.Rain.TargetVolume);
						UpdateFlagBased (flag, CurrentChunkAudio.Thunder.Key, 
								null, 
								CurrentChunkAudio.Wind.TargetVolume);
						UpdateFlagBased (flag, CurrentChunkAudio.Wind.Key, 
								null, 
								CurrentChunkAudio.Wind.TargetVolume);
				}
		}

		void ToggleAboveGroundNightAmbience (bool flag)
		{
				if (flag) {
						UpdateFlagBased (flag,
								CurrentChunkAudio.AgNightCoastal.Key, 
								Get (CurrentChunkAudio.AgNightCoastal.Key), 
								CurrentChunkAudio.AgNightCoastal.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgNightForested.Key, 
								Get (CurrentChunkAudio.AgNightForested.Key), 
								CurrentChunkAudio.AgNightForested.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgNightCivilized.Key, 
								Get (CurrentChunkAudio.AgNightCivilized.Key), 
								CurrentChunkAudio.AgNightCivilized.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgNightOpen.Key, 
								Get (CurrentChunkAudio.AgNightOpen.Key), 
								CurrentChunkAudio.AgNightOpen.TargetVolume);
				} else {
						UpdateFlagBased (flag,
								CurrentChunkAudio.AgNightCoastal.Key, 
								null, 
								CurrentChunkAudio.AgNightCoastal.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgNightForested.Key, 
								null, 
								CurrentChunkAudio.AgNightForested.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgNightCivilized.Key, 
								null, 
								CurrentChunkAudio.AgNightCivilized.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.AgNightOpen.Key, 
								null, 
								CurrentChunkAudio.AgNightOpen.TargetVolume);
				}
		}

		void ToggleStructureAmbience (bool flag)
		{
				UpdateFlagBased (flag, CurrentStructureInteriorAudio.Key, 
						CurrentStructureInteriorAudio.Clip,
						CurrentStructureInteriorAudio.TargetVolume);
		}

		void ToggleUnderGroundAmbience (bool flag)
		{
				if (flag) {
						UpdateFlagBased (flag,
								CurrentChunkAudio.UgShallow.Key, 
								Get (CurrentChunkAudio.UgShallow.Key), 
								CurrentChunkAudio.UgShallow.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.UgDeep.Key, 
								Get (CurrentChunkAudio.UgDeep.Key), 
								CurrentChunkAudio.UgDeep.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.UgDeep.Key, 
								Get (CurrentChunkAudio.UgEnclosed.Key), 
								CurrentChunkAudio.UgEnclosed.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.UgOpen.Key, 
								Get (CurrentChunkAudio.UgOpen.Key), 
								CurrentChunkAudio.UgOpen.TargetVolume);
				} else {
						UpdateFlagBased (flag,
								CurrentChunkAudio.UgShallow.Key, 
								null, 
								CurrentChunkAudio.UgShallow.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.UgDeep.Key, 
								null, 
								CurrentChunkAudio.UgDeep.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.UgDeep.Key, 
								null, 
								CurrentChunkAudio.UgEnclosed.TargetVolume);
						UpdateFlagBased (flag, 
								CurrentChunkAudio.UgOpen.Key, 
								null, 
								CurrentChunkAudio.UgOpen.TargetVolume);
				}
		}

		void UpdateFlagBased (bool flag, string key, AudioClip clip, float targetVolume)
		{
				if (flag && !hAudioStack.Instance.IsLoaded (key)) {
						var newAudio = new Hydrogen.Core.AudioStackItem (clip, key);

						// Settings On Interior Audio
						newAudio.Loop = true;
						newAudio.Fade = true;
						newAudio.FadeInTime = FadeInTime;
						newAudio.FadeOutTime = FadeOutTime;

						// Add to Stack
						hAudioStack.Instance.Add (newAudio);

				} else if (flag && hAudioStack.Instance.IsPlaying (key)) {
						hAudioStack.Instance.LoadedItems [key].TargetVolume = targetVolume;
				} else if (flag && hAudioStack.Instance.IsLoaded (key)) {
						hAudioStack.Instance.LoadedItems [key].Source.Play ();
				} else if (!flag && hAudioStack.Instance.IsLoaded (key)) {
						hAudioStack.Instance.LoadedItems [key].TargetVolume = 0f;
				}
		}
		//check to see which audio clips are supposed to be playing
		/*				

						
				use RainIntensity / WindIntensity / ThunderIntensity to set volumes of:
				- Rain
				- Wind
				- Thunder
				
				//if there are clips that are supposed to be playing but aren't, load the clip and start them at volume 0
				//don't wait for other clips to finish fading out - start playing them right away
				//if there are clips playing that aren't at max volume, fade them from 0 to AmbientAudioSetting.MaxVolume over FadeInTime seconds
				
				//in the case of Rain / Wind / Thunder, max volume will be RainIntensity * AmbientAudioSetting.MaxVolume
				//RainIntensity etc. will be different on each refresh so the max volume will have to be adjusted on each refresh


								
				AudioClip newClip = Mods.Get.Clip (CurrentChunkName, StructureInteriorAudio.ClipName);

				*/
		[Serializable]
		public class ChunkAudioItem
		{
				public string Key;
				public float TargetVolume = 1f;
		}

		[Serializable]
		public class ChunkAudioSettings
		{
				public ChunkAudioItem AgDayCoastal;
				//R
				public ChunkAudioItem AgDayForest;
				//G
				public ChunkAudioItem AgDayCivilized;
				//B
				public ChunkAudioItem AgDayOpen;
				//A
				public ChunkAudioItem AgNightCoastal;
				//R
				public ChunkAudioItem AgNightForested;
				//G
				public ChunkAudioItem AgNightCivilized;
				//B
				public ChunkAudioItem AgNightOpen;
				//A
				public ChunkAudioItem UgShallow;
				//R
				public ChunkAudioItem UgDeep;
				//G
				public ChunkAudioItem UgEnclosed;
				//B
				public ChunkAudioItem UgOpen;
				//A
				public ChunkAudioItem Rain;
				public ChunkAudioItem Wind;
				public ChunkAudioItem Thunder;
		}
}
