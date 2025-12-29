using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;

    private Vector3 camPos = new Vector3(0, 13.5f, -12f); // 변경하지 마세요

    void Update()
    {
        transform.position = Player.position + camPos;
    }
}
