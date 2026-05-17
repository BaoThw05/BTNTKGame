using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Loa phát nhạc")]
    // Gán component AudioSource của chính Player vào đây trong Inspector
    public AudioSource audioSource;

    [Header("Mảng âm thanh bước chân chung")]
    // Bạn kéo các file tiếng đi bộ vào đây (Kéo 1 file hoặc kéo 3 file khác nhau để nghe tự nhiên)
    public AudioClip[] walkSteps;

    // Hàm này sẽ được gọi từ Animation Event (Như nãy giờ chúng ta vẫn làm)
    public void PlayStepSound()
    {
        // Kiểm tra xem đã kéo âm thanh vào chưa và loa có hoạt động không
        if (walkSteps.Length > 0 && audioSource != null)
        {
            // Chọn ngẫu nhiên 1 tiếng động trong mảng để nghe không bị nhàm chán
            int randomIndex = Random.Range(0, walkSteps.Length);

            // PlayOneShot giúp tiếng chân phát ra mà không làm ngắt các âm thanh khác (như nhạc nền)
            audioSource.PlayOneShot(walkSteps[randomIndex]);
        }
    }
}