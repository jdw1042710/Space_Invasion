using Unity.VisualScripting;
using UnityEngine;

public class UICameraTracker : MonoBehaviour
{
    private void Update()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        Vector3 oppositeDir = transform.position - Camera.main.transform.position;
        Vector3 oppositePos = transform.position + oppositeDir;
        transform.LookAt(oppositePos);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        LookAtCamera();
    }
    #endif
}
