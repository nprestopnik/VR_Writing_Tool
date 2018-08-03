using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnvironmentAudioManager : MonoBehaviour {

	//set from scene
	//these two values are used to make a cube of sorts where transient sounds will be able to be played
	public Vector3 transientAreaMin; //the corner of the cube with the lowest x, y, and z values
	public Vector3 transientAreaMax; //the corner with the highest x, y, and z values

	public Light lightningGenerator;
	
	//set from lighting/time of day preset
	[HideInInspector]
	public AudioClip[] transientSoundClips; //all of the possible transient sounds to play in this scene at the current setting
	[HideInInspector]
	public float minDelay; //the minimum possible time between sounds
	[HideInInspector]
	public float maxDelay; //the maximum possible time between sounds
	[HideInInspector]
	public float transientVolume; //the volume at which to play transient sounds

	[HideInInspector]
	public float spacialBlend;
	[HideInInspector]
	public bool lightning;

	void Start () {
		StartCoroutine(PlayTransientSounds());
	}

	IEnumerator PlayTransientSounds() {
		while (true) {
			//generate a random delay between sounds within the range
			float delay = Random.Range(minDelay, maxDelay); 
			if (transientSoundClips.Length != 0) {
				//choose a random clip from the array of transient sounds, generate a random position for it, and play it at that position
				AudioClip clip = transientSoundClips[Random.Range(0, transientSoundClips.Length)];
				Vector3 position = RandomVector3InBox();
				//AudioSource.PlayClipAtPoint(clip, position, transientVolume);
				PlayAudioClip(clip, position, transientVolume, spacialBlend);
				if(lightning){
					//make lightning!
					StopCoroutine(LightningFlash());
					StartCoroutine(LightningFlash());
				}
			}
			//wait the generated delay before playing the next sound
			yield return new WaitForSeconds(delay);
		}
	}

	//generate a random position within the given transient sound box
	Vector3 RandomVector3InBox() {
		float x = Random.Range(transientAreaMin.x, transientAreaMax.x);
		float y = Random.Range(transientAreaMin.y, transientAreaMax.y);
		float z = Random.Range(transientAreaMin.z, transientAreaMax.z);
		return new Vector3(x, y, z);
	}

	AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume, float spacialBlend) {
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.spatialBlend = spacialBlend;
		source.Play();
		Destroy(go, clip.length);
		return source;
 	}

	 IEnumerator LightningFlash() {
		 lightningGenerator.intensity = 10;
		 yield return new WaitForSeconds(0.05f);
		 lightningGenerator.intensity = 4;
		 yield return new WaitForSeconds(0.03f);
		 lightningGenerator.intensity = 8;
		 yield return new WaitForSeconds(0.03f);
		 lightningGenerator.intensity = 6;
		 yield return new WaitForSeconds(0.1f);
		 lightningGenerator.intensity = 2;
		 yield return new WaitForSeconds(0.05f);
		 lightningGenerator.intensity = 1;
		 yield return new WaitForSeconds(0.03f);
		 lightningGenerator.intensity = 0;
	 }




}
