using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.Player.transform.position;
        Vector3 mapPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - mapPos.x;
                float diffY = playerPos.y - mapPos.y;
                float dirX = diffX > 0 ? 1 : -1;
                float dirY = diffY > 0 ? 1 : -1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40f);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40f);
                }
                break;

            case "Eneny":
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - mapPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3),Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
