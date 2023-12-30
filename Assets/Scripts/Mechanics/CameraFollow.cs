using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float minXClamp= 3f;
    private float maxXClamp= 7f;
    private float minYClamp = -1f;
    private float maxYClamp = 14f;
    [SerializeField] private Player player;

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 cameraPos;

            cameraPos = transform.position;
            cameraPos.x = Mathf.Clamp(player.transform.position.x, minXClamp, maxXClamp);
            cameraPos.y = Mathf.Clamp(player.transform.position.y, minYClamp, maxYClamp);

            transform.position = cameraPos;
        }
    }
}