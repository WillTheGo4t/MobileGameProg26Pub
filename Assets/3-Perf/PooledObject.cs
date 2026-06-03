using Unity.VisualScripting;
using UnityEngine;

public class PooledObject : MonoBehaviour
{

    // Der eigene Pool aus dem man kommt
    ObjectPool _ownPool;

    public void Initialize(ObjectPool ownPool)
    {
        _ownPool = ownPool;
    }

    public void ReturnToPool()
    {
        _ownPool.ReturnPooledObject(this.gameObject);
    }

    public void SetActiveSelf(bool value)
    {
        // irgendeine Logik, z.B. aktviere / deaktiviere Collision / Scripts / Sound etc.
        this.gameObject.SetActive(value);
    }
}
