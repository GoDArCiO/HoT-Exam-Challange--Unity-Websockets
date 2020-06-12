using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class destroyIt : NetworkBehaviour
{
    public GameObject tegoZniszcz;
    public GameObject me;
    public void eloZiom()
    {
        
        Cmdelo(tegoZniszcz);
    }
    [Command]
    public void Cmdelo(GameObject tegoZniszcz)
    {
        Rpcelo(tegoZniszcz);
    }
    [ClientRpc]
    public void Rpcelo(GameObject tegoZniszcz)
    {
        Destroy(tegoZniszcz);
    }
}
