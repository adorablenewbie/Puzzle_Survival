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
                if (dialogueManager.isTyping) //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~``��������
                {
                    dialogueManager.isTyping = false; // Ÿ���� ����
                    dialogueManager.dialogueText.text = dialogueManager.fullText; // ���� ��� ��ü ǥ��
                    return;
                }
                if (dialogueManager.isLastLine)
                {
                    dialogueManager.gameObject.SetActive(false);
                    dialogueManager.isLastLine = false;
                    dialogueManager.isDialogue = false;
                    dialogueManager.npcCameara.gameObject.SetActive(false); // NPC ��ȭ�� ī�޶� ��Ȱ��ȭ
                    dialogueManager.npcAnimator.SetInteger("actionValue", 0); // NPC �ִϸ����� ��ȭ ���� ����
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