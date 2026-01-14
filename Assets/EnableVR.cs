using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
public class EnableVR : MonoBehaviour
{
    void Start()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }
}
