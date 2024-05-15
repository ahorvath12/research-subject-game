using UnityEngine;
using UnityEngine.UI;

public class TvScreen : MonoBehaviour
{
    public AudioClip tvOnClip, tvOffClip;

    void OnEnable()
    {
        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        if (!audioSource)
        {
            return;
        }

        audioSource.clip = tvOnClip;
        audioSource.Play();
    }

    public void TurnOff()
    {
        AudioSource audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.clip = tvOffClip;
            audioSource.Play();
        }


        RawImage image = this.gameObject.GetComponent<RawImage>();
        if (image)
        {
            image.enabled = false;
        }

    }
}
