namespace Metroidvania.Interfaces
{
    public interface ISaveAndLoad
    {
        void LoadData(GameData.GameData gameData);
        void SaveData(ref GameData.GameData gameData);
    }
}