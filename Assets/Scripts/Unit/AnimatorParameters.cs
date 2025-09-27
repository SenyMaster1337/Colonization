using UnityEngine;

public class AnimatorParameters : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public readonly int IsRun = Animator.StringToHash(nameof(IsRun));

    public void PlayRun()
    {
        _animator.SetBool(IsRun, true);
    }

    public void StopRun()
    {
        _animator.SetBool(IsRun, false);
    }
}
