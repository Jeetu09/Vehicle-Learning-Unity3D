using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeToLevelOne : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("RTO Office");
    }
}
