using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyTimeSliderController : MonoBehaviour
{
    public Button controller;
    public GameObject slider;
    public DataCenter dataCenter;

    // Start is called before the first frame update
    void Start()
    {
        controller.onClick.AddListener(toggle);
        slider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggle()
    {
        slider.SetActive(!slider.activeSelf);
        dataCenter.isReplaying = !dataCenter.isReplaying;
    }
}
