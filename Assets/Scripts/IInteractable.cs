/// <summary>
/// Oyuncunun etkileşime geçebileceği nesnelerin uygulaması gereken arayüz.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Oyuncu bu nesneyle etkileşime geçtiğinde çalışacak fonksiyon.
    /// </summary>
    /// <param name="player">Etkileşimi başlatan oyuncu</param>
    void Interact(PlayerInteraction player);
}