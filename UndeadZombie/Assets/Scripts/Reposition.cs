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

        float dirX = playerPos.x - mapPos.x;
        float dirY = playerPos.y - mapPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        Vector3 plyaerDir = GameManager.instance.Player.inputVec;
        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Ground":
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
                    transform.Translate(plyaerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                }
                break;
        }
    }
}
