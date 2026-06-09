using UnityEngine;

public class HeightAdjuster : MonoBehaviour
{
    [SerializeField] float _cameraOffset = 200f;

    public void AdjustHeight()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 2000, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + _cameraOffset;
            transform.position = pos;
        }
    }
}
