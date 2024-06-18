
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Мои сохранения
        public int money = 0;                       
        public string newPlayerName = "New player";

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            
        }
    }
}
