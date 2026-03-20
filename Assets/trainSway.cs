using UnityEngine;

public class SwayX : MonoBehaviour
{
    [SerializeField] private Animator charAnimator;
    [SerializeField] private GameController gameController;
    public float swayAmount = 0.5f;   
    public float swaySpeed = 1f;      

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        charAnimator.SetFloat("Speed", Mathf.Abs(gameController.gameSpeed * 0.15f));
        if (gameController.gameStarted)
        {
            float xOffset = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            transform.position = startPosition + new Vector3(xOffset, 0f, 0f);
        }
    }
}
