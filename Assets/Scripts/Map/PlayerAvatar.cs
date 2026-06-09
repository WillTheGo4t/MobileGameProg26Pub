using Unity.VisualScripting;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{

    [SerializeField] float _rotationSpeed = 5f;

    float _currentHeading = 0;
    float _targetHeading;

    // Update is called once per frame
    void Update()
    {
        // könnten hier auch this.transform.rotation.y statt _currentHeading nehmen, aber das wäre deutlich unleserlicher
        if (_currentHeading == _targetHeading)
            return;

        RotateTowardsTarget();
    }


    void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.Euler(0, _targetHeading, 0);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * _rotationSpeed);

        _currentHeading = this.transform.rotation.y;
    }

    
    public void SetLookDirection(float heading)
    {
        _targetHeading = heading;
    } 
}
