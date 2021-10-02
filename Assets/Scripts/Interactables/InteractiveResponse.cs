using UnityEngine;

public class InteractiveResponse : MonoBehaviour
{
    [SerializeField] private AudioSource responseSound;

    public virtual void DoResponseAction()
    {
        PlaySound();
    }

    public virtual void UndoResponseAction()
    {
        PlaySound();
    }

    private void PlaySound()
    {
        if (responseSound == null)
            return;
    
        responseSound.pitch = Random.Range(0.9f, 1.1f);
        responseSound.Play();
    }

    public virtual bool IsAvailable()
    {
        return gameObject.activeInHierarchy;
    }
}
