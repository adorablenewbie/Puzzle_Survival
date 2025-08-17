using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    private DialogueManager dialogueManager;

    void Start()
    {
        camera = Camera.main;
        dialogueManager = PlayerManager.Instance.Player.dialogueManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
            
        }
        if (context.phase == InputActionPhase.Started && dialogueManager.gameObject.activeSelf)
        {
            if (dialogueManager.isDialogue)
            {
                if (dialogueManager.isTyping) //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~``수정하자
                {
                    dialogueManager.isTyping = false; // 타이핑 중지
                    dialogueManager.dialogueText.text = dialogueManager.fullText; // 현재 대사 전체 표시
                    return;
                }
                if (dialogueManager.isLastLine)
                {
                    dialogueManager.gameObject.SetActive(false);
                    dialogueManager.isLastLine = false;
                    dialogueManager.isDialogue = false;
                    dialogueManager.npcCameara.gameObject.SetActive(false); // NPC 대화용 카메라 비활성화
                    dialogueManager.npcAnimator.SetInteger("actionValue", 0); // NPC 애니메이터 대화 상태 해제
                    return;
                }
                dialogueManager.ShowNextLine();
                return;
            }
            dialogueManager.ShowDialogue(dialogueManager.currentBranch, dialogueManager.currentIndex);
            dialogueManager.isDialogue = true;
        }
    }
}