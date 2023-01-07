using UnityEngine;

public class StartGameButtonUI : MonoBehaviour
{
    public void SetActiveForMaster(bool isMaster) {
        gameObject.SetActive(isMaster);
    }
}
