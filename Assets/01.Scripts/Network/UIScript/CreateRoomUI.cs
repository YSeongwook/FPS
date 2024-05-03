using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> crewImgs;

    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    private CreateGameRoomData roomData;

    private void Start()
    {
        roomData = new CreateGameRoomData() { maxPlayerCount = 4 };
    }

    public void UpdateMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        for (int i = 0; i < maxPlayerCountButtons.Count; i++) 
        {
            if (i == count-1)
            {
                maxPlayerCountButtons[i].image.color = new Color(1f,1f,1f,1f);
            }
            else
            {
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        for(int i = 0; i < crewImgs.Count; i++)
        {
            if(i < roomData.maxPlayerCount)
            {
                crewImgs[i].gameObject.SetActive(true);
            }
            else
            {
                crewImgs[i].gameObject.SetActive(false);
            }
        }

    }

}

public class CreateGameRoomData
{
    public int maxPlayerCount;
}