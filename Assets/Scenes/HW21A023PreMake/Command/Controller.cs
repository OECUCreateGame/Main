using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    public int ClickCount;
    public bool NextLine = false;
    private bool obsClick = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(obsClick)
        {
            ClickCount++;
        }
    }

    private void FixedUpdate()
    {

    }

    public bool IsClicked
    {
        get { return this.obsClick; }
        private set { this.obsClick = value; }
    }
}
