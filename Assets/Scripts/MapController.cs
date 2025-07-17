using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public bl_MiniMap Map;
    private bool Rotation = true;
    public Button MapButton;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        Map.UpdateRate = 0;
        MapButton.onClick.AddListener(OnToggle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Map != null)
        {
            ChangeRotation();
        }

        if (Map.UpdateRate == 0)
        {
            Map.UpdateRate = 1;
        } 
    }

    void ChangeRotation()
    {
        Rotation = !Rotation;
        Map.GetComponentInChildren<bl_MiniMap>().SetMapRotationMode(Rotation);
    }

    void OnToggle()
    {
        UI.SetActive(!UI.activeSelf);
    }
}
