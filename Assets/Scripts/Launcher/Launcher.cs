using ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private void Awake()
    {
        Bootstrapper.Initiailze();
    }
}
