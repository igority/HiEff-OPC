﻿namespace OPCtoMongoDBService.Models
{
    public class Tag
    {
        private int _index;
        private string _name;
        private string _value;

        public Tag(int index, string name, string value)
        {
            this._index = index;
            this._name = name;
            this._value = value;
        }

        public int Index { get { return _index; } set { value = _index; } }
        public string Name { get { return _name; } set { value = _name; } }
        public string Value { get { return _value; } set { value = _value; } }
    }
}

