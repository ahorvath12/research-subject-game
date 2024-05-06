using System.Collections;
using UnityEngine;

// Optimization, documentation, credits, examples, and more coming in a future update. Stay tuned and check the site often: https://pixeldough.itch.io/retro-framerate-controller

public class RetroFramerateController : MonoBehaviour
{

    [Range(1, 60)]
    public int fps = 24;
    public float speed = 1f;

    public enum AnimationStyles
    {
        Percision,
        Functionality
    }
    public AnimationStyles animationStyle = AnimationStyles.Functionality;
    private AnimationStyles _animationStyle;

    private int _fps = 24;

    private Animator animator;
    private float frameDelta = 0f;
    private float frameTimer = 0f;
    private float animationDelta = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AnimateFixed());

        _animationStyle = animationStyle;

    }
    
    private void Update()
    {
        frameDelta = 1f / _fps;

        if (_animationStyle == AnimationStyles.Percision)
        {
            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                AnimatorClipInfo clip = animator.GetCurrentAnimatorClipInfo(0)[0];
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

                animationDelta = Time.deltaTime / clip.clip.length * speed * Time.timeScale;

                frameTimer += animationDelta;
            }
        }
    }


    private IEnumerator AnimateFixed()
    {
        float _time = Time.time;

        yield return new WaitForSecondsRealtime(frameDelta);

        AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (clips.Length > 0)
        {
            animator.speed = 1f;
            switch (_animationStyle)
            {
                case AnimationStyles.Percision:
                    if (frameTimer >= 1) frameTimer %= 1;
                    animator.Play(state.shortNameHash, 0, frameTimer);
                    break;
                case AnimationStyles.Functionality:
                    animationDelta = (Time.time - _time) * speed * Time.timeScale;
                    animator.Update(animationDelta);
                    break;
            }
            animator.speed = 0f;
        }

        _fps = fps;

        StartCoroutine(AnimateFixed());
    }


    public void ChangeState(string stateName)
    {
        animator.Play(stateName);
        frameTimer = 0;
    }


}
