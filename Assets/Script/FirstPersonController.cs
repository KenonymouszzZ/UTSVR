using UnityEngine;

/// <summary>
/// First-Person Controller sederhana.
/// - Gerak dengan WASD (relatif ke arah hadap kamera)
/// - Lihat sekeliling dengan mouse (look)
/// - Lompat dengan Space (butuh CharacterController)
/// - Esc untuk toggle "mode main" / "mode UI" (kunci/lepas kursor)
///
/// PENTING (perbaikan interaksi UI World Space):
/// Kamera & gerak HANYA aktif saat kursor TERKUNCI (mode main).
/// Saat kursor dibuka (mode UI), look & movement DIBEKUKAN supaya
/// mouse bisa dipakai mengklik Slider/Toggle/Button pada panel
/// World Space tanpa kamera ikut berputar liar.
///
/// CARA PAKAI:
/// 1. Buat GameObject kosong (mis. "Player"), tambahkan komponen CharacterController.
/// 2. Jadikan Main Camera sebagai ANAK player, lalu drag Main Camera ke field "playerCamera".
/// 3. (Opsional) Atur posisi kamera setinggi mata (~1.6 unit di sumbu Y lokal).
/// 4. Tekan Play — gerak pakai WASD + mouse, lompat pakai Space.
/// 5. Dekati panel audio, tekan Esc untuk masuk "mode UI", lalu klik slider/toggle.
///    Tekan Esc lagi untuk kembali "mode main".
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Camera / Look")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 85f; // batas lihat atas/bawah

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float gravity = -19.62f;  // ~2x gravitasi bumi biar lompat terasa enak
    [SerializeField] private float jumpHeight = 1.2f;

    // Referensi komponen
    private CharacterController controller;

    // Kecepatan vertikal (untuk gravitasi & lompat)
    private float verticalVelocity;

    // Rotasi kamera (pitch = lihat atas/bawah)
    private float pitch;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Kunci kursor di tengah layar & sembunyikan (standar FPS / mode main)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Esc selalu bisa dipakai untuk pindah mode main <-> mode UI
        HandleCursorLock();

        // Hanya gerak & lihat saat kursor TERKUNCI (mode main).
        // Saat kursor dibuka (mode UI), kamera & gerak dibekukan supaya
        // mouse bisa dipakai mengklik panel World Space dengan tenang.
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            HandleMouseLook();
            HandleMovement();
        }
        else
        {
            // Mode UI: tetap proses gravitasi ringan supaya player tidak
            // "mengambang" jika sebelumnya sedang di udara, tapi tanpa input gerak.
            ApplyGravityOnly();
        }
    }

    // ========================
    // MOUSE LOOK
    // ========================
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Putar body (player) ke kiri/kanan
        transform.Rotate(Vector3.up * mouseX);

        // Putar kamera atas/bawah, lalu kunci supaya tidak over-rotate
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        if (playerCamera != null)
        {
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
    }

    // ========================
    // MOVEMENT (WASD + gravitasi + lompat)
    // ========================
    void HandleMovement()
    {
        // Input gerak (WASD / arrow keys)
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        // Arah gerak relatif ke arah hadap player
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move = move.normalized * walkSpeed;

        // Gravitasi: jika di tanah dan sedang jatuh, reset velocity ke kecil
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; // sedikit negatif biar tetap nempel ke tanah
        }

        // Lompat (hanya kalau menyentuh tanah)
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            // Rumus lompat: v = sqrt(2 * g * h)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Terapkan gravitasi
        verticalVelocity += gravity * Time.deltaTime;

        // Gabungkan gerak horizontal + vertikal, lalu pindahkan
        Vector3 finalMove = (move + Vector3.up * verticalVelocity) * Time.deltaTime;
        controller.Move(finalMove);
    }

    // ========================
    // GRAVITASI SAJA (dipakai saat mode UI, tanpa input gerak)
    // ========================
    void ApplyGravityOnly()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    // ========================
    // CURSOR LOCK (Esc untuk toggle mode main <-> mode UI)
    // ========================
    void HandleCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Masuk mode UI: lepas & tampilkan kursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Kembali ke mode main: kunci & sembunyikan kursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}