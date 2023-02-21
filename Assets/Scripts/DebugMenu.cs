using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public void AddExperience(int exp)
    {
        GameManager.instance.GainExperience(exp);
    }
}
