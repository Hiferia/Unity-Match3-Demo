using UnityEngine;

public enum Clip { Select, Swap, Clear };

public class SFXManager : MonoBehaviour 
{
	public static SFXManager Instance;

	private AudioSource[] sfx;

	// Use this for initialization
	void Start () 
	{
		Instance = GetComponent<SFXManager>();
		sfx = GetComponents<AudioSource>();
    }

	public void PlaySFX(Clip audioClip) 
	{
		sfx[(int)audioClip].Play();
	}
}
