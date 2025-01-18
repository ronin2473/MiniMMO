using UnityEngine;
using TMPro;

public class DisplayUserName : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;


    public void UpdateUserNameDisplay(string newName)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = newName;
        }
        else
        {
            Debug.LogWarning("Textmesh is not assigned");
        }
    }
}
