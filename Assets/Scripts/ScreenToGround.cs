using UnityEngine;

public class ScreenToGround : MonoBehaviour
{
    public TowerScriptableObject pickedTower;

    private Ray ray;
    private RaycastHit hit;
    private GameObject tower;
    // Start is called before the first frame update
    void Start()
    {
        tower = null;
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool IsColliding = Physics.Raycast(ray, out hit, Mathf.Infinity);
        if (Input.GetMouseButtonDown(0) && IsColliding)
        {
            tower = Instantiate(pickedTower.TowerPrefab);
        }
        else if (Input.GetMouseButton(0) && IsColliding && tower)
        {
            tower.transform.position = hit.point;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.DrawLine(ray.origin, hit.point);
            if (IsColliding)
            {
                tower = null;
            }
            else
            {
                Destroy(tower, 1f);
            }
        }
    }
}
