using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotSelectorBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(WaitBeforeSelecting());
    }

    IEnumerator WaitBeforeSelecting()
    {
        yield return new WaitForSeconds(0.01f);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetComponent<Button>().gameObject);
        transform.GetChild(0).GetComponent<Button>().Select();
        transform.GetChild(0).GetComponent<Button>().OnSelect (null);
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }
}
