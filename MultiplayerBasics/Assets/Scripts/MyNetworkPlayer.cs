using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour

{

    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;

    [SyncVar(hook =nameof(HandleSetDisplayName))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook =nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color color =Color.black;

    #region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
       
        displayName = newDisplayName;
    }


    [Server]
    public void GenerateColor(Color color)
    {
        this.color = color;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {

      
        RpcLogNewName(newDisplayName);

        if (newDisplayName.Length < 2 || newDisplayName.Length > 20)
        {
            Debug.LogWarning("New display name is too short!");
            return;
        }
        for(int i =0; i<newDisplayName.Length;i++)
        {
            if (newDisplayName[i] != ' ')
            {
                if (!((newDisplayName[i] <= 'a' && newDisplayName[i] >= 'z') || (newDisplayName[i] <= 'A' && newDisplayName[i] >= 'Z') || (newDisplayName[i] <= '0' && newDisplayName[i] >= '9')))
                {
                    Debug.LogWarning("Illegal Charcter used");
                    return;
                }
            }
        }
        SetDisplayName(newDisplayName);
    }


    
    #endregion

    #region Client
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    private void HandleSetDisplayName(string oldName,string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("a@fasg");
    }

    [ClientRpc]
    private void RpcLogNewName(string newName)
    {
        Debug.Log(newName);
    }

    #endregion
}
