using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] GameObject profilePage;
    [SerializeField] GameObject[] relationshipPages;

    [SerializeField] Button backButton;
    [SerializeField] Button nextButton;

    private int currentPage = 0;
    public bool resetPage = false; 

    private void Start()
    {
        CheckSerializedFields(); 

        if (backButton)
        {
            backButton.onClick.AddListener(ShowPreviousPage);
            EventTrigger backButtonTrigger = backButton.gameObject.AddComponent<EventTrigger>();
            AddPointerEnterEvent(backButtonTrigger, PlayButtonSound);
        }

        if (nextButton)
        {
            nextButton.onClick.AddListener(ShowNextPage);
            EventTrigger nextButtonTrigger = nextButton.gameObject.AddComponent<EventTrigger>();
            AddPointerEnterEvent(nextButtonTrigger, PlayButtonSound);
        }

        if (resetPage)
        {
            ShowPage(0);
            resetPage = false;
        }
        else
        {
            ShowPage(currentPage);
        }
    }

    void CheckSerializedFields()
    {
        if (!profilePage)
            Debug.LogError("ProfilePage is not assigned in the ProfileManager.");

        foreach (var page in relationshipPages)
        {
            if (!page)
                Debug.LogError("One or more RelationshipPages are not assigned in the ProfileManager.");
        }

        if (!backButton)
            Debug.LogError("BackButton is not assigned in the ProfileManager.");

        if (!nextButton)
            Debug.LogError("NextButton is not assigned in the ProfileManager.");
    }

    void ShowPreviousPage()
    {
        currentPage--;
        if (currentPage < 0)
            currentPage = relationshipPages.Length;
        ShowPage(currentPage);
    }

    void ShowNextPage()
    {
        currentPage++;
        if (currentPage >= relationshipPages.Length + 1)
            currentPage = 0;
        ShowPage(currentPage);
    }

    void ShowPage(int pageIndex)
    {
        if (profilePage!= null)
        {
            profilePage.SetActive(false);
            foreach (var page in relationshipPages)
            {
                page.SetActive(false);
            }

            if (pageIndex == 0)
            {
                profilePage.SetActive(true);
            }
            else if (pageIndex > 0 && pageIndex <= relationshipPages.Length + 1)
            {
                relationshipPages[pageIndex - 1].SetActive(true);
            }
        }
    }

    public void ShowProfile()
    {
        if (profilePage!= null)
        {
            profilePage.SetActive(true);
        }
    }

    void AddPointerEnterEvent(EventTrigger trigger, UnityEngine.Events.UnityAction action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { action.Invoke(); });
        trigger.triggers.Add(entry);
    }

    void PlayButtonSound()
    {

    }

    public void SetPage(int pageNum)
    {
        currentPage = pageNum;
        resetPage = true; 
    }
}
