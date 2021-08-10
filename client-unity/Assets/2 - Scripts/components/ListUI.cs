using UnityEngine;

public class ListUI : MonoBehaviour
{
    public GameObject AddItem(GameObject gameObject)
    {
        GameObject go = Instantiate(gameObject, transform);
        return go;
    }

    public void RemoveById(int id)
    {
        Destroy(transform.GetChild(id).gameObject);
    }

    public void RemoveAllItems()
    {
        int nChildren = transform.childCount;
        for (int i = nChildren - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
