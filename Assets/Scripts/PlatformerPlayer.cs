using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sprite Sheet from free Unity Asset: CosmicMan by Quantum Fox Studio
/// https://assetstore.unity.com/packages/2d/characters/cosmicman-167448
/// Sound effects from free Unity Asset:  Sound FX - Retro Pack by GameMaker6696
/// https://assetstore.unity.com/packages/audio/sound-fx/sound-fx-retro-pack-121743
/// Music from free Unity Asset: Space Game BGM #1 by Lady-Freija
/// https://assetstore.unity.com/packages/audio/music/space-game-bgm-1-169419
/// </summary>
public class PlatformerPlayer : MonoBehaviour
{   

    //public AudioSource audioSource;
    private AudioSource _audioSource;
    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip fallSound;

    public float speed = 250.0f;
    public float jumpForce = 12.0f;

    private Rigidbody2D _body;
    private Animator _anim;
    private PolygonCollider2D _polygon;

    private bool _IsJumping = false;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _polygon = GetComponent<PolygonCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // horizontal movement
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        _anim.SetFloat("speed", Mathf.Abs(deltaX));
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
            if (!_audioSource.isPlaying)
                _audioSource.PlayOneShot(walkSound);    // Play walking Sound
        }

        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        _body.velocity = movement;

        // jump
        Vector3 max = _polygon.bounds.max;
        Vector3 min = _polygon.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        bool grounded = false;
        if (hit != null)
        {
            //Debug.Log("grounded");
            grounded = true;
            _anim.SetBool("jump", false);
            _IsJumping = false;
        }
        // turn gravity off when standing on slope
        _body.gravityScale = grounded && deltaX == 0 ? 0 : 1;
        // jump pressed while player standing on ground
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("jumping");
            _audioSource.PlayOneShot(jumpSound);        // Play Jump Sound
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _anim.SetBool("jump", true);
            _IsJumping = true;
        }

        if ((!grounded) && (!_IsJumping))
        {
            // Play falling sound (nothing under player and not jumping) - needs more work - possible isse with collision detection
            if (!_audioSource.isPlaying)
                _audioSource.PlayOneShot(fallSound); 
        }

        MovingPlatform platform = null;
        if (hit != null)
        {
            platform = hit.GetComponent<MovingPlatform>();
        }
        if (platform != null)
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }

        Vector3 pScale = Vector3.one;
        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
        }

    }

}
