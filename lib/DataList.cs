namespace Library
{
    public class DataStore<TKey, TValue>
    {
        List<Pair<TKey, TValue>> _list;
        int _size;

        Pair<TKey, TValue>? _NULL_element;
        TKey? _NULL_key;
        TValue? _NULL_value;
        public DataStore()
        {
            // star with room for ten elements:
            _list = new List<Pair<TKey, TValue>>();
            // initially the DataStore is empty:
            _size = 0;
        }

        public void Add(Pair<TKey, TValue> element)
        {
            _list.Add(element);
            _size = _list.Count;
        }

        public void Add(TKey key, TValue value)
        {
            Pair<TKey, TValue> element = new Pair<TKey, TValue>(key, value);
            _list.Add(element);
            _size = _list.Count;
        }

        public Pair<TKey, TValue>? GetElementByIndex(int index)
        {
            if (index > _size - 1)
            {
                return _NULL_element;
            }
            return _list[index];
        }
        public TKey? GetKeyByIndex(int index)
        {
            if (index > _size - 1)
            {
                return _NULL_key;
            }
            return _list[index].GetKey();
        }
        public TValue? GetValueByIndex(int index)
        {
            if (index > _size - 1)
            {
                return _NULL_value;
            }
            return _list[index].GetValue();
        }
    } // public class DataStore<TKey, TValue>

} // namespace Library