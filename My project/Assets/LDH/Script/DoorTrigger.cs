using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private bool colider = false;
    void OnCollisionEnter(Collision collision)
    {
        if (colider) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("문에 닿음");
      
            colider = true;
           
            StageController stageManager = FindAnyObjectByType<StageController>();
            if (stageManager != null)
            {
                Debug.Log("찾음");
                stageManager.LoadNextRoom();
            }
            else
            {
                Debug.Log("스테이지 로드될게없음");
            }
        }
    }
}