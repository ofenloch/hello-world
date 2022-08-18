namespace Library
{
    class Pair<TKey, TValue>
    {

        // this is the key of the current value;
        private TKey _key;
        // this is the current value:
        private TValue _value;

        public Pair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }
        public TKey GetKey()
        {
            return _key;
        }
        public TValue GetValue()
        {
            return _value;
        }

    } // class Pair<TKey, TValue>

} // namespace Library