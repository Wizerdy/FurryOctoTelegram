using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Animator shake;

    public void Shake()
    {
        shake.SetTrigger("SHAKESHAKESHAKE");
    }
}
