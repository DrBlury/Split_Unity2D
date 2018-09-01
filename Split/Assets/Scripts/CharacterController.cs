using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour {
    [SerializeField] private float jumpSpeed = 400f;
    private float smoothMovement = .25f;
    [SerializeField] private bool canBeControlledMidAir = true;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private Transform bottom_check;
    [SerializeField] private Transform top_check;
    public AudioClip JumpSoundClip;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public bool canDoubleJump = true;
    const float botRadius = .5f;
    public bool isTouchingGround;
    const float topRadius = .5f;
    private Rigidbody2D rb;
    private Vector3 m_Velocity = Vector3.zero;


    [Header("Events")]
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake() {
        rb = this.GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate() {
        bool isGrounded = isTouchingGround;
        isTouchingGround = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(bottom_check.position, botRadius, GroundMask);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                isTouchingGround = true;
                if (!isGrounded)
                    OnLandEvent.Invoke();
            }
        }

        if (rb.velocity.y < 0) {
            rb.gravityScale = fallMultiplier;
        } else if (rb.velocity.y > 0 && !Input.GetButton ("Jump")) {            
            rb.gravityScale = lowJumpMultiplier;
        } else {
            rb.gravityScale = 1f;
        }
    }

    public void Move(float move, bool jump) {
        if (isTouchingGround) {
            canDoubleJump = true;
        }

        if (move > 0) {
            GetComponent<SpriteRenderer>().flipX = true;
        } else if (move < 0){
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (canBeControlledMidAir || isTouchingGround) {
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, smoothMovement);
        }

        if (isTouchingGround && jump || canDoubleJump && jump) {
            isTouchingGround = false;
            AudioSource.PlayClipAtPoint(JumpSoundClip, transform.position);
            rb.AddForce(new Vector2(0f, jumpSpeed));

            if (canDoubleJump) {
                canDoubleJump = !canDoubleJump;
            }
        }
    }
}