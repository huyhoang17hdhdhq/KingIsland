using TMPro;
using UnityEngine;

public class CommonUIPanel : MonoBehaviour
{
    public static CommonUIPanel Instance;

    public TextMeshProUGUI nameIslandText;
    public TextMeshProUGUI castleText;
    public TextMeshProUGUI productionRateText;
 

    private void Awake()
    {
        Instance = this;
    }

    public void Show(EventInfo info)
    {
        nameIslandText.text = info.nameIsland;
        castleText.text = info.castle;
        productionRateText.text = info.productionRate;
  
        gameObject.SetActive(true);
    }
}
