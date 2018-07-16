using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnvironmentAudioManager : MonoBehaviour {

	//set from scene
	public Vector3 transientAreaMin;
	public Vector3 transientAreaMax;
	
	//set from lighting/time of day preset
	[HideInInspector]
	public AudioClip[] transientSoundClips;
	[HideInInspector]
	public float minDelay;
	[HideInInspector]
	public float maxDelay;
	[HideInInspector]
	public float transientVolume;

	void Start () {
		StartCoroutine(PlayTransientSounds());
	}

	IEnumerator PlayTransientSounds() {
		while (true) {
			float delay = Random.Range(minDelay, maxDelay);
			if (transientSoundClips.Length != 0) {
				AudioClip clip = transientSoundClips[Random.Range(0, transientSoundClips.Length)];
				Vector3 position = RandomVector3InBox();
				AudioSource.PlayClipAtPoint(clip, position, transientVolume);
			}
			yield return new WaitForSeconds(delay);
		}
	}

	Vector3 RandomVector3InBox() {
		float x = Random.Range(transientAreaMin.x, transientAreaMax.x);
		float y = Random.Range(transientAreaMin.y, transientAreaMax.y);
		float z = Random.Range(transientAreaMin.z, transientAreaMax.z);
		return new Vector3(x, y, z);
	}



}
