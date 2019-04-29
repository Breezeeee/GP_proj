using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider mSlider;
    public Slider lSlider;
    public Slider DSlider;
    private GameObject[] hairRoots;

    void Start()
    {
        hairRoots = GameObject.FindGameObjectsWithTag("HairRoot");

        mSlider.value = hairRoots[0].GetComponent<HairPoint>().g;
        lSlider.value = hairRoots[0].GetComponent<HairPoint>().length;
        DSlider.value = hairRoots[0].GetComponent<HairPoint>().D;
    }

    void Update()
    {
        if (hairRoots.Length == 1)
        {
            hairRoots = GameObject.FindGameObjectsWithTag("HairRoot");
        }
        for (int i = 0; i < hairRoots.Length; i++)
        {
            hairRoots[i].GetComponent<HairPoint>().g = mSlider.GetComponent<Slider>().value;
            hairRoots[i].GetComponent<HairPoint>().length = lSlider.GetComponent<Slider>().value;
            hairRoots[i].GetComponent<HairPoint>().D = DSlider.GetComponent<Slider>().value;
        }
    }
}
