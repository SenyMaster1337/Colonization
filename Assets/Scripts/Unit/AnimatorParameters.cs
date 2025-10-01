using UnityEngine;

public class AnimatorParameters : MonoBehaviour
{
    public readonly int IsRun = Animator.StringToHash(nameof(IsRun));

    [SerializeField] private Animator _animator;

    public void PlayRun()
    {
        _animator.SetBool(IsRun, true);
    }

    public void StopRun()
    {
        _animator.SetBool(IsRun, false);
    }
}
