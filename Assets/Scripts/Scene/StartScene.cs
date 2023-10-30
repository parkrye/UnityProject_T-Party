using UnityEngine;

public class StartScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.Scene.LoadScene("TitleScene");
    }
}
