using UnityEngine;

public class ListUI : MonoBehaviour
{
    public void AddItem(GameObject gameObject)
    {
        Instantiate(gameObject, transform);
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
