using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MudDumplingDivision : MonoBehaviour
{
    //バラバラになるときに使う変数
    [SerializeField] float radiuce = 5.0f; //爆発の影響範囲（円の半径）
    [SerializeField] float force = 15.0f;　//爆発の力

    //サウンド追加分 1
    [SerializeField] private CriAtomSource crayBallSound;
    private bool broken = false;
    private List<CriAtomEx.GameVariableInfo> gameVariableInfoList = new List<CriAtomEx.GameVariableInfo>();

    void Start(){
        int gameVariableCount = CriAtomEx.GetNumGameVariables();
        for(int i = 0; i < gameVariableCount; i++){
            CriAtomEx.GameVariableInfo gameVariableInfo;
            CriAtomEx.GetGameVariableInfo((ushort)i, out gameVariableInfo);
            gameVariableInfoList.Add(gameVariableInfo);
        }

        ChangeGameVariable("CrayBallState", 0f);
        crayBallSound.Play();
    }

    void Update(){
        if(broken && crayBallSound.status.ToString().Equals("PlayEnd")){
            Destroy(this.gameObject);
            //Debug.Log(crayBallSound.status.ToString());
        }
    }
    //サウンド追加分 1 終了


    private void OnCollisionEnter(Collision other)
    {
        foreach (Transform debris in GetComponentInChildren<Transform>())
        {
            debris.transform.parent = null;
            debris.GetComponent<MeshCollider>().enabled = true;
            Rigidbody rb = debris.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddExplosionForce(force, debris.transform.position, radiuce, 0f);
        }

        //サウンド追加分 2
        //Debug.Log(other.gameObject.name);
        if (!broken)
        {
            crayBallSound.Stop();
            ChangeGameVariable("CrayBallState", 1f);
            crayBallSound.Play();
            broken = true;
        }
    }

    //サウンド追加分 3
    //指定の名前のゲーム変数を探し、その値を変更する
    private void ChangeGameVariable(string gameVariableName, float value){
        if(gameVariableInfoList.Any(gameVariableInfoList => gameVariableInfoList.name == gameVariableName)){
            var id = gameVariableInfoList.FirstOrDefault(gv => gv.name == gameVariableName).id;
            CriAtomEx.SetGameVariable(id, value);
        }
    }
}
