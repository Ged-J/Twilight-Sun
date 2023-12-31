
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private static Animator myAnimator;
    private static SceneTransitions instance;
    private static readonly int Spin = Animator.StringToHash("Spin");
    private static readonly int Vertical1 = Animator.StringToHash("Vertical");
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");
    private static readonly int VerticalTransition = Animator.StringToHash("VerticalTransition");
    private static readonly int FadeIn = Animator.StringToHash("FadeIn");
    private static readonly int FastFadeIn = Animator.StringToHash("FastFadeIn");

    private void Awake()
    {
        if (instance == null)
        {
            myAnimator = GetComponent<Animator>();
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void ResetTriggers()
    {
        myAnimator.ResetTrigger(Spin);
        myAnimator.ResetTrigger(VerticalTransition);
        myAnimator.ResetTrigger(FadeOut);
        myAnimator.ResetTrigger(FadeIn);
        myAnimator.ResetTrigger(FastFadeIn);
    }
    
    public static void BattleTransition()
    {
        ResetTriggers();
        myAnimator.SetTrigger(Spin);
    }
    
    public static void vertical()
    {
        ResetTriggers();
        myAnimator.SetTrigger(VerticalTransition);
    }
    
    public static void Fadeout()
    {
        ResetTriggers();
        myAnimator.SetTrigger(FadeOut);
    }

    public static void fastFadeIn()
    {
        ResetTriggers();
        myAnimator.SetTrigger(FastFadeIn);
    }
    
    public static void Fadein()
    {
        ResetTriggers();
        myAnimator.SetTrigger(FadeIn);
    }
}
