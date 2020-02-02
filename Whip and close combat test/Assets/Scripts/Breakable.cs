using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    //public GameObject child1, child2;
    public GameObject[] childs;
    public Transform child1SpawnPosition, child2SpawnPosition;
   
    // Start is called before the first frame update
    void Start()
    {
       
        //var sprite = Sprite.Create(spriteRenderer.sprite.texture, rect, Vector2.one * 0.5f, 100, 1,SpriteMeshType.FullRect, border,true);

    }

    // Update is called once per frame
    void Update()
    {
       /* spriteRenderer = GetComponent<SpriteRenderer>();
        var rect = new Rect(0, 0, spriteRenderer.sprite.rect.width, spriteRenderer.sprite.rect.height);
        var border = new Vector4(leftBorder, bottomBorder, rightBorder, topBorder);
        spriteRenderer.sprite = Sprite.Create(spriteRenderer.sprite.texture, rect, Vector2.one * 0.5f, 32, 0, SpriteMeshType.FullRect, border,true);*/
    }

    public void Break()
    {
        foreach (GameObject child in childs)
        {
            if(child.gameObject.GetComponents<SlicedObject>() != null)
            {
                GameObject childClone = Instantiate(child, child1SpawnPosition.position, Quaternion.identity);
                if (!childClone.activeInHierarchy)
                {
                    childClone.SetActive(true);
                }

                childClone.transform.localScale = new Vector3(1, 1, 1);
                childClone.GetComponent<SlicedObject>().ChanceSprite();
            }
          
        }

        /*GameObject childClone1 = Instantiate(child1,child1SpawnPosition.position,Quaternion.identity);
        if(!childClone1.activeInHierarchy)
        {
            childClone1.SetActive(true);
        }

        childClone1.GetComponent<SlicedObject>().ChanceSprite();

        GameObject childClone2 = Instantiate(child2, child2SpawnPosition.position, Quaternion.identity);
        if (!childClone2.activeInHierarchy)
        {
            childClone2.SetActive(true);
        }

        childClone2.GetComponent<SlicedObject>().ChanceSprite();
        */

        Destroy(gameObject);
    }
}
