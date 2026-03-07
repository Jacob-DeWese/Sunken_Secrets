using UnityEngine;
using System.Collections.Generic;

public class Test_Script : MonoBehaviour
{
    public Transform player;
    public float fadeAlpha = 0.3f;

    private List<Renderer> fadedRenderers = new List<Renderer>();

    void LateUpdate()
    {
        RestoreObjects();

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform != player)
            {
                Renderer rend = hit.transform.GetComponent<Renderer>();
                if (rend != null)
                {
                    SetTransparent(rend);
                    fadedRenderers.Add(rend);
                }
            }
        }
    }

    void SetTransparent(Renderer rend)
    {
        foreach (Material mat in rend.materials)
        {
            Color c = mat.color;
            c.a = fadeAlpha;
            mat.color = c;
        }
    }

    void RestoreObjects()
    {
        foreach (Renderer rend in fadedRenderers)
        {
            foreach (Material mat in rend.materials)
            {
                Color c = mat.color;
                c.a = 1f;
                mat.color = c;
            }
        }

        fadedRenderers.Clear();
    }
}