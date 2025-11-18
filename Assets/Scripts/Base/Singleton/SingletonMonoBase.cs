using UnityEngine;
public class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this as T;        
        }
    }

    private void OnDestroy() {
        if(Instance == this)
        {
            Instance = null;
        }    
    }
}
