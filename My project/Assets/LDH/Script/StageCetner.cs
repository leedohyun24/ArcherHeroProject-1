using System;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class StageController : MonoBehaviour
{
    [Header("방 프리팹 리스트 (설계도)")]
    public List<GameObject> roomPrefabs;

    [Header("맵 위치")]
    public Transform roomParent;

    [Header("플레이어 Transform")]
    public Transform player;

    private GameObject currentRoom;
    private Transform spawn = null;

    public UnityEvent onStageClear;

    private int currentStage = 0;
    public bool GameClear = false;
    public void LoadNextRoom()
    {
        if (roomParent.childCount > 0)
        {
            foreach (Transform child in roomParent)
            {
                Destroy(child.gameObject);
            }
        }

        if(currentStage < roomPrefabs.Count)
        {
            currentRoom = Instantiate(roomPrefabs[currentStage], Vector3.zero, Quaternion.identity, roomParent);
            Debug.Log($"새로운 맵 로드됨 (Stage {currentStage}) → {currentRoom.name}");
            currentStage++;
        }
        else
        {
            Debug.Log("다 불러옴");

            if(onStageClear  != null)
            {
                onStageClear.Invoke();
            }
            

            GameClear = true;
        }

        foreach (Transform t in currentRoom.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == "PlayerStart") 
            {
                spawn = t;
                break;
            }
            else
            {
                Debug.Log("못찾음");
            }
        }
        if (spawn != null)
        {
            player.position = spawn.position;
            player.rotation = spawn.rotation;
          
        }

    }
}