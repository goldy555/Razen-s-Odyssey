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

    private void UpdateBar()
    {
        fillBar.fillAmount = currentMP / (float)maxMP;

        if (currentMP <= 0 && !isRegenerating)
        {
            StartCoroutine(StartMPRegeneration());
        }
        else if (currentMP >= maxMP && isRegenerating)
        {
            isRegenerating = false; // stop regenerating when bar is full
        }
    }

    public bool UseMP(int amount)
    {
        if (!isRegenerating && currentMP - amount >= 0) // check to ensure mp is not used during regeneration
        {
            currentMP -= amount;
            UpdateBar();
            return true;
        }
        return false;
    }

    private IEnumerator StartMPRegeneration()
    {
        isRegenerating = true;
        yield return new WaitForSeconds(5f);

        while (currentMP < maxMP)
        {
            currentMP++;
            UpdateBar();
            yield return new WaitForSeconds(10f / maxMP);  //  for gradual filling.
        }

        isRegenerating = false;
    }
    public bool IsRegenerating()
    {
        return isRegenerating;
    }
}
