using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator creature, insertedVHSTape;
    // Start is called before the first frame update
    void Start()
    {
        creature.enabled = false;
        insertedVHSTape.enabled = false;
    }

    public void EnableAnimator(Animator gameObject) {
        gameObject.enabled = true;
    }
}
