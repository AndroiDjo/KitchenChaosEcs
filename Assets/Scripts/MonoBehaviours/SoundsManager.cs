using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundsManager : MonoBehaviour {
    [SerializeField] private AudioClipRefsSO audioRefs;

    public static SoundsManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void PlayCuttingSound(Vector3 position) {
        PlayRandomSound(audioRefs.chops, position);
    }
    
    public void PlayTrashSound(Vector3 position) {
        PlayRandomSound(audioRefs.trash, position);
    }
    
    public void PlayDeliverySuccessSound(Vector3 position) {
        PlayRandomSound(audioRefs.deliverySuccess, position);
    }
    
    public void PlayDeliveryFailSound(Vector3 position) {
        PlayRandomSound(audioRefs.deliveryFail, position);
    }
    
    public void PlayPickupSound(Vector3 position) {
        PlayRandomSound(audioRefs.objectPickup, position);
    }
    
    public void PlayDropSound(Vector3 position) {
        PlayRandomSound(audioRefs.objectDrop, position);
    }
    
    public void PlayStepSound(Vector3 position) {
        PlayRandomSound(audioRefs.footsteps, position);
    }

    private void PlayRandomSound(AudioClip[] clips, Vector3 position, float volume = 1f) {
        PlaySound(clips[Random.Range(0, clips.Length)], position, volume);

        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SelectedItemInteractSystem>();
    }
    
    private void PlaySound(AudioClip clip, Vector3 position, float volume=1f) {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
    
}
