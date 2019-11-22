using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public float rotationSpeed;
    private GameObject LevelArchitecture; //tous les murs de notre scène aka les objets qui vont rotate
    private GameObject LevelProps; //Tous les éléments de décor aka caisses ou autre aka tout ce qui ne va pas rotate avec la scène 
    private GameObject player;
    private Vector3 levelCenter;

    private bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        LevelArchitecture = transform.Find("LevelArchitecture").gameObject;
        LevelProps = transform.Find("LevelProps").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");

        ComputeLevelCenter();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !rotating)//clique gauche
        {
            StartCoroutine(RotateLevel(0));
        }
        if (Input.GetMouseButtonDown(1) && !rotating)//clique droit
        {
            StartCoroutine(RotateLevel(1));
        }

    }

    /// <summary>
    ///   Tourne la scène dans le sens indiqué par sens. 0 gauche, 1 droite
    /// </summary>
    /// <param name="sens"></param>
    IEnumerator RotateLevel(int sens)
    {
        //désactiver la gravité, les inputs et la vélocité sur le player et les props
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        for (int i = 0; i < LevelProps.transform.childCount; i++)
        {
            LevelProps.transform.GetChild(i).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        //rotation
        if (sens == 0)
        {
            float desiredRotation = transform.eulerAngles.z + 90;
            if (desiredRotation > 271)
            {
                desiredRotation = 0;
            }

            while (!Approx(0.1f, transform.eulerAngles.z, desiredRotation))
            {
                rotating = true;
                transform.RotateAround(levelCenter, new Vector3(0, 0, 1), rotationSpeed);
                yield return null;
            }
            player.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);
        }
        else if (sens == 1)
        {
            float desiredRotation = transform.eulerAngles.z - 90;
            Debug.Log(desiredRotation);
            if (desiredRotation < -1)
            {
                desiredRotation = 270;
            }

            while (!Approx(0.1f, transform.eulerAngles.z, desiredRotation))
            {
                rotating = true;
                transform.RotateAround(levelCenter, new Vector3(0, 0, 1), -rotationSpeed);
                yield return null;
            }
            player.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);
        }
        rotating = false;

        //réactiver la gravité sur le joueur et les props 
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        for (int i = 0; i < LevelProps.transform.childCount; i++)
        {
            LevelProps.transform.GetChild(i).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            LevelProps.transform.GetChild(i).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    /// <summary>
    /// retroune vraie si a et b sont à peu près égaux à epsilon près, false sinon
    /// </summary>
    /// <param name="epsilon"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private bool Approx(float epsilon, float a, float b)
    {
        if (Mathf.Abs(a - b) <= epsilon)
            return true;
        else
            return false;
    }

    /// <summary>
    /// permet de calculer le center du niveau en ce basant sur les murs extérieurs
    /// </summary>
    private void ComputeLevelCenter()
    {
        GameObject outsideWalls = LevelArchitecture.transform.Find("OutsideWalls").gameObject;

        Vector3 barycentre = new Vector3();
        for (int i = 0; i < outsideWalls.transform.childCount; i++)
        {
            barycentre += outsideWalls.transform.GetChild(i).transform.position;
        }
        barycentre /= outsideWalls.transform.childCount;

        levelCenter = barycentre;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(levelCenter, 0.1f);
    }
}
