using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlurEffect : MonoBehaviour {


    [Range(0.0f,0.95f)]
    public float blurAmount = 0.8f;

    public bool extraBlur = false;
    public Shader curShader;
	public GameObject player;

    private Material curMaterial;
    private RenderTexture tempRT;
	private bool blerTrigger;


    //ここでマテリアルを取得
    Material material {

        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }

    //マテリアルがなかったら削除
    void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }

    //ここでレンダリングしてる
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
		blerTrigger = player.GetComponent<MovePlayer>().blerTrigger;

		
			if (blerTrigger == true)
			{
				//要件を満たすテクスチャの生成
				if (tempRT == null || tempRT.width != source.width || tempRT.height != source.height)
				{
					DestroyImmediate(tempRT);
					tempRT = new RenderTexture(source.width, source.height, 0);
					tempRT.hideFlags = HideFlags.HideAndDontSave;
					Graphics.Blit(source, tempRT);
				}

				//追加のブラーを使うかどうか
				if (extraBlur)
				{
					//ざっくりいうと解像度を1/4になってます
					RenderTexture blurBuffer = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
					tempRT.MarkRestoreExpected();
					Graphics.Blit(tempRT, blurBuffer);
					Graphics.Blit(blurBuffer, tempRT);

					RenderTexture.ReleaseTemporary(blurBuffer);
				}

				//シェーダーの外部変数の登録
				material.SetTexture("_MainTex", tempRT);
				material.SetFloat("_BlurAmount", 1 - blurAmount);

				//上記の追加のブラーを使うかどうかの設定
				Graphics.Blit(source, tempRT, material);
				Graphics.Blit(tempRT, destination);
			}
			else
			{
				//特殊効果なしでテクスチャに張り付け
				Graphics.Blit(source, destination);
			}
		

    }

}
