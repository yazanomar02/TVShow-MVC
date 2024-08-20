namespace TV_Program_Management.Repo
{
    public interface IStateRepository
    {
        string GetValue(string key);
        void SetValue(string key, string value);
        void Remove(string key);
    }
}
