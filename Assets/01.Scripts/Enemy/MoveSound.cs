using UnityEngine;

public class MoveSound : MonoBehaviour
{
    public AudioSource PatrolRightFoot;
    public AudioSource PatrolLeftFoot;
    public AudioSource TraceRightFoot;
    public AudioSource TraceLeftFoot;

    private void PRF()
    {
        PatrolRightFoot.PlayOneShot(PatrolRightFoot.clip);
    }
    private void PLF()
    {
        PatrolLeftFoot.PlayOneShot(PatrolLeftFoot.clip);
    }
    private void TRF()
    {
        TraceRightFoot.PlayOneShot(TraceRightFoot.clip);
    }
    private void TLF()
    {
        TraceLeftFoot.PlayOneShot(TraceLeftFoot.clip);
    }
}
