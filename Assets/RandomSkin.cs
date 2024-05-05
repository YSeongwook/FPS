using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkin : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinMeshRenderers; // 하위 오브젝트의 스킨 매쉬 랜더러 배열

    void Start()
    {
        // 하위 오브젝트에서 스킨 매쉬 랜더러를 모두 가져옴
        skinMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        DisableAllButOne();
    }

    public void DisableAllButOne()
    {
        if (skinMeshRenderers.Length <= 1)
        {
            Debug.LogWarning("하위 오브젝트에 2개 이상의 스킨 매쉬 랜더러가 필요합니다.");
            return;
        }

        // 랜덤으로 인덱스 선택
        int randomIndex = Random.Range(0, skinMeshRenderers.Length);

        // 모든 스킨 매쉬 랜더러를 순회하며 선택된 하나를 제외하고 비활성화
        for (int i = 0; i < skinMeshRenderers.Length; i++)
        {
            if (i != randomIndex)
            {
                skinMeshRenderers[i].gameObject.SetActive(false);
            }
        }
    }
}
