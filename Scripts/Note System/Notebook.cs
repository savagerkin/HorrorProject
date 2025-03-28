using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    private bool notebookOpen = false;
    [SerializeField] private string notebookName;
    public void SwitchOpenCloseNoteBook()
    {
        if(notebookOpen == false)
        {
            Debug.Log(notebookName + "Opened");
            notebookOpen = true;
        }
        else
        {
            Debug.Log(notebookName + "Closed");
            notebookOpen = false;
        }
    }
    
    public void CloseNotebook()
    {
        Debug.Log(notebookName + "Closed");
        notebookOpen = false;
    }



}
