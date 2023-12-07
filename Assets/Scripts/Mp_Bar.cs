using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mp_Bar : MonoBehaviour
{

    public int maxMP = 3;
    private int currentMP;
    public Image fillBar;

    private bool isRegenerating = false;

    private void Start()
    {
        currentMP = maxMP;
        UpdateBar();
    }
    //update the UI MP bar
    private void UpdateBar()
    {
        fillBar.fillAmount = currentMP / (float)maxMP;

        if (currentMP <= 0 && !isRegenerating)
        {
            StartCoroutine(StartMPRegeneration());
        }
        else if (currentMP >= maxMP && isRegenerating)
        {
            isRegenerating = false; 
        }
    }
    //till isRegenrating is false ( mp is greater than 0) will decrease certain amount of MP each time fireball is shot
    public bool UseMP(int amount)
    {
        if (!isRegenerating && currentMP - amount >= 0) 
        {
            currentMP -= amount;
            UpdateBar();
            return true;
        }
        return false;
    }
    // regenrate the MP bar in game if it falls to 0
    private IEnumerator StartMPRegeneration()
    {
        isRegenerating = true;
        yield return new WaitForSeconds(5f);

        while (currentMP < maxMP)
        {
            currentMP++;
            UpdateBar();
            yield return new WaitForSeconds(10f / maxMP);  
        }

        isRegenerating = false;
    }
    public bool IsRegenerating()
    {
        return isRegenerating;
    }
}
