﻿
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
        public string newPlayerName = "New player";
        public int kills = 0;
        public int wins = 0;
        public int loses = 0;
        public float volume = 0.2f;
        public float sensetive = 1.75f;

        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            
        }
    }
}
