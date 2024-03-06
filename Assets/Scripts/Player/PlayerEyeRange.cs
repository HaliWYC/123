using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyeRange : MonoBehaviour
{
    public Player player;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        player.eyeRange = CheckEyeRange();

        checkEnemyNumber();
    }

    #region PlayerCheck
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, player.eyeRange);
    }

    private float CheckEyeRange()
    {
        return Mathf.Max(player.playerInformation.MeleeRange, player.playerInformation.RangedRange) + Settings.eyeRangeBase;
    }

    public void checkEnemyNumber()
    {
        
        var colliders = Physics2D.OverlapCircleAll(transform.position, player.eyeRange);
        List<GameObject> attackList = player.attackTargetList;

        foreach (var target in colliders)
        {
            if (target.CompareTag("Enemy") || target.CompareTag("NPC"))
            {
                if (attackList != null && attackList.Count > 0)
                {
                    if ((target.transform.position - player.transform.position).sqrMagnitude > CheckEyeRange())
                        attackList.Remove(target.gameObject);
                }
                else if (!CheckEnemyExist(attackList, target.gameObject))
                    attackList.Add(target.gameObject);
            }
        }
    }

    private bool CheckEnemyExist(List<GameObject> attackList,GameObject target)
    {
        foreach(GameObject enemy in attackList)
        {
            if (target == enemy)
                return true;
        }
        return false;
    }

    private void AddAttackTargetIntoList(List<GameObject> attackList, GameObject target)
    {
        bool SpaceCapable = false;
        for(int index = 0; index < attackList.Count; index++)
        {
            if (attackList[index] == null)
            {
                attackList[index] = target;
                SpaceCapable = true;
                break;
            }
        }
        if (!SpaceCapable)
            attackList.Add(target);
    }
    
    #endregion
}
