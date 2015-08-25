namespace Hfr.Database
{
    interface IDataRepository
    {
        void Initialize();
        void Drop();
        void Clear();
    }
}
