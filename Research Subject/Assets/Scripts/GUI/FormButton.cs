using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked() {
        Button button = this.gameObject.GetComponent<Button>();
        if (!button) {
            return;
        }

        button.enabled = false;
    }
}
