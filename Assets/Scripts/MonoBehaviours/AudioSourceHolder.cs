using UnityEngine;

public class AudioSourceHolder : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;

    private bool isPlaying;
    public void Play() {
        if (!isPlaying) {
            audioSource.Play();
            isPlaying = true;
        }
    }

    public void Pause() {
        if (isPlaying) {
            audioSource.Pause();
            isPlaying = false;
        }
    }
}