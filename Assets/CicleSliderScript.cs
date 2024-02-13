using UnityEngine;
using UnityEngine.UI;

public class CicleSliderScript : MonoBehaviour
{
    [SerializeField] Transform handle;
    [SerializeField] Text valTxt;
    Vector3 mousePos;

    public void onHandleDrag()
    {
        mousePos = Input.mousePosition;
        Vector2 dir=mousePos - handle.position;
        float angle = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
        angle = (angle <= 0) ? (360 + angle) : angle;
        if(angle<=10 || angle >= 50)
        {
            Quaternion r = Quaternion.AngleAxis(angle + 10f, Vector3.forward);
            handle.rotation = r;
            angle = ((angle>10)? (angle-360): angle)+45;
        }
    }
}
