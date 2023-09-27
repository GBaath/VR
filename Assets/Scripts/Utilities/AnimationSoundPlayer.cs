using UnityEngine;

public class AnimationSoundPlayer : MonoBehaviour {
    public void PlayClip(AudioData audioData) {
        CustomPlayClipAtPoint(audioData.GetRandomClip(), transform.position, audioData.volume, audioData.pitchBase, Random.Range(-audioData.pitchRange, audioData.pitchRange));
    }

    public void CustomPlayClipAtPoint(AudioClip clip, Vector3 position, [UnityEngine.Internal.DefaultValue("1.0F")] float volume, float pitchBase, float pitch) {
        GameObject gameObject = new("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        if (TryGetComponent(out Enemy enemy)) {
            audioSource.pitch = pitchBase / enemy.randomPitch + pitch;
        } else {
            audioSource.pitch = pitchBase + pitch;
        }
        audioSource.Play();
        Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}
