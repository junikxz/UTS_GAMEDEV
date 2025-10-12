using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_introManager : MonoBehaviour
{
    private string nextScene = "Level1";
    public TextMeshProUGUI paragraphText;
    [TextArea(3, 10)] public string[] paragraphs;
    private int currParagraphIdx = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(paragraphs.Length > 0)
        {
            paragraphText.text = paragraphs[currParagraphIdx];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextParagraph();
        }
    }
    
    private void DisplayNextParagraph()
    {
        currParagraphIdx++;

        if(currParagraphIdx < paragraphs.Length)
        {
            paragraphText.text = paragraphs[currParagraphIdx];
        } else
        {
            Debug.Log("End of intro. Moving to next scene: " + nextScene);
            SceneManager.LoadScene(nextScene);
        }
    }
}
