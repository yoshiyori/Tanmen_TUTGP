using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameDrawing : MonoBehaviour
{
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントを取得する
        this.line = GetComponent<LineRenderer>();

        //線の幅を決める
        this.line.startWidth = 0.1f;
        this.line.endWidth = 0.1f;

        //頂点の数を決める
        this.line.positionCount = 4;
        line.SetPosition(0, this.transform.position + new Vector3(-70, 50, 0));
        line.SetPosition(1, this.transform.position + new Vector3(70, 50, 0));
        line.SetPosition(2, this.transform.position + new Vector3(70, -50, 0));
        line.SetPosition(3, this.transform.position + new Vector3(-70, 50, 0));

    }

}
