using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
public class DisableVR : MonoBehaviour
{
    void Start()
    {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }
}
