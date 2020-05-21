using UnityEngine;

public class Target : MonoBehaviour {
    public int clickCapacity = 1;
    public int clicksRecorded = 0;
    public int id;

    public void OnMouseDown() {
        clicksRecorded++;
        ClientSend.BoxClicked(id);
        if (clicksRecorded >= clickCapacity) {
            gameObject.SetActive(false);
        }
    }
}
