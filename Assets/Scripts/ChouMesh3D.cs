using UnityEngine;
using UnityEngine.Serialization;

public class ChouMesh3D : MonoBehaviour
{
    [SerializeField] public GameObject ObjetBebe;
    [SerializeField] public GameObject ObjetPetit;
    [SerializeField] public GameObject ObjetPret;

    void Start()
    {
        CacherObjets();
    }

    public void CacherObjets()
    {
        ObjetBebe.SetActive(false);
        ObjetPetit.SetActive(false);
        ObjetPret.SetActive(false);
    }
}