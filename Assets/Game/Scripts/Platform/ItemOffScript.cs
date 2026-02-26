using UnityEngine;

public class ItemOffScript : MonoBehaviour
{
    void Update()
    {
        if (!GameManager.IsGameStart)
        {
            return;
        }
        if (transform.position.y < PlayerController2d.Instance.transform.position.y - 10f)
        {
            gameObject.SetActive(false);
        }
    }
}