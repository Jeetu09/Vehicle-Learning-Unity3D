using UnityEngine;

public class Detecto : MonoBehaviour
{
    public Transform Player;
    public Transform Table;

    public GameObject UI;

    void Update()
    {
        float Dist = Vector3.Distance(Player.position, Table.position);
        // Debug.Log(Dist);

        if (Dist <= 3f)
        {
            UI.SetActive(false);
            Debug.Log("Plauyer is close");
        }
    }

}
